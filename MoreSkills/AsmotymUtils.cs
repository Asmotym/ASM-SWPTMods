using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using MoreSkills;

public class AsmotymUtils
{

    public static ID GetCurrentID()
    {
        return Global.code.uiCharacter.curCustomization.GetComponent<ID>();
    }

    public static bool IsReduce()
    {
        return Chainloader.PluginInfos.ContainsKey("aedenthorn.Respec") && AedenthornUtils.CheckKeyHeld(Chainloader.PluginInfos["aedenthorn.Respec"].Instance.Config[new ConfigDefinition("Options", "ModKey")].BoxedValue as string);
    }
    
}
