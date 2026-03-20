using GorillaNetworking;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine.UIElements;

namespace GreyServers.Patches
{
    [HarmonyPatch(typeof(CosmeticsController), "GetCosmeticsPlayFabCatalogData")]
    internal class CosmeticPatch
    {
        private static readonly MethodInfo AddCosmeticMethod = typeof(CosmeticsController).GetMethod("AddCosmetic", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        private static readonly FieldInfo CosAgeDict = typeof(CosmeticsController).GetField("_playerOwnedCosmeticsAge", BindingFlags.Instance | BindingFlags.NonPublic);
        private static readonly FieldInfo BuilderSetsField = typeof(BuilderSetManager).GetField("_starterPieceSets", BindingFlags.Instance | BindingFlags.NonPublic);
        private static readonly FieldInfo InitField = typeof(CosmeticsController).GetField("initializedCosmetics", BindingFlags.Instance | BindingFlags.NonPublic);

        static bool Prefix(CosmeticsController __instance)
        {
            if (AddCosmeticMethod == null) return true;
            if (CosAgeDict == null) return true;
            if (BuilderSetsField == null) return true;
            if (InitField == null) return true;
            void AddCosmetic(string id) => AddCosmeticMethod.Invoke(__instance, new object[] { id });
            var OwnedCosmeticsAge = (Dictionary<string, int>)CosAgeDict.GetValue(__instance);
            AddCosmetic("Slingshot");

            if (BuilderSetManager.instance != null)
            {
                var starterSets = (List<BuilderPieceSet>)BuilderSetsField.GetValue(BuilderSetManager.instance);
                if (starterSets != null)
                {
                    foreach (var set in starterSets)
                    {
                        if (set != null && !string.IsNullOrEmpty(set.playfabID))
                        {
                            AddCosmetic(set.playfabID);
                        }
                    }
                }
            }

            foreach (var item in __instance.allCosmetics)
            {
                if (!string.IsNullOrEmpty(item.itemName))
                {
                    AddCosmetic(item.itemName);

                    if (OwnedCosmeticsAge != null)
                    {
                        OwnedCosmeticsAge[item.itemName] = 0;
                    }
                }
            }

            InitField.SetValue(__instance, true);
            return false;
        }
    }
}
