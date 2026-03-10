using System;
using System.Reflection;
using HarmonyLib;

namespace GreyServers.HarmonyPatches
{
    public class HarmonyPatches
    {
        public static bool IsPatched { get; private set; }
        private static HarmonyLib.Harmony instance;

        internal static void ApplyHarmonyPatches()
        {
            if (!IsPatched)
            {
                if (instance == null)
                {
                    instance = new HarmonyLib.Harmony(PluginInfo.GUID);
                }
                instance.PatchAll(Assembly.GetExecutingAssembly());
                IsPatched = true;
            }
        }

        internal static void RemoveHarmonyPatches()
        {
            if (instance != null && IsPatched)
            {
                instance.UnpatchSelf();
                IsPatched = false;
            }
        }
    }
}