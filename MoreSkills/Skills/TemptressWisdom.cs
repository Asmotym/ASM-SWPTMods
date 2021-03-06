using BepInEx.Configuration;
using System.Collections.Generic;
using SkillFramework.BaseSkill;
using SkillFramework;
using System;

namespace MoreSkills.Skills
{
    public partial class TemptressWisdom : BaseSkill
    {
        // temptress wisdom config entries
        public ConfigEntry<int> increase;

        protected override string skillId
        {
            get { return typeof(MoreSkillsPlugin).Namespace + "_" + typeof(TemptressWisdom).Name; }
            set { skillId = value; }
        }

        public override void SetConfig()
        {
            // bind skill settings
            increase = plugin.Config.Bind<int>(configSection, "Increase", 2, "Base Power value increase per skill level");
        }

        public override void SetSkillDescription()
        {
            skillDescription = new List<string>()
            {
                string.Format("Increase Power by {0} per level", increase.Value), // en
                string.Format("每级增加 {0} 的力量", increase.Value), // chinese
                string.Format("Увеличивайте мощность на {0} за уровень", increase.Value), // russian
                string.Format("レベルごとに力を {0} 増やします", increase.Value), // japanese
            };
        }

        public override void SetSkillName()
        {
            skillName = new List<string>()
            {
                "Temptress Wisdom", // en
                "魔女智慧", // chinese
                "Мудрость соблазнительницы", // russian
                "誘惑の知恵", // japanese
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
            if (!CanHandleSkillIncreaseDecrease(skillBox, skillId))
                return false;

            MoreSkillsPlugin.Log($"Decrease called for {skillBox.name}...");

            ID id = AsmotymUtils.GetCurrentID();

            // decrease power value
            id.power -= increase.Value;

            // refresh ui character
            if (Global.code.uiCharacter.gameObject.activeSelf)
                Global.code.uiCharacter.Refresh();

            return true;
        }

        public override bool OnIncreaseSkillLevel(SkillBox skillBox, SkillInfo skillInfo)
        {
            // cannot handle skill increase
            if (!CanHandleSkillIncreaseDecrease(skillBox, skillId))
                return false;

            MoreSkillsPlugin.Log($"Increase called for {skillBox.name}...");

            ID id = AsmotymUtils.GetCurrentID();

            // increase power value
            id.power += increase.Value;

            // update stats & run sfx & refresh ui
            Global.code.uiCharacter.curCustomization.UpdateStats();
            RM.code.PlayOneShot(RM.code.sndAddAttribute);
            Global.code.uiCharacter.Refresh();

            return true;
        }
    }
}
