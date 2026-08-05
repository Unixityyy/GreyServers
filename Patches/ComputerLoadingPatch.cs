using System;
using GorillaNetworking;
using HarmonyLib;

namespace GreyServers.HarmonyPatches
{
    [HarmonyPatch(typeof(GorillaComputer), "SwitchToLoadingState")]
    internal class ComputerLoadingPatch
    {
        private static bool Prefix()
        {
            return true;
        }
    }
}
