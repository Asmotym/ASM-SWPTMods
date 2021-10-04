using HarmonyLib;
using AedenthornSkillFrameworkPlusPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreSkills.Skills
{
    public class ManaOverflowPatches
    {
        public static ManaOverflowSkill skill;

        public ManaOverflowPatches(ManaOverflowSkill temptressWisdom)
        {
            skill = temptressWisdom;
        }

        /*[HarmonyPatch(typeof(CharacterCustomization), nameof(CharacterCustomization.AddSkillPoint))]
        static class CharacterCustomization_AddSkillPoint_Patch
        {
            static void Postfix(CharacterCustomization __instance, string skillname, ref ID ____ID)
            {
                MoreSkillsPlugin.Log($"CharacterCustomization_AddSkillPoint_Patch - skillname: {skillname} - ID: {____ID.name} - Reduce: {AsmotymUtils.IsReduce()}");
            }
        }*/
    }
}
