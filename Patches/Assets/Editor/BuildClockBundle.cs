#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System.IO;

public static class BuildClockBundle
{
    [MenuItem("GorillaTagClock/Build AssetBundle")]
    public static void Build()
    {
        string outputFolder = Path.Combine(
            Directory.GetParent(Application.dataPath).FullName,
            "Build"
        );

        if (!Directory.Exists(outputFolder))
            Directory.CreateDirectory(outputFolder);

        string prefabPath =
            "Assets/Clock/GorillaTagClock.prefab";

        if (AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath) == null)
        {
            Debug.LogError(
                "[GorillaTagClock] GorillaTagClock.prefab was not found!"
            );

            return;
        }

        AssetBundleBuild bundle = new AssetBundleBuild
        {
            assetBundleName = "gorillatagclock",
            assetNames = new[]
            {
                prefabPath
            }
        };

        BuildPipeline.BuildAssetBundles(
            outputFolder,
            new[]
            {
                bundle
            },
            BuildAssetBundleOptions.None,
            BuildTarget.StandaloneWindows64
        );

        Debug.Log(
            "[GorillaTagClock] Successfully built AssetBundle!"
        );

        Debug.Log(
            "[GorillaTagClock] Output: " +
            Path.Combine(
                outputFolder,
                "gorillatagclock"
            )
        );
    }
}

#endif
