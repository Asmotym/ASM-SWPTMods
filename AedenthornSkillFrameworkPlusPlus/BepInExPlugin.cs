﻿using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace AedenthornSkillFrameworkPlusPlus
{
    [BepInPlugin("asmotym.AedenthornSkillFrameworkPlusPlus", "Skill Framework ++", "0.0.1")]
    public class BepInExPlugin : BaseUnityPlugin
    {
        public static ConfigEntry<bool> modEnabled;
        public static ConfigEntry<bool> isDebug;
        public static ConfigEntry<int> nexusID;

        public static BepInExPlugin context;

        public static Dictionary<string, Dictionary<string, int>> characterSkillLevels = new Dictionary<string, Dictionary<string, int>>();
        public static Dictionary<string, SkillInfo> customSkills = new Dictionary<string, SkillInfo>();

        public static void Dbgl(string str = "", bool pref = true)
        {
            if (isDebug.Value)
                Debug.Log((pref ? typeof(BepInExPlugin).Namespace + " " : "") + str);
        }
        private void Awake()
        {
            context = this;
            modEnabled = Config.Bind("General", "Enabled", true, "Enable this mod");
            isDebug = Config.Bind<bool>("General", "IsDebug", true, "Enable debug logs");

            //nexusID = Config.Bind<int>("General", "NexusID", 69, "Nexus mod ID for updates");

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), null);

        }
        public static int GetCharacterSkillLevel(string charName, string skillname)
        {
            if (!characterSkillLevels.ContainsKey(charName))
                characterSkillLevels[charName] = new Dictionary<string, int>();
            if (!characterSkillLevels[charName].ContainsKey(skillname))
                characterSkillLevels[charName][skillname] = 0;
            return characterSkillLevels[charName][skillname];
        }
        public static void SetCharacterSkillLevel(string charName, string skillname, int level)
        {
            if (!characterSkillLevels.ContainsKey(charName))
                characterSkillLevels[charName] = new Dictionary<string, int>();
            characterSkillLevels[charName][skillname] = level;
        }

        [HarmonyPatch(typeof(UICharacter), nameof(UICharacter.Refresh))]
        static class UICharacter_Refresh_Patch
        {
            static void Postfix(UICharacter __instance)
            {
                if (!modEnabled.Value || !customSkills.Any())
                    return;
                Transform[] parents = new Transform[]
                {
                    __instance.daggerproficiency.transform.parent,
                    __instance.ignorpain.transform.parent,
                    __instance.healaura.transform.parent,
                    __instance.goldenhand.transform.parent
                };
                foreach (SkillInfo info in customSkills.Values)
                {
                    Transform t = parents[info.category].Find(info.id);
                    if (!t)
                    {
                        if (parents[info.category].childCount % 6 == 0)
                        {
                            // decrease y of lower cats
                            float height = parents[info.category].GetComponent<GridLayoutGroup>().spacing.y + __instance.daggerproficiency.transform.GetComponent<RectTransform>().sizeDelta.y;
                            for (int i = info.category + 1; i < parents.Length; i++)
                            {
                                parents[i].parent.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0, height);
                                foreach (Transform c in parents[i])
                                {
                                    foreach (Transform cc in c)
                                    {
                                        if (cc.name.StartsWith("plus"))
                                            cc.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                                    }
                                }
                            }
                            parents[0].parent.parent.GetComponent<RectTransform>().sizeDelta += new Vector2(0, height);
                        }
                        t = Instantiate(__instance.daggerproficiency.transform, parents[info.category]);
                        t.name = info.id;
                        t.GetComponentInChildren<Button>().GetComponent<RawImage>().texture = info.icon;
                    }

                    Localization.LocalizationDic[info.id] = info.name;
                    Localization.LocalizationDic[info.id + "DESC"] = info.description;
                    int points = GetCharacterSkillLevel(__instance.curCustomization.name, info.id);
                    SkillBox sb = t.GetComponent<SkillBox>();
                    sb.maxPoints = info.maxPoints;
                    sb.isActiveSkill = info.isActiveSkill;
                    sb.Initiate(points);
                    __instance.curCustomization.UpdateStats();
                }
            }
        }
        //[HarmonyPatch(typeof(UICharacter), nameof(UICharacter.ShowTips))]
        static class UICharacter_ShowTips_Patch
        {
            static void Postfix(UICharacter __instance)
            {
                if (!modEnabled.Value || !customSkills.Any() || GameObject.Find(customSkills.Values.ToList()[0].id))
                    return;

                Dbgl("Showing tips");

                Transform[] parents = new Transform[]
                {
                    __instance.daggerproficiency.transform.parent,
                    __instance.ignorpain.transform.parent,
                    __instance.healaura.transform.parent,
                    __instance.goldenhand.transform.parent
                };

                foreach (SkillInfo info in customSkills.Values)
                {
                    int points = GetCharacterSkillLevel(__instance.curCustomization.name, info.id);
                    Transform t = Instantiate(__instance.daggerproficiency.transform, parents[info.category]);
                    t.name = info.id;
                    t.GetComponentInChildren<Button>().GetComponent<RawImage>().texture = info.icon;
                    Localization.LocalizationDic[info.id] = info.name;
                    Localization.LocalizationDic[info.id + "DESC"] = info.description;
                    SkillBox sb = t.GetComponent<SkillBox>();
                    sb.maxPoints = info.maxPoints;
                    sb.isActiveSkill = info.isActiveSkill;
                    sb.Initiate(points);
                }
            }
        }
        [HarmonyPatch(typeof(SkillBox), nameof(SkillBox.ButtonClick))]
        static class SkillBox_ButtonClick_Patch
        {
            static bool Prefix(SkillBox __instance)
            {
                if (!modEnabled.Value)
                    return true;

                bool reduce = Chainloader.PluginInfos.ContainsKey("aedenthorn.Respec") && AedenthornUtils.CheckKeyHeld(Chainloader.PluginInfos["aedenthorn.Respec"].Instance.Config[new ConfigDefinition("Options", "ModKey")].BoxedValue as string);
                SkillInfo skillInfo = SkillAPI.GetSkill(__instance.name);

                // skip to default method if not a custom skill
                if (skillInfo == null)
                    return true;

                // either there's no available points or at max points for the skill
                if (!reduce && Global.code.uiCharacter.curCustomization._ID.skillPoints <= 0 || !reduce && __instance.points >= __instance.maxPoints)
                    return false;

                if (reduce)
                {
                    // check if we can proceed to decrease skill level
                    if (reduce && !skillInfo.OnDecreaseSkillLevel(__instance, skillInfo))
                        return false;
                    SkillAPI.DecreaseSkillLevel(__instance.name, Global.code.uiCharacter.curCustomization.name);
                    // refresh ui
                    Global.code.uiCharacter.Refresh();
                    // return to avoid running default code that increase the skill level
                    return false;
                }

                // check if we can proceed to increase skill level
                if (!reduce && !skillInfo.OnIncreaseSkillLevel(__instance, skillInfo))
                    return false;
                SkillAPI.IncreaseSkillLevel(__instance.name, Global.code.uiCharacter.curCustomization.name);

                return true;
            }
        }

        [HarmonyPatch(typeof(Mainframe), "SaveCharacterCustomization")]
        static class SaveCharacterCustomization_Patch
        {
            static void Prefix(Mainframe __instance, CharacterCustomization customization)
            {
                if (!modEnabled.Value)
                    return;

                foreach (SkillInfo info in customSkills.Values)
                {
                    ES2.Save<int>(GetCharacterSkillLevel(customization.name, info.id), $"{__instance.GetFolderName()}{customization.name}.txt?tag=CustomSkill{info.id}");
                    info.OnSaveCharacterCustomization(info, __instance, customization);
                }
            }
        }

        [HarmonyPatch(typeof(Mainframe), "LoadCharacterCustomization")]
        static class LoadCharacterCustomization_Patch
        {
            static void Postfix(Mainframe __instance, CharacterCustomization gen)
            {
                if (!modEnabled.Value)
                    return;

                foreach (SkillInfo info in customSkills.Values)
                {
                    if (ES2.Exists($"{__instance.GetFolderName()}{gen.name}.txt?tag=CustomSkill{info.id}"))
                    {
                        SetCharacterSkillLevel(gen.name, info.id, ES2.Load<int>($"{__instance.GetFolderName()}{gen.name}.txt?tag=CustomSkill{info.id}"));
                        info.OnLoadCharacterCustomization(info, __instance, gen);
                    }
                }
            }
        }
    }
}
