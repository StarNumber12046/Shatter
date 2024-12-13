using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;

namespace ExampleMod
{
    [HarmonyPatch(typeof(PlatformSetup), "GetDevIDs")]
    class ExamplePatch
    {
        public static bool Prefix(PlatformSetup __instance) // this is ran before the method
        {
            MonoBehaviour.print("Hello from Prefix!");
            return true; // true to continue execution of the method, false to end it
        }

        public static void Postfix(PlatformSetup __instance) // this is ran after the method
        {
            MonoBehaviour.print("Hello from Postfix!");
        }
    }
}
