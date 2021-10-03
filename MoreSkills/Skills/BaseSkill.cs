using AedenthornSkillFrameworkPlusPlus;
using BepInEx.Configuration;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MoreSkills.Skills
{
    public abstract class BaseSkill : ISkill
    {
        // default properties
        protected MoreSkillsPlugin plugin;
        protected List<string> skillName;
        protected List<string> skillDescription;
        protected string configSection;
        protected Texture2D skillIcon = new Texture2D(1, 1);
        protected string iconName;

        // config default properties
        protected int defaultMaxPoints;
        protected int defaultReqLevel;

        // default config entries
        public static ConfigEntry<int> maxPoints;
        public static ConfigEntry<int> reqLevel;

        protected abstract string skillId
        {
            get;
            set;
        }

        public BaseSkill(MoreSkillsPlugin moreSkillsPlugin, string sectionName, string defaultIconName = "frame", int maxPoints = 5, int reqLevel = 2)
        {
            plugin = moreSkillsPlugin;
            configSection = sectionName;
            defaultMaxPoints = maxPoints;
            defaultReqLevel = reqLevel;
            iconName = defaultIconName;
            MoreSkillsPlugin.Log($"Setup Skill using: {configSection} - {defaultMaxPoints} - {defaultReqLevel} - {iconName}");
            Build();
        }

        public override string ToString()
        {
            return iconName;
        }

        public void Build()
        {
            // bind default skill settings
            maxPoints = plugin.Config.Bind<int>(configSection, "MaxPoints", defaultMaxPoints, "Maximum skill points for this skill");
            reqLevel = plugin.Config.Bind<int>(configSection, "RequiredLevel", defaultReqLevel, "Character level required for this skill");
            
            // bind skill information
            SetConfig();
            SetSkillDescription();
            SetSkillName();
            SetSettingChanged();
            SetSkillIcon();

            // add skill to the list
            AddSkill();
        }

        public virtual void Update()
        {
            SetSkillDescription();

            // initialize increase/decrease delegate
            SkillInfo skillInfo = SkillAPI.GetSkill(skillId);

            if (skillInfo != null)
            {
                skillInfo.decreaseDelegate = Decrease;
                skillInfo.increaseDelegate = Increase;
            }
        }

        public virtual void AddSkill()
        {
            // initialize skill using SkillFramework API
            SkillAPI.AddSkill(skillId, skillName, skillDescription, 1, skillIcon, maxPoints.Value, reqLevel.Value, false);
        }

        public virtual void SetSkillIcon()
        {
            string path = Path.Combine(AedenthornUtils.GetAssetPath(plugin), $"{iconName}.png");
            MoreSkillsPlugin.Log($"Setup skill icon {path}");
            if (File.Exists(path))
                skillIcon.LoadImage(File.ReadAllBytes(path));
        }

        public abstract void SetConfig();
        public abstract void SetSkillDescription();
        public abstract void SetSkillName();
        public abstract void SetSettingChanged();
        public abstract bool Decrease(SkillBox skillBox, SkillInfo skillInfo);
        public abstract bool Increase(SkillBox skillBox, SkillInfo skillInfo);
        
    }
}
