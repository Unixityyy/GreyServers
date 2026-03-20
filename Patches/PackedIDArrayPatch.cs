using HarmonyLib;
using GorillaNetworking;

[HarmonyPatch(typeof(CosmeticsController.CosmeticSet), "ToPackedIDArray")]
public class PackedIDArrayPatch
{
    private static void Prefix(CosmeticsController.CosmeticSet __instance)
    {
        if (__instance.items == null) return;

        var controller = CosmeticsController.instance;
        if (controller == null) return;

        for (int i = 0; i < __instance.items.Length; i++)
        {
            if (string.IsNullOrEmpty(__instance.items[i].itemName))
            {
                __instance.items[i] = controller.nullItem;
            }
        }
    }
}