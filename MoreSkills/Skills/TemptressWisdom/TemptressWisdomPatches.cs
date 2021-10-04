using HarmonyLib;
using AedenthornSkillFrameworkPlusPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreSkills.Skills
{
    public class TemptressWisdomPatches
    {
        public static TemptressWisdomSkill skill;

        public TemptressWisdomPatches(TemptressWisdomSkill temptressWisdom)
        {
            skill = temptressWisdom;
        }

    }
}
