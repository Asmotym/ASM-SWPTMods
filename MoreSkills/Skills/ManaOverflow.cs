using BepInEx.Configuration;
using System.Collections.Generic;
using AedenthornSkillFrameworkPlusPlus.BaseSkill;
using AedenthornSkillFrameworkPlusPlus;
using System;

namespace MoreSkills.Skills
{
    public partial class ManaOverflow : BaseSkill
    {
        // mana overflow config entries
        public ConfigEntry<int> increase;

        protected override string skillId
        {
            get { return typeof(MoreSkillsPlugin).Namespace + "_" + typeof(ManaOverflow).Name; }
            set { skillId = value; }
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

        public override void PostfixCharacterCustomizationUpdateStats(CharacterCustomization characterCustomization, SkillInfo skillInfo)
        {
            ID id = characterCustomization.GetComponent<ID>();
            id.maxMana += (float) increase.Value * SkillAPI.GetCharacterSkillLevel(skillInfo.id, characterCustomization.name);
        }
    }
}
