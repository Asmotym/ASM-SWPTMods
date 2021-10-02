using BepInEx.Configuration;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using AedenthornSkillFrameworkPlusPlus;
using System;

namespace MoreSkills.Skills
{
    public partial class TemptressWisdom
    {
        private static MoreSkillsPlugin plugin;
        private static string configSection = "Temptress Wisdom";

        public static ConfigEntry<int> increase;
        public static ConfigEntry<int> maxPoints;
        public static ConfigEntry<int> reqLevel;

        private static string skillId = typeof(MoreSkillsPlugin).Namespace + "_" + typeof(TemptressWisdom).Name;
        private static List<string> skillName = new List<string>()
        {
            "Temptress Wisdom", // en
            "魔女智慧", // chinese
            "Мудрость соблазнительницы", // russian
            "誘惑の知恵", // japanese
        };

        private static List<string> skillDescription;
        private static Texture2D skillIcon = new Texture2D(1, 1);

        public TemptressWisdom(MoreSkillsPlugin moreSkillsPlugin)
        {
            plugin = moreSkillsPlugin;
            Build();
        }

        private void Build()
        {
            SetConfig();
            SetSkillDescription();

            increase.SettingChanged += Increase_SettingChanged;

            SetSkillIcon();

            AddSkill();
        }

        /// <summary>
        /// Call this method to update skill infos
        /// </summary>
        public void Update()
        {
            SetSkillDescription();

            // initialize decrease delegate
            SkillInfo skillInfo = SkillAPI.GetSkill(skillId);

            if (skillInfo != null)
            {
                skillInfo.decreaseDelegate = Decrease;
                skillInfo.increaseDelegate = Increase;
            }
        }

        private void AddSkill()
        {
            // initialize skill using SkillFramework API
            SkillAPI.AddSkill(skillId, skillName, skillDescription, 1, skillIcon, maxPoints.Value, reqLevel.Value, false);
        }

        /// <summary>
        /// Setup plugin config default values
        /// </summary>
        private void SetConfig()
        {
            // bind skill settings
            increase = plugin.Config.Bind<int>(configSection, "Increase", 2, "Base Power value increase per skill level");
            maxPoints = plugin.Config.Bind<int>(configSection, "MaxPoints", 10, "Maximum skill points for this skill");
            reqLevel = plugin.Config.Bind<int>(configSection, "RequiredLevel", 12, "Character level required for this skill");
        }

        /// <summary>
        /// Setup skill description translations
        /// </summary>
        private void SetSkillDescription()
        {
            skillDescription = new List<string>()
            {
                string.Format("Increase Power by {0} per level", increase.Value), // en
                string.Format("每级增加 {0} 的力量", increase.Value), // chinese
                string.Format("Увеличивайте мощность на {0} за уровень", increase.Value), // russian
                string.Format("レベルごとに力を {0} 増やします", increase.Value), // japanese
            };
        }

        private void SetSkillIcon()
        {
            if (File.Exists(Path.Combine(AedenthornUtils.GetAssetPath(plugin), "TemptressWisdom.png")))
                skillIcon.LoadImage(File.ReadAllBytes(Path.Combine(AedenthornUtils.GetAssetPath(plugin), "TemptressWisdom.png")));
        }

        public static bool Decrease(SkillBox skillBox, SkillInfo skillInfo)
        {
            if (!MoreSkillsPlugin.modEnabled.Value)
                return true;

            // get character ID instance
            ID id = Global.code.uiCharacter.curCustomization.GetComponent<ID>();

            // procceed to next call if not in the current skill
            if (skillId != skillBox.name)
                return true;

            MoreSkillsPlugin.Log($"Decrease called for {skillBox.name}...");

            // decrease power value
            id.power -= increase.Value;

            // refresh ui character
            if (Global.code.uiCharacter.gameObject.activeSelf)
                Global.code.uiCharacter.Refresh();

            return true;
        }

        public static bool Increase(SkillBox skillBox, SkillInfo skillInfo)
        {
            if (!MoreSkillsPlugin.modEnabled.Value)
                return true;

            // get character ID instance
            ID id = Global.code.uiCharacter.curCustomization.GetComponent<ID>();

            // procceed to next call if not in the current skill
            if (skillId != skillBox.name)
                return true;

            MoreSkillsPlugin.Log($"Increase called for {skillBox.name}...");

            // increase power value
            id.power += increase.Value;

            // refresh ui character
            if (Global.code.uiCharacter.gameObject.activeSelf)
                Global.code.uiCharacter.Refresh();

            return true;
        }

        private void Increase_SettingChanged(object sender, EventArgs e)
        {
            Update();
        }

    }
}
