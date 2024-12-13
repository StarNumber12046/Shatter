using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shatter.Patches.MarbleControllerPatch
{
    [HarmonyPatch(typeof(MarbleController), "IsReadyToWin")]
    internal class MarbleControllerIsReadyToWinPatch
    {
        static bool Prefix(ref bool __result)
        {
            if (!Config.forceReadyToWin) return true;
            __result = true;
            return false;
        }
    }
}
