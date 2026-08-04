using HarmonyLib;
using GorillaNetworking;

namespace GreyServers.Patches
{
    [HarmonyPatch(typeof(CosmeticsController.CosmeticSet), "ToPackedIDArray")]
    public static class PackedIDArrayPatch
    {
        private static void Prefix(CosmeticsController.CosmeticSet __instance)
        {
            if (__instance == null || __instance.items == null)
                return;

            CosmeticsController controller = CosmeticsController.instance;

            if (controller == null || controller.nullItem == null)
                return;

            for (int i = 0; i < __instance.items.Length; i++)
            {
                if (__instance.items[i] == null ||
                    string.IsNullOrEmpty(__instance.items[i].itemName))
                {
                    __instance.items[i] = controller.nullItem;
                }
            }
        }
    }
}
