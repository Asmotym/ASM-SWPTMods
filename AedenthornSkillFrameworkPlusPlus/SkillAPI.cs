using System.Collections.Generic;
using UnityEngine;

namespace AedenthornSkillFrameworkPlusPlus
{
    public static class SkillAPI
    {
        public static void AddSkill(
            string id,
            List<string> name,
            List<string> description,
            int category,
            Texture2D icon,
            int maxPoints,
            int reqLevel,
            bool isActiveSkill
            )
        {
            // check if skill already exist
            if (AedenthornSkillFrameworkPlusPlus.customSkills.ContainsKey(id)) {
                AedenthornSkillFrameworkPlusPlus.Log($"Updating skill {id}");
            } else {
                AedenthornSkillFrameworkPlusPlus.Log($"Adding skill {id}");
            }

            // create and add the skill
            AedenthornSkillFrameworkPlusPlus.customSkills[id] = new SkillInfo()
            {
                id = id,
                name = name,
                description = description,
                category = category,
                icon = icon,
                maxPoints = maxPoints,
                reqLevel = reqLevel,
                isActiveSkill = isActiveSkill
            };

            // refresh current ui character to draw the new skill
            if (Global.code?.uiCharacter?.gameObject.activeSelf == true)
                Global.code.uiCharacter.Refresh();
        }

        public static SkillInfo GetSkill(string id)
        {
            return AedenthornSkillFrameworkPlusPlus.customSkills.ContainsKey(id) ? AedenthornSkillFrameworkPlusPlus.customSkills[id] : null;
        }

        public static int GetCharacterSkillLevel(string id, string name)
        {
            if (SkillNotExist(id, name)) {
                return 0;
            }
            return AedenthornSkillFrameworkPlusPlus.characterSkillLevels[name][id];
        }

        public static void DecreaseSkillLevel(string id, string name)
        {
            // check if skill don't exist
            if (SkillNotExist(id, name))
                return;
            // check if skill is not level 0
            if (AedenthornSkillFrameworkPlusPlus.characterSkillLevels[name][id] == 0)
                return;
            // decrease skill level
            AedenthornSkillFrameworkPlusPlus.characterSkillLevels[name][id]--;
        }

        public static void IncreaseSkillLevel(string id, string name)
        {
            // check if skill don't exist
            if (SkillNotExist(id, name))
                return;
            // increase skill level
            AedenthornSkillFrameworkPlusPlus.characterSkillLevels[name][id]++;
        }

        public static bool SkillNotExist(string id, string name)
        {
            return !AedenthornSkillFrameworkPlusPlus.characterSkillLevels.ContainsKey(name) || !AedenthornSkillFrameworkPlusPlus.characterSkillLevels[name].ContainsKey(id);
        }
    }
}