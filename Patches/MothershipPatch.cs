using HarmonyLib;

namespace GreyServers.Patches
{
    [HarmonyPatch(typeof(MothershipClientApiUnity), nameof(MothershipClientApiUnity.IsEnabled))]
    public static class MothershipPatch
    {
        public static bool Prefix(ref bool __result)
        {
            __result = false;
            return false;
        }
    }
}
