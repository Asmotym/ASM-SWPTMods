using System.Reflection;
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
        // skills
        private static TemptressWisdom temptressWisdom;

        // config entries
        public static ConfigEntry<bool> modEnabled;
        public static ConfigEntry<bool> isDebug;
        public static ConfigEntry<int> nexusID;

        public static void Log(string message)
        {
            if (isDebug.Value)
                Debug.Log(typeof(MoreSkillsPlugin).Namespace + " - " + message);
        }

        private void Awake()
        {
            modEnabled = Config.Bind<bool>("General", "Enabled", true, "Enable this mod");
            isDebug = Config.Bind<bool>("General", "IsDebug", true, "Enable debug logs");

            // build skills
            temptressWisdom = new TemptressWisdom(this);

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), null);
            Log("Plugin Awake");
        }

        public void Start()
        {
            Log("Plugin Start");
            temptressWisdom.Update();
        }

    }
}
