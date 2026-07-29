using GorillaNetworking;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace GreyServers.Patches
{
    [HarmonyPatch(typeof(CosmeticsController), "GetCosmeticsPlayFabCatalogData")]
    internal class CosmeticPatch
    {
        private static readonly MethodInfo AddCosmeticMethod =
            AccessTools.Method(typeof(CosmeticsController), "AddCosmetic");

        private static readonly FieldInfo OwnedAgeField =
            AccessTools.Field(typeof(CosmeticsController), "_playerOwnedCosmeticsAge");

        private static readonly FieldInfo StarterSetsField =
            AccessTools.Field(typeof(BuilderSetManager), "_starterPieceSets");

        private static readonly FieldInfo InitializedField =
            AccessTools.Field(typeof(CosmeticsController), "initializedCosmetics");

        static bool Prefix(CosmeticsController __instance)
        {
            try
            {
                if (AddCosmeticMethod == null ||
                    OwnedAgeField == null ||
                    InitializedField == null)
                {
                    UnityEngine.Debug.LogError("[GreyServers] Reflection failed.");
                    return true;
                }

                var ownedAge =
                    OwnedAgeField.GetValue(__instance) as Dictionary<string, int>;

                void Add(string id)
                {
                    if (string.IsNullOrEmpty(id))
                        return;

                    AddCosmeticMethod.Invoke(__instance, new object[] { id });

                    ownedAge?.TryAdd(id, 0);
                }

                Add("Slingshot");

                if (BuilderSetManager.instance != null && StarterSetsField != null)
                {
                    var starterSets =
                        StarterSetsField.GetValue(BuilderSetManager.instance) as List<BuilderPieceSet>;

                    if (starterSets != null)
                    {
                        foreach (var set in starterSets)
                            Add(set?.playfabID);
                    }
                }

                if (__instance.allCosmetics != null)
                {
                    foreach (var cosmetic in __instance.allCosmetics)
                        Add(cosmetic?.itemName);
                }

                InitializedField.SetValue(__instance, true);

                return false;
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError($"[GreyServers] Cosmetic patch failed:\n{ex}");
                return true;
            }
        }
    }
}
