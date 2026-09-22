using System.Collections;
using HarmonyLib;
using PlayFab;
using UnityEngine;
using TMPro;

namespace GreyServers.Patches
{
    [HarmonyPatch(typeof(GorillaTagger), "Start")]
    public static class OwnerTagPatch
    {
        // Replace this with YOUR PlayFab ID.
        // Every GreyServers user who should see the OWNER tag
        // needs to have the same ID here.
        private const string OwnerPlayFabId = "6933119F642370C1";

        private static GameObject ownerTag;
        private static Coroutine tagCoroutine;

        private static void Postfix(GorillaTagger __instance)
        {
            if (tagCoroutine != null)
                return;

            tagCoroutine = __instance.StartCoroutine(CreateOwnerTag());
        }

        private static IEnumerator CreateOwnerTag()
        {
            // Wait until PlayFab has logged in.
            while (!PlayFabClientAPI.IsClientLoggedIn())
                yield return null;

            string localPlayFabId = PlayFabSettings.staticPlayer.PlayFabId;

            // Only create the tag if THIS client is the owner.
            if (localPlayFabId != OwnerPlayFabId)
                yield break;

            while (GorillaTagger.Instance == null ||
                   GorillaTagger.Instance.offlineVRRig == null)
            {
                yield return null;
            }

            VRRig rig = GorillaTagger.Instance.offlineVRRig;

            if (rig == null)
                yield break;

            CreateTag(rig);
        }

        private static void CreateTag(VRRig rig)
        {
            if (ownerTag != null)
                return;

            // Find the head.
            Transform head = rig.transform.Find("head");

            if (head == null)
            {
                // Fallback if the rig hierarchy is different.
                head = rig.transform;
            }

            ownerTag = new GameObject("GreyServers_OWNER_TAG");
            ownerTag.transform.SetParent(head, false);

            // Position above the head.
            ownerTag.transform.localPosition = new Vector3(0f, 0.45f, 0f);
            ownerTag.transform.localRotation = Quaternion.identity;
            ownerTag.transform.localScale = Vector3.one * 0.01f;

            TextMeshPro text = ownerTag.AddComponent<TextMeshPro>();

            text.text = "OWNER";
            text.fontSize = 5f;
            text.alignment = TextAlignmentOptions.Center;
            text.fontStyle = FontStyles.Bold;

            // Make it readable from either direction.
            text.rectTransform.sizeDelta = new Vector2(100f, 25f);

            // Billboard toward the local camera.
            ownerTag.AddComponent<OwnerTagBillboard>();
        }

        private class OwnerTagBillboard : MonoBehaviour
        {
            private void LateUpdate()
            {
                if (Camera.main == null)
                    return;

                transform.LookAt(Camera.main.transform);
                transform.Rotate(0f, 180f, 0f);
            }
        }
    }
}
