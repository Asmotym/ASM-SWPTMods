using BepInEx.Configuration;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using AedenthornSkillFrameworkPlusPlus;
using System;

namespace MoreSkills.Skills
{
    public partial class BearSpirit : BaseSkill
    {
        // bear spirit config entries
        public ConfigEntry<int> increaseVitality;
        public ConfigEntry<int> increaseStrength;

        protected override string skillId
        {
            get { return typeof(MoreSkillsPlugin).Namespace + "_" + typeof(BearSpirit).Name; }
            set { skillId = value; }
        }

        public BearSpirit(
            MoreSkillsPlugin moreSkillsPlugin,
            string sectionName,
            string defaultIconName = "frame",
            int maxPoints = 10,
            int reqLevel = 2) : base(moreSkillsPlugin, sectionName, typeof(BearSpirit).Name, maxPoints, reqLevel)
        { }

        public override void SetConfig()
        {
            // bind skill settings
            increaseVitality = plugin.Config.Bind<int>(configSection, "VitalityIncrease", 2, "Vitality stat increase per skill level");
            increaseStrength = plugin.Config.Bind<int>(configSection, "StrengthIncrease", 1, "Strength stat increase per skill level");
        }

        public override void SetSkillDescription()
        {
            skillDescription = new List<string>()
            {
                string.Format("Increase Strength by {0} and Vitality by {1} per level", increaseStrength.Value, increaseVitality.Value), // en
                string.Format("每级增加 {0} 的力量和 {1} 的活力", increaseStrength.Value, increaseVitality.Value), // chinese
                string.Format("Увеличение силы на {0} и живучести на {1} за уровень", increaseStrength.Value, increaseVitality.Value), // russian
                string.Format("レベルごとに強度を{0}増加させ、活力を{1}増加させます", increaseStrength.Value, increaseVitality.Value), // japanese
            };
        }

        public override void SetSkillName()
        {
            skillName = new List<string>()
            {
                "Bear Spirit", // en
                "熊灵", // chinese
                "Медведь Дух", // russian
                "ベアスピリット", // japanese
            };
        }

        public override void SetSettingChanged()
        {
            increaseVitality.SettingChanged += IncreaseVitality_SettingChanged;
            increaseStrength.SettingChanged += IncreaseStrength_SettingChanged;
        }

        public void IncreaseVitality_SettingChanged(object sender, EventArgs e)
        {
            Update();
        }

        public void IncreaseStrength_SettingChanged(object sender, EventArgs e)
        {
            Update();
        }

        public override bool Decrease(SkillBox skillBox, SkillInfo skillInfo)
        {
            // cannot handle skill decrease
            if (!AsmotymUtils.CanHandleSkillIncreaseDecrease(skillBox, skillId))
                return true;

            MoreSkillsPlugin.Log($"Decrease called for {skillBox.name}...");

            ID id = AsmotymUtils.GetCurrentID();

            // decrease vitality and strength values
            id.strength -= increaseStrength.Value;
            id.vitality -= increaseVitality.Value;

            return true;
        }

        public override bool Increase(SkillBox skillBox, SkillInfo skillInfo)
        {
            // cannot handle skill increase
            if (!AsmotymUtils.CanHandleSkillIncreaseDecrease(skillBox, skillId))
                return true;

            MoreSkillsPlugin.Log($"Increase called for {skillBox.name}...");

            ID id = AsmotymUtils.GetCurrentID();

            // increase vitality and strength values
            id.strength += increaseStrength.Value;
            id.vitality += increaseVitality.Value;

            return true;
        }

    }
}
