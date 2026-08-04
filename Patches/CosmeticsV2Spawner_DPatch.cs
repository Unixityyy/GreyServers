using HarmonyLib;
using GorillaNetworking;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GreyServers.Patches
{
    [HarmonyPatch(typeof(CosmeticsV2Spawner_Dirty), "ProcessLoadOpInfos")]
    public static class CosmeticsV2Spawner_DPatch
    {
        private static bool Prefix(VRRig rig, string playfabId)
        {
            if (rig == null || string.IsNullOrEmpty(playfabId))
                return true;

            Traverse traverse = Traverse.Create(typeof(CosmeticsV2Spawner_Dirty));

            var dict = traverse
                .Field("_gVRRigDatasIndexByRig")
                .GetValue<Dictionary<VRRig, int>>();

            if (dict == null)
                return true;

            if (!dict.ContainsKey(rig))
            {
                Debug.LogWarning($"{rig.gameObject.name} not found in rig dictionary.");

                if (rig.isOfflineVRRig)
                {
                    dict[rig] = 0;
                }
                else
                {
                    return true;
                }
            }

            int index = dict[rig];

            Array loadOpArray = traverse
                .Field("_g_loadOpInfosForRigAndCosmeticIDDicts")
                .GetValue() as Array;

            if (loadOpArray == null || index >= loadOpArray.Length)
                return true;

            object data = loadOpArray.GetValue(index);

            if (data == null)
            {
                Debug.LogWarning($"Missing cosmetic load data for index {index}");

                try
                {
                    traverse.Method("PrepareLoadOpInfos").GetValue();

                    loadOpArray = traverse
                        .Field("_g_loadOpInfosForRigAndCosmeticIDDicts")
                        .GetValue() as Array;
                }
                catch (Exception e)
                {
                    Debug.LogError($"PrepareLoadOpInfos failed: {e}");
                }
            }

            if (loadOpArray == null || index >= loadOpArray.Length)
                return true;

            IDictionary rigDict = loadOpArray.GetValue(index) as IDictionary;

            if (rigDict == null)
            {
                Debug.LogError($"Cosmetic dictionary missing for rig index {index}");
                return true;
            }

            if (!rigDict.Contains(playfabId))
            {
                Debug.LogWarning($"Cosmetic {playfabId} not found.");
                return true;
            }

            return true;
        }
    }
}
