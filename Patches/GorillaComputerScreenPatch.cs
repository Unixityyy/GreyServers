using System;
using GorillaNetworking;
using HarmonyLib;
using Photon.Pun;

namespace GreyServers.HarmonyPatches
{
    [HarmonyPatch(typeof(GorillaComputer), "UpdateScreen")]
    internal class GorillaComputerScreenPatch
    {
        private static void Postfix(GorillaComputer __instance)
        {
            string str = PhotonNetwork.CountOfPlayers.ToString();

            if (__instance.currentState == GorillaComputer.ComputerState.Startup)
            {
                __instance.screenText.Set("UNIXITY OS\n\n" + str + " PLAYERS ONLINE\n\n0 USERS BANNED YESTERDAY\n\nPRESS ANY KEY TO BEGIN");
            }
        }
    }
}