using System.Reflection;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using BepInEx;
using BepInEx.Configuration;
using MoreSkills.Skills;
using SkillFramework;
using SkillFramework.BaseSkill;

namespace MoreSkills
{
    [BepInDependency("aedenthorn.SkillFramework", "0.2.1")]
    [BepInPlugin("asmotym.MoreSkills", "More Skills", "0.0.1")]
    public partial class MoreSkillsPlugin : BaseUnityPlugin
    {
        // config entries
        public static ConfigEntry<bool> modEnabled;
        public static ConfigEntry<bool> isDebug;
        public static ConfigEntry<int> nexusID;

        public static List<ISkill> skillList;

        public static void Log(string message)
        {
            if (isDebug.Value)
                Debug.Log(typeof(MoreSkillsPlugin).Namespace + " - " + message);
        }

        private void Awake()
        {
            modEnabled = Config.Bind<bool>("0 - General", "Enabled", true, "Enable this mod");
            isDebug = Config.Bind<bool>("0 - General", "IsDebug", true, "Enable debug logs");

            Log("Plugin Awake");

            // build skills
            skillList = new List<ISkill>()
            {
                new TemptressWisdom()
                {
                    iconName = typeof(TemptressWisdom).Name
                }.Build(this, "1 - Temptress Wisdom"),
                new BearSpirit()
                {
                    iconName = typeof(BearSpirit).Name
                }.Build(this, "2 - Bear Spirit"),
                new CatsAgility()
                {
                    iconName = typeof(CatsAgility).Name
                }.Build(this, "3 - Cats Agility"),
                new ManaOverflow()
                { 
                    iconName = typeof(ManaOverflow).Name
                }.Build(this, "4 - Mana Overflow"),
                new MightyPower()
                { 
                    iconName = typeof(MightyPower).Name
                }.Build(this, "5 - Mighty Power"),
            };

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), null);
        }

        public void Start()
        {
            Log("Plugin Start");
            foreach (BaseSkill skill in skillList)
            {
                Log($"Update skill {skill}...");
                skill.Update();
            }
        }

    }
}
