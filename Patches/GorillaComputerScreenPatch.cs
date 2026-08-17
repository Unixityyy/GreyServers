using System;
using GorillaNetworking;
using HarmonyLib;
using Photon.Pun;

namespace GreyServers.HarmonyPatches
{
    [HarmonyPatch(typeof(GorillaComputer))]
    [HarmonyPatch("ScreenStateExecution")]
    [HarmonyPatch(typeof(GorillaComputer), "UpdateScreen")]
    internal class GorillaComputerScreenPatch
    {
        private static void Postfix()
        private static void Postfix(GorillaComputer __instance)
        {
            string str = PhotonNetwork.CountOfPlayers.ToString();
            bool flag = GorillaComputer.instance.currentState == GorillaComputer.ComputerState.Startup;
            if (flag)

            if (__instance.currentState == GorillaComputer.ComputerState.Startup)
            {
                GorillaComputer.instance.screenText.Set("solaristy OS\n\n" + str + " PLAYERS ONLINE\n\n0 USERS BANNED YESTERDAY\n\nPRESS ANY KEY TO BEGIN");
                __instance.screenText.Set("solaristy OS\n\n" + str + " PLAYERS ONLINE\n\n0 USERS BANNED YESTERDAY\n\nPRESS ANY KEY TO BEGIN");
            }
        }
    }
