using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoreSkills;

public class AsmotymUtils
{

    public static bool CanHandleSkillIncreaseDecrease(SkillBox skillBox, string skillId)
    {
        if (!MoreSkillsPlugin.modEnabled.Value)
            return false;

        // procceed to next call if not in the current skill
        if (skillId != skillBox.name)
            return false;

        return true;
    }

    public static ID GetCurrentID()
    {
        return Global.code.uiCharacter.curCustomization.GetComponent<ID>();
    }
    
}
