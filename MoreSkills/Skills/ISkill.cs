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

        bool Decrease(SkillBox skillBox, SkillInfo skillInfo);

        bool Increase(SkillBox skillBox, SkillInfo skillInfo);

    }
}
