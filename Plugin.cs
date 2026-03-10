using System;
using UnityEngine;
using PlayFab;
using GorillaNetworking;
using GreyServers;

#if BEPINEX_RELEASE
using BepInEx;
#elif MELONLOADER_RELEASE
using MelonLoader;
[assembly: MelonInfo(typeof(GreyServers.Plugin), PluginInfo.Name, PluginInfo.Version, "Unixity")]
[assembly: MelonGame("Another Axiom", "Gorilla Tag")]
#endif

namespace GreyServers
{
#if BEPINEX_RELEASE
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            Initialize();
        }
#elif MELONLOADER_RELEASE
    public class Plugin : MelonMod
    {
        public override void OnInitializeMelon()
        {
            Initialize();
        }
#endif

        private void Initialize()
        {
            Debug.Log("current title id is " + PlayFabSettings.TitleId);
            PlayFabSettings.TitleId = "1457F3";
            PlayFabAuthenticatorSettings.TitleId = "1457F3";

            GreyServers.HarmonyPatches.HarmonyPatches.ApplyHarmonyPatches();

            Debug.Log("setting titleid to " + PlayFabSettings.TitleId);
#if BEPINEX_RELEASE
            Debug.Log(PlayFabAuthenticatorSettings.AuthApiBaseUrl);
            UnityEngine.Object.DontDestroyOnLoad(this.gameObject);
#elif MELONLOADER_RELEASE
            Debug.Log(PlayFabAuthenticatorSettings.AuthApiBaseUrl);
            GameObject target = new GameObject("GreyServers");
            UnityEngine.Object.DontDestroyOnLoad(target);
#endif
        }
    }
}