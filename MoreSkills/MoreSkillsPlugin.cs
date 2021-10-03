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

        public static List<BaseSkill> skillList;

        public static void Log(string message)
        {
            if (isDebug.Value)
                Debug.Log(typeof(MoreSkillsPlugin).Namespace + " - " + message);
        }

        private void Awake()
        {
            modEnabled = Config.Bind<bool>("0 - General", "Enabled", true, "Enable this mod");
            isDebug = Config.Bind<bool>("0 - General", "IsDebug", true, "Enable debug logs");

            // build skills
            skillList = new List<BaseSkill>()
            {
                new TemptressWisdom(this, "1 - Temptress Wisdom"),
                new BearSpirit(this, "2 - Bear Spirit"),
                new CatsAgility(this, "3 - Cats Agility"),
            };

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), null);
            Log("Plugin Awake");
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
