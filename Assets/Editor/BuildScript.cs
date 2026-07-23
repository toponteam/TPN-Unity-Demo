using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class BuildScript
{
    [MenuItem("Build/Build Android APK")]
    public static void BuildAndroidAPK()
    {
        string[] scenes = { "Assets/Scenes/HomeScreenScene.unity", "Assets/Scenes/AutomicRewardInterScene.unity", "Assets/Scenes/AnyThinkAdDemoScene.unity" };
        string outputPath = "Build/Android/AnyThinkUnitySDK.apk";
        var buildOptions = new BuildPlayerOptions { scenes = scenes, locationPathName = outputPath, targetGroup = BuildTargetGroup.Android, target = BuildTarget.Android, options = BuildOptions.None };
        BuildReport report = BuildPipeline.BuildPlayer(buildOptions);
        BuildSummary summary = report.summary;
        if (summary.result == BuildResult.Succeeded) Debug.Log($"Build succeeded: {outputPath} ({summary.totalSize / 1048576.0:F1} MB)");
        else if (summary.result == BuildResult.Failed) Debug.LogError($"Build failed: {summary.result}");
    }

    public static void BuildAndroid()
    {
        EditorUserBuildSettings.exportAsGoogleAndroidProject = false;
        string[] scenes = { "Assets/Scenes/HomeScreenScene.unity", "Assets/Scenes/AutomicRewardInterScene.unity", "Assets/Scenes/AnyThinkAdDemoScene.unity" };
        string outputPath = "Build/Android/AnyThinkUnitySDK.apk";
        var buildOptions = new BuildPlayerOptions { scenes = scenes, locationPathName = outputPath, targetGroup = BuildTargetGroup.Android, target = BuildTarget.Android, options = BuildOptions.None };
        BuildReport report = BuildPipeline.BuildPlayer(buildOptions);
        BuildSummary summary = report.summary;
        if (summary.result == BuildResult.Succeeded) { Debug.Log($"Build succeeded: {outputPath} ({summary.totalSize / 1048576.0:F1} MB)"); EditorApplication.Exit(0); }
        else { Debug.LogError($"Build failed: {summary.result}"); EditorApplication.Exit(1); }
    }

    public static void BuildiOS()
    {
        string[] scenes = { "Assets/Scenes/HomeScreenScene.unity", "Assets/Scenes/AutomicRewardInterScene.unity", "Assets/Scenes/AnyThinkAdDemoScene.unity" };
        string outputPath = "Build/iOS";
        var buildOptions = new BuildPlayerOptions { scenes = scenes, locationPathName = outputPath, targetGroup = BuildTargetGroup.iOS, target = BuildTarget.iOS, options = BuildOptions.None };
        BuildReport report = BuildPipeline.BuildPlayer(buildOptions);
        BuildSummary summary = report.summary;
        if (summary.result == BuildResult.Succeeded) { Debug.Log($"iOS Build succeeded: {outputPath} ({summary.totalSize / 1048576.0:F1} MB)"); EditorApplication.Exit(0); }
        else { Debug.LogError($"iOS Build failed: {summary.result}"); EditorApplication.Exit(1); }
    }

    public static void ExportAndroidProject()
    {
        EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;
        EditorUserBuildSettings.exportAsGoogleAndroidProject = true;
        string[] scenes = { "Assets/Scenes/HomeScreenScene.unity", "Assets/Scenes/AutomicRewardInterScene.unity", "Assets/Scenes/AnyThinkAdDemoScene.unity" };
        string outputPath = "Build/AndroidProject";
        var buildOptions = new BuildPlayerOptions { scenes = scenes, locationPathName = outputPath, targetGroup = BuildTargetGroup.Android, target = BuildTarget.Android, options = BuildOptions.None };
        BuildReport report = BuildPipeline.BuildPlayer(buildOptions);
        BuildSummary summary = report.summary;
        if (summary.result == BuildResult.Succeeded) { Debug.Log($"Android project exported: {outputPath} ({summary.totalSize / 1048576.0:F1} MB)"); EditorApplication.Exit(0); }
        else { Debug.LogError($"Android project export failed: {summary.result}"); EditorApplication.Exit(1); }
    }
}
