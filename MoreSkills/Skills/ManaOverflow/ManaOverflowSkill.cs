using BepInEx.Configuration;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using AedenthornSkillFrameworkPlusPlus;
using System;

namespace MoreSkills.Skills
{
    public partial class ManaOverflowSkill : AsmotymSkill
    {
        // mana overflow config entries
        public ConfigEntry<int> increase;

        protected override string skillId
        {
            get { return typeof(MoreSkillsPlugin).Namespace + "_" + typeof(ManaOverflowSkill).Name; }
            set { skillId = value; }
        }

        public ManaOverflowSkill(
            MoreSkillsPlugin moreSkillsPlugin,
            string sectionName,
            int category = 2,
            string defaultIconName = "frame",
            int maxPoints = 10,
            int reqLevel = 2) : base(moreSkillsPlugin, sectionName, category, typeof(ManaOverflowSkill).Name, maxPoints, reqLevel)
        {
            new ManaOverflowPatches(this);
        }

        public override void SetConfig()
        {
            // bind skill settings
            increase = plugin.Config.Bind<int>(configSection, "Increase", 20, "Base Max Mana value increase per skill level");
        }

        public override void SetSkillDescription()
        {
            skillDescription = new List<string>()
            {
                string.Format("Increase Maximum Mana by {0} per level", increase.Value), // en
                string.Format("每级增加 {0} 的最大法力值", increase.Value), // chinese
                string.Format("Увеличивайте максимум маны на {0} за уровень", increase.Value), // russian
                string.Format("レベルごとに最大マナを{0}増やします", increase.Value), // japanese
            };
        }

        public override void SetSkillName()
        {
            skillName = new List<string>()
            {
                "Mana Overflow", // en
                "法力溢出", // chinese
                "Переполнение маны", // russian
                "マナオーバーフロー", // japanese
            };
        }

        public override void SetSettingChanged()
        {
            increase.SettingChanged += Increase_SettingChanged;
        }

        public void Increase_SettingChanged(object sender, EventArgs e)
        {
            Update();
        }

        public override bool OnDecreaseSkillLevel(SkillBox skillBox, SkillInfo skillInfo)
        {
            // cannot handle skill decrease
            if (!AsmotymUtils.CanHandleSkillIncreaseDecrease(skillBox, skillId))
                return true;

            MoreSkillsPlugin.Log($"Decrease called for {skillBox.name}...");

            /*ID id = AsmotymUtils.GetCurrentID();

            // decrease max mana value
            id.maxMana -= increase.Value;

            Global.code.uiCharacter.curCustomization._ID = id;*/

            return true;
        }

        public override bool OnIncreaseSkillLevel(SkillBox skillBox, SkillInfo skillInfo)
        {
            // cannot handle skill increase
            if (!AsmotymUtils.CanHandleSkillIncreaseDecrease(skillBox, skillId))
                return true;

            MoreSkillsPlugin.Log($"Increase called for {skillBox.name}...");

            /*ID id = AsmotymUtils.GetCurrentID();

            // increase max mana value
            id.maxMana += increase.Value;

            Global.code.uiCharacter.curCustomization._ID = id;*/

            return true;
        }
    }
}
