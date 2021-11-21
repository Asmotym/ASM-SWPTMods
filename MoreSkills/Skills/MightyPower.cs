using BepInEx.Configuration;
using System.Collections.Generic;
using System;
using SkillFramework;
using SkillFramework.BaseSkill;

namespace MoreSkills.Skills
{
    public partial class MightyPower : BaseSkill
    {
        // temptress wisdom config entries
        public ConfigEntry<int> increasePercent;

        protected override string skillId
        {
            get { return typeof(MoreSkillsPlugin).Namespace + "_" + typeof(MightyPower).Name; }
            set { skillId = value; }
        }

        public override void SetConfig()
        {
            // bind skill settings
            increasePercent = plugin.Config.Bind<int>(configSection, "Increase", 2, "Base Magic Damage % value increase per skill level");
        }

        public override void SetSkillDescription()
        {
            skillDescription = new List<string>()
            {
                string.Format("Increase Magic Damage by {0}% per level", increasePercent.Value), // en
                string.Format("每级增加 {0}% 的魔法伤害", increasePercent.Value), // chinese
                string.Format("Увеличивайте магический урон на {0}% за уровень", increasePercent.Value), // russian
                string.Format("レベルごとに魔法ダメージを{0}％増加させる", increasePercent.Value), // japanese
            };
        }

        public override void SetSkillName()
        {
            skillName = new List<string>()
            {
                "Mighty Power", // en
                "强大的力量", // chinese
                "Могущественная сила", // russian
                "マイティパワー", // japanese
            };
        }

        public override void SetSettingChanged()
        {
            increasePercent.SettingChanged += Increase_SettingChanged;
        }

        public void Increase_SettingChanged(object sender, EventArgs e)
        {
            Update();
        }

        public override void PostfixCharacterCustomizationUpdateStats(CharacterCustomization characterCustomization, SkillInfo skillInfo)
        {
            ID id = characterCustomization.GetComponent<ID>();

            int skillLevel = SkillAPI.GetCharacterSkillLevel(skillInfo.id, characterCustomization.name);
            int skillIncreasePercentTotal = skillLevel == 0 
                ? increasePercent.Value
                : increasePercent.Value * skillLevel;

            if (skillLevel == 0)
            {
                // decreasing skill level
                id.fireballDamage = (int) (id.fireballDamage * (1 - ((double)skillIncreasePercentTotal / 100)));
                id.frostibiteDamage = (int) (id.frostibiteDamage * (1 - ((double)skillIncreasePercentTotal / 100)));
                return;
            }

            // increasing skill level
            id.fireballDamage = (int) (id.fireballDamage * (1 + ((double) skillIncreasePercentTotal / 100)));
            id.frostibiteDamage = (int) (id.frostibiteDamage * (1 + ((double) skillIncreasePercentTotal / 100)));
        }
    }
}
