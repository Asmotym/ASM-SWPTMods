using BepInEx.Configuration;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using AedenthornSkillFrameworkPlusPlus;
using System;

namespace MoreSkills.Skills
{
    public interface ISkill
    {

        void Build();

        void SetConfig();

        void SetSettingChanged();

        void SetSkillDescription();

        void SetSkillIcon();

        void SetSkillName();

        void AddSkill();

        void Update();

        bool OnDecreaseSkillLevel(SkillBox skillBox, SkillInfo skillInfo);

        bool OnIncreaseSkillLevel(SkillBox skillBox, SkillInfo skillInfo);

    }
}
