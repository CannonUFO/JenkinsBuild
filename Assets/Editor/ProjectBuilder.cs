using UnityEditor;
using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using static BuildSetting;
using log4net.Core;

public class BuildSetting
{
    public enum BuildConfig
    {
        Debug,
        Release
    }

    public string OutputPath
    {
        get
        {
            return Path.Combine(outputPath, PlayerSettings.productName) + GetExtension();
        }
        set
        {
            outputPath = value;
        }
    }
    private string outputPath = Path.GetDirectoryName(Application.dataPath);
    public BuildConfig buildConfig = BuildConfig.Debug;

    public BuildSetting(string outputPath, BuildConfig release)
    {
        OutputPath = outputPath;
        buildConfig = release;
    }

    private string GetExtension()
    {
        string extension = "";

        switch (EditorUserBuildSettings.activeBuildTarget)
        {
            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneWindows64:
                extension = ".exe";
                break;
            case BuildTarget.Android:
                extension = ".apk";
                break;
            case BuildTarget.iOS:
                extension = ".ipa";
                break;
        }

        return extension;
    }
}

public class BuildTool
{

    private static BuildSetting buildSetting = new BuildSetting("", BuildConfig.Release);
    [MenuItem("BuildTool/BuildProject")]
    private static void BuildProject()
    {
        HandleCommandLineArgs();
        HandleBuild();
    }

    private static void HandleBuild()
    {
        BuildTarget buildTarget = EditorUserBuildSettings.activeBuildTarget;//to do°Ñ¼Æ¤Æ

        var buildPlayerOption = new BuildPlayerOptions()
        {
            scenes = EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes),
            locationPathName = buildSetting.OutputPath,
            target = buildTarget,
            options = BuildOptions.None
        };
        
        var report = BuildPipeline.BuildPlayer(buildPlayerOption);
        if (report.summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded)
        {
            Debug.LogFormat("BuildProject Success! Total Time:{0}, Size:{1} bytes", report.summary.totalTime, report.summary.totalSize);
            if (UnityEditorInternal.InternalEditorUtility.inBatchMode)
                EditorApplication.Exit(0);
        }
        else
        {
            if (UnityEditorInternal.InternalEditorUtility.inBatchMode)
                EditorApplication.Exit(1);
            throw new Exception("BuildPlayer failure: " + report.summary.ToString() + "\n" + "Check if scenes is included in BuildSetting");
        }
    }

    private static void HandleCommandLineArgs()
    {
        if (!UnityEditorInternal.InternalEditorUtility.inBatchMode)
            return;
        Dictionary<string, Action<string>> cmdActions = new Dictionary<string, Action<string>>
        {
            {
                "-outputPath", (arg) => {buildSetting.OutputPath = arg; }
            },
            {
                "-buildConfig", (arg) => {buildSetting.buildConfig = (BuildConfig)Enum.Parse(typeof(BuildConfig), arg); }
            }
        };

        Action<string> actionCache;
        string[] cmdArguments = Environment.GetCommandLineArgs();

        for (int count = 0; count < cmdArguments.Length; count++)
        {
            if (cmdActions.ContainsKey(cmdArguments[count]))
            {
                actionCache = cmdActions[cmdArguments[count]];
                actionCache(cmdArguments[count + 1]);
            }
        }
    }


    
}