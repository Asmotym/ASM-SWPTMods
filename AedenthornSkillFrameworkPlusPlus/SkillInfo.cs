using System.Collections.Generic;
using UnityEngine;

namespace AedenthornSkillFrameworkPlusPlus
{
    public delegate bool SkillValidateIncreaseDelegate(SkillBox skillBox, SkillInfo skillInfo);
    public delegate bool SkillValidateDecreaseDelegate(SkillBox skillBox, SkillInfo skillInfo);

    public delegate void SaveCharacterCustomizationDelegate(SkillInfo skillInfo, Mainframe mainFrame, CharacterCustomization characterCustomization);
    public delegate void LoadCharacterCustomizationDelegate(SkillInfo skillInfo, Mainframe mainFrame, CharacterCustomization characterCustomization);

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
        public SkillValidateIncreaseDelegate SetOnIncreaseSkillLevel
        {
            get { return _increaseDelegate; }
            set { _increaseDelegate = value; }
        }

        public bool OnIncreaseSkillLevel(SkillBox skillBox, SkillInfo skillInfo)
        {
            if (_increaseDelegate != null)
                return _increaseDelegate(skillBox, skillInfo);

            return true;
        }

        private SkillValidateDecreaseDelegate _decreaseDelegate;
        public SkillValidateDecreaseDelegate SetOnDecreaseSkillLevel
        {
            get { return _decreaseDelegate; }
            set { _decreaseDelegate = value; }
        }

        public bool OnDecreaseSkillLevel(SkillBox skillBox, SkillInfo skillInfo)
        {
            if (_decreaseDelegate != null)
                return _decreaseDelegate(skillBox, skillInfo);

            return true;
        }

        private SaveCharacterCustomizationDelegate _saveCharacterCustomizationDelegate;
        public SaveCharacterCustomizationDelegate SetSaveCharacterCustomization
        {
            get { return _saveCharacterCustomizationDelegate; }
            set { _saveCharacterCustomizationDelegate = value; }
        }

        public void OnSaveCharacterCustomization(SkillInfo skillInfo, Mainframe mainFrame, CharacterCustomization characterCustomization)
        {
            if (_saveCharacterCustomizationDelegate != null)
                _saveCharacterCustomizationDelegate(skillInfo, mainFrame, characterCustomization);
        }

        private LoadCharacterCustomizationDelegate _loadCharacterCustomizationDelegate;
        public LoadCharacterCustomizationDelegate SetLoadCharacterCustomization
        {
            get { return _loadCharacterCustomizationDelegate; }
            set { _loadCharacterCustomizationDelegate = value; }
        }

        public void OnLoadCharacterCustomization(SkillInfo skillInfo, Mainframe mainFrame, CharacterCustomization characterCustomization)
        {
            if (_loadCharacterCustomizationDelegate != null)
                _loadCharacterCustomizationDelegate(skillInfo, mainFrame, characterCustomization);
        }
    }
}