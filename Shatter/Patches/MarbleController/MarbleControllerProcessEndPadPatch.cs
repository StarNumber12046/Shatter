using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Shatter;

namespace Shater.Patches.MarbleControllerPatch
{
    [HarmonyPatch(typeof(MarbleController), "ProcessEndPad")]
    internal class MarbleControllerProcessEndPadPatch
    {
        static bool Prefix(MarbleController __instance, float timeToMove, ref bool __result)
        {
            if (!Config.forceEndPad) return true; // Don't skip
            if (!NetworkManager.IsSingleplayer)
            {
                return true;
            }
            if (__instance.InGhostMode)
            {
                return true;
            }
            if (__instance.GetVelocity().magnitude < 0.0001f)
            {
                return true;
            }
            GameObject endPad = __instance.EndPad;
            if (endPad == null)
            {
                return true;
            }
            float num = 1f;
            if (!endPad.GetComponent<EndPadController>().TheCollider.CastRayInWorldSpace(__instance.GetPosition(), __instance.GetVelocity(), timeToMove, __instance.Radius, out num))
            {
                return true;
            }
            var accountTimeMethod = typeof(MarbleController).GetMethod("AccountTime", BindingFlags.NonPublic | BindingFlags.Instance);
            var onEndPadContactMethod = typeof(MarbleController).GetMethod("OnEndPadContact", BindingFlags.NonPublic | BindingFlags.Instance);

            accountTimeMethod.Invoke(__instance, new object[] { num * timeToMove });
            onEndPadContactMethod.Invoke(__instance, new object[] { __instance.EndPad.gameObject });

            __result = true;

            return false; // false skips the rest of the method
        }

    }
}
