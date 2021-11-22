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
        protected static Location previousLocation;
        protected static Discord.Activity previousActivity;
        protected static long startTime;

        public static void Log(string message)
        {
            if (isDebug.Value)
                Debug.Log(typeof(DiscordPresencePlugin).Namespace + " - " + message);
        }

        public static long GetCurrentUnixTimeStamp()
        {
            return new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        }

        public static void UpdatePresence(Activity? activity = null)
        {
            if (modEnabled.Value)
            {
                Log("Update Presence...");
                if (!activity.HasValue)
                {
                    activity = new Activity
                    {
                        State = "Playing Solo", // default state, can be improved later
                        Details = GetCurrentLocation().levelname,
                        Assets = new ActivityAssets
                        {
                            LargeImage = "default",
                            LargeText = GetCurrentCharacter().characterName
                        },
                        Timestamps = new ActivityTimestamps
                        {
                            Start = startTime
                        }
                    };
                }
                
                // update activity
                activityManager.UpdateActivity((Activity) activity, results => { });
            }
        }

        private void Awake()
        {
            modEnabled = Config.Bind<bool>("0 - General", "Enabled", true, "Enable this mod");
            isDebug = Config.Bind<bool>("0 - General", "IsDebug", true, "Enable debug logs");

            Log("Plugin Awake");

            // init presence
            discord = new Discord.Discord(CLIENT_ID, (UInt64) Discord.CreateFlags.Default);
            activityManager = discord.GetActivityManager();

            startTime = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

            // setup default state
            UpdatePresence(new Activity {
                Details = "In Menu",
                Assets = new ActivityAssets {
                    LargeText = "She Will Punish Them",
                    LargeImage = "default"
                },
                Timestamps = new ActivityTimestamps
                {
                    Start = startTime
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

        public static CharacterCustomization GetCurrentCharacter()
        {
            return Player.code.customization;
        }

        [HarmonyPatch(typeof(Global), "Update")]
        static class Global_ToggleLingerie_Patch
        {
            static void Postfix(Global __instance)
            {
                Location location = __instance.curlocation;

                // first setup if location hasn't be defined
                if (!previousLocation)
                {
                    // setup presence and save previous location
                    previousLocation = location;

                    UpdatePresence();

                    return;
                }

                // skip if location hasn't changed
                if (previousLocation.levelname == location.levelname)
                    return;

                // setup if location has changed
                if (previousLocation.levelname != location.levelname)
                {
                    previousLocation = location;
                    UpdatePresence();
                    return;
                }
            }
        }
    }
}
