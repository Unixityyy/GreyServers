using System.Collections;
using HarmonyLib;
using PlayFab;
using UnityEngine;

namespace GreyServers.Patches
{
    [HarmonyPatch(typeof(GorillaTagger), "Start")]
    public static class OwnerTagPatch
    {
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
            while (!PlayFabClientAPI.IsClientLoggedIn())
                yield return null;

            string playFabId = PlayFabSettings.staticPlayer.PlayFabId;

            if (playFabId != OwnerPlayFabId)
                yield break;

            while (GorillaTagger.Instance == null ||
                   GorillaTagger.Instance.offlineVRRig == null)
            {
                yield return null;
            }

            CreateTag(GorillaTagger.Instance.offlineVRRig);
        }

        private static void CreateTag(VRRig rig)
        {
            if (ownerTag != null)
                return;

            Transform head = rig.transform.Find("head");

            if (head == null)
                head = rig.transform;

            ownerTag = new GameObject("GreyServers_OWNER_TAG");

            ownerTag.transform.SetParent(head, false);
            ownerTag.transform.localPosition = new Vector3(0f, 0.45f, 0f);
            ownerTag.transform.localScale = Vector3.one * 0.01f;

            TextMesh text = ownerTag.AddComponent<TextMesh>();

            text.text = "OWNER";
            text.fontSize = 300;
            text.characterSize = 1f;
            text.anchor = TextAnchor.MiddleCenter;
            text.alignment = TextAlignment.Center;

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
