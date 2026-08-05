using GorillaNetworking;
using HarmonyLib;
using PlayFab;
using PlayFab.ClientModels;

namespace GreyServers.Patches
{
    [HarmonyPatch(typeof(CosmeticsController), "PurchaseItem")]
    public static class Patch_PurchaseItem
    {
        private static bool Prefix(CosmeticsController __instance)
        {
            if (__instance == null)
                return false;

            CosmeticsController controller = __instance;

            if (controller.itemToBuy.isNullItem ||
                controller.itemToBuy.itemName == controller.nullItem.itemName)
            {
                return false;
            }

            string id = controller.itemToBuy.itemName;
            var cosmetic = controller.itemToBuy;

            ExecuteCloudScriptRequest request = new ExecuteCloudScriptRequest
            {
                FunctionName = "buyFreeCosmetic",
                FunctionParameter = new
                {
                    itemID = id
                }
            };

            PlayFabClientAPI.ExecuteCloudScript(
                request,
                result =>
                {
                    if (!controller.allCosmeticsDict.ContainsKey(id))
                    {
                        controller.allCosmeticsDict.Add(id, cosmetic);
                    }

                    if (GorillaTagger.Instance != null &&
                        GorillaTagger.Instance.offlineVRRig != null)
                    {
                        GorillaTagger.Instance.offlineVRRig.GetCosmeticsPlayFabCatalogData();
                    }

                    controller.itemToBuy = controller.nullItem;
                },
                error =>
                {
                    controller.itemToBuy = controller.nullItem;
                }
            );

            return false;
        }
    }
}
