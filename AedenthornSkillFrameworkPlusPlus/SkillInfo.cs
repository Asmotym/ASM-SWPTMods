using System.Collections.Generic;
using UnityEngine;

namespace AedenthornSkillFrameworkPlusPlus
{
    public delegate bool SkillValidateIncreaseDelegate(SkillBox skillBox, SkillInfo skillInfo);
    public delegate bool SkillValidateDecreaseDelegate(SkillBox skillBox, SkillInfo skillInfo);

    public class SkillInfo
    {
        public string id;
        public List<string> name;
        public List<string> description;
        public int category;
        public Texture2D icon;
        public int maxPoints;
        public int reqLevel;
        public bool isActiveSkill;

        private SkillValidateIncreaseDelegate _increaseDelegate;
        public SkillValidateIncreaseDelegate increaseDelegate
        {
            get { return _increaseDelegate; }
            set { _increaseDelegate = value; }
        }

        private SkillValidateDecreaseDelegate _decreaseDelegate;
        public SkillValidateDecreaseDelegate decreaseDelegate
        {
            get { return _decreaseDelegate; }
            set { _decreaseDelegate = value; }
        }

        public bool isValidToIncrease(SkillBox skillBox, SkillInfo skillInfo)
        {
            if (increaseDelegate != null)
                return increaseDelegate(skillBox, skillInfo);

            return true;
        }

        public bool isValidToDecrease(SkillBox skillBox, SkillInfo skillInfo)
        {
            if (decreaseDelegate != null)
                return decreaseDelegate(skillBox, skillInfo);

            return true;
        }
    }
}