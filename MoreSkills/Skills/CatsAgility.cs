using BepInEx.Configuration;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using AedenthornSkillFrameworkPlusPlus;
using System;

namespace MoreSkills.Skills
{
    public partial class CatsAgility : BaseSkill
    {
        // cat's agility config entries
        public ConfigEntry<int> increase;

        protected override string skillId
        {
            get { return typeof(MoreSkillsPlugin).Namespace + "_" + typeof(CatsAgility).Name; }
            set { skillId = value; }
        }

        public CatsAgility(
            MoreSkillsPlugin moreSkillsPlugin,
            string sectionName,
            string defaultIconName = "frame",
            int maxPoints = 10,
            int reqLevel = 2): base(moreSkillsPlugin, sectionName, typeof(CatsAgility).Name, maxPoints, reqLevel)
        { }

        public override void SetConfig()
        {
            // bind skill settings
            increase = plugin.Config.Bind<int>(configSection, "AgilityIncrease", 2, "Agility stat increase per skill level");
        }

        public override void SetSkillDescription()
        {
            skillDescription = new List<string>()
            {
                string.Format("Increase Agility by {0} per level", increase.Value), // en
                string.Format("每级将敏捷提高 {0}", increase.Value), // chinese
                string.Format("Повышайте ловкость на {0} за уровень", increase.Value), // russian
                string.Format("レベルごとに敏捷性を{0}ずつ増やします", increase.Value), // japanese
            };
        }

        public override void SetSkillName()
        {
            skillName = new List<string>()
            {
                "Cat's Agility", // en
                "猫的敏捷", // chinese
                "Кошачья ловкость", // russian
                "猫の敏捷性", // japanese
            };
        }

        public override void SetSettingChanged()
        {
            increase.SettingChanged += Increase_SettingChanged;
        }

        private void Increase_SettingChanged(object sender, EventArgs e)
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
            id.agility -= increase.Value;

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
            id.agility += increase.Value;

            return true;
        }

    }
}
