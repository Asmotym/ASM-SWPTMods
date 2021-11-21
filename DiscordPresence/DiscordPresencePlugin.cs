using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;
using BepInEx;
using BepInEx.Configuration;
using System.Reflection;
using Discord;

namespace DiscordPresence
{
    [BepInPlugin("asmotym.DiscordPresence", "Discord Presence", "0.0.1")]
    public class DiscordPresencePlugin : BaseUnityPlugin
    {
        #region Config Entries
        public static ConfigEntry<bool> modEnabled;
        public static ConfigEntry<bool> isDebug;
        public static ConfigEntry<bool> nexusID;
        #endregion

        private static long CLIENT_ID = 896147824008380456;
        public static Discord.Discord discord;
        public static Discord.ActivityManager activityManager;
        protected static Discord.Activity previousActivity;

        public static void Log(string message)
        {
            if (isDebug.Value)
                Debug.Log(typeof(DiscordPresencePlugin).Namespace + " - " + message);
        }

        public static long GetCurrentUnixTimeStamp()
        {
            return new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        }

        public static void UpdatePresence(Activity activity)
        {
            if (modEnabled.Value)
            {
                // save given activity
                previousActivity = activity;
                // update activity
                activityManager.UpdateActivity(activity, results => { });
            }
        }

        public static void UpdateFromPreviousPresence()
        {
            if (modEnabled.Value)
                activityManager.UpdateActivity(previousActivity, result => { });
        }

        private void Awake()
        {
            modEnabled = Config.Bind<bool>("0 - General", "Enabled", true, "Enable this mod");
            isDebug = Config.Bind<bool>("0 - General", "IsDebug", true, "Enable debug logs");

            Log("Plugin Awake");

            // init presence
            discord = new Discord.Discord(CLIENT_ID, (UInt64) Discord.CreateFlags.Default);
            activityManager = discord.GetActivityManager();

            // setup default state
            UpdatePresence(new Activity {
                Details = "In Menu",
                State = "Idle",
                Assets = new ActivityAssets {
                    LargeText = "She Will Punish Them",
                    LargeImage = "default"
                }
            });

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), null);
        }

        public void Start()
        {
            Log("Plugin Start");
        }

        public void Update()
        {
            if (modEnabled.Value)
                discord.RunCallbacks();
        }

        public static Location GetCurrentLocation()
        {
            return Global.code.curlocation;
        }

        [HarmonyPatch(typeof(Global), "Update")]
        static class Global_Update_Patch
        {
            static void Postfix(Global __instance)
            {
                UpdatePresence(new Activity
                { 
                    Details = "Idle",
                    State = __instance.curlocation.levelname,
                    Assets = new ActivityAssets
                    {
                        LargeText = $"Playing as {__instance.uiCharacter.curCustomization.characterName}",
                        LargeImage = "default"
                    }
                });
            }
        }

        [HarmonyPatch(typeof(Global), nameof(Global.ToggleCharacter))]
        static class Global_ToggleCharacter_Patch
        {
            static void Postfix(Global __instance)
            {
                if (__instance.uiCharacter.gameObject.activeSelf)
                    UpdatePresence(new Activity
                    {
                        Details = "Character Menu",
                        State = GetCurrentLocation().levelname,
                        Assets = new ActivityAssets
                        {
                            LargeText = $"Managing {__instance.uiCharacter.curCustomization.characterName}",
                            LargeImage = "default"
                        }
                    });
                else
                    UpdateFromPreviousPresence();
            }
        }

        [HarmonyPatch(typeof(Global), nameof(Global.ToggleWorldMap))]
        static class Global_ToggleLingerie_Patch
        {
            static void Postfix(Global __instance)
            {
                if (__instance.uiCharacter.gameObject.activeSelf)
                    UpdatePresence(new Activity {
                        Details = "World Map",
                        State = GetCurrentLocation().levelname,
                        Assets = new ActivityAssets {
                            LargeText = "World Map",
                            LargeImage = "default"
                        }
                    });
                else
                    UpdateFromPreviousPresence();
            }
        }
    }
}
