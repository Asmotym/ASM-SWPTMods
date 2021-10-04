using System.Reflection;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using BepInEx;
using BepInEx.Configuration;
using MoreSkills.Skills;

namespace MoreSkills
{
    [BepInDependency("asmotym.AedenthornSkillFrameworkPlusPlus", "0.0.1")]
    [BepInPlugin("asmotym.MoreSkills", "More Skills", "0.0.1")]
    public partial class MoreSkillsPlugin : BaseUnityPlugin
    {
        // config entries
        public static ConfigEntry<bool> modEnabled;
        public static ConfigEntry<bool> isDebug;
        public static ConfigEntry<int> nexusID;

        public static List<AsmotymSkill> skillList;

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
            skillList = new List<AsmotymSkill>()
            {
                new TemptressWisdomSkill(this, "1 - Temptress Wisdom"),
                new BearSpiritSkill(this, "2 - Bear Spirit"),
                new CatsAgilitySkill(this, "3 - Cats Agility"),
                new ManaOverflowSkill(this, "4 - Mana Overflow"),
            };

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), null);
        }

        public void Start()
        {
            Log("Plugin Start");
            foreach (AsmotymSkill skill in skillList)
            {
                Log($"Update skill {skill}...");
                skill.Update();
            }
        }

    }
}
