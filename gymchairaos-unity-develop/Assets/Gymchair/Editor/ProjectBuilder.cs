using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;
using System;
using System.IO;
using System.Collections.Generic;

public class ProjectBuilder
{
    [MenuItem("build/apk")]
    static void BuildTest()
    {
        string version = "1.0.0";
        string bundleVersion = "1";
        string package = "kr.co.nanoapps.gymchair";
        string keystoreName = "nanoapps.keystore";
        string keystorePass = "nano2022";
        string keyaliasName = "nanoapps";
        string keyaliasPass = "nano2022";

        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, package);

        PlayerSettings.SplashScreen.show = false;
        PlayerSettings.Android.keystoreName = keystoreName;
        PlayerSettings.keystorePass = keystorePass;
        PlayerSettings.Android.keyaliasName = keyaliasName;
        PlayerSettings.keyaliasPass = keyaliasPass;
        PlayerSettings.Android.bundleVersionCode = int.Parse(bundleVersion);
        PlayerSettings.Android.useAPKExpansionFiles = false;
        PlayerSettings.bundleVersion = version;

        GenericBuild(FindEnabledEditorScenes(), "release.apk", BuildTarget.Android, BuildOptions.None);
    }

    static void AndroidBuild()
    {
        string version = GetArg("-ver");
        string bundleVersion = GetArg("-bundle_ver");
        string package = GetArg("-package");
        string keystoreName = GetArg("-keystoreName");
        string keystorePass = GetArg("-keystorePass");
        string keyaliasName = GetArg("-keyaliasName");
        string keyaliasPass = GetArg("-keyaliasPass");

        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, package);

        PlayerSettings.SplashScreen.show = false;
        PlayerSettings.Android.keystoreName = keystoreName;
        PlayerSettings.keystorePass = keystorePass;
        PlayerSettings.Android.keyaliasName = keyaliasName;
        PlayerSettings.keyaliasPass = keyaliasPass;
        PlayerSettings.Android.bundleVersionCode = int.Parse(bundleVersion);
        PlayerSettings.Android.useAPKExpansionFiles = false;
        PlayerSettings.bundleVersion = version;

        GenericBuild(FindEnabledEditorScenes(), "release.apk", BuildTarget.Android, BuildOptions.None);
        //File.Copy("Build/release_full.apk", $"{buildPath}/{appName}_{versionCode}_FULL.apk");
        //File.Delete("Build/release_full.apk");
    }

    private static string[] FindEnabledEditorScenes()
    {
        List<string> EditorScenes = new List<string>();

        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (!scene.enabled)
                continue;

            EditorScenes.Add(scene.path);
        }
        return EditorScenes.ToArray();
    }

    static void GenericBuild(string[] scenes, string target_filename, BuildTarget build_target, BuildOptions build_options)
    {
        BuildPlayerOptions options = new BuildPlayerOptions();
        options.scenes = scenes;
        options.locationPathName = target_filename;
        options.target = build_target;
        options.options = build_options;

        BuildReport report = BuildPipeline.BuildPlayer(options);

        var summary = report.summary;

        Console.WriteLine($"**** summary.result : {summary.result}");

        if (summary.result == BuildResult.Succeeded)
        {
            Console.WriteLine("**** Succeeded!");
        }
        else if (summary.result == BuildResult.Failed)
        {
            Console.WriteLine("**** Failed!");
            foreach (var step in report.steps)
            {
                foreach (var message in step.messages)
                {
                    Console.WriteLine("****" + message);
                }
            }
        }
        else if (summary.result == BuildResult.Cancelled)
        {
            Console.WriteLine("**** Cancelled!");
        }
        else
        {
            Console.WriteLine("**** Unknown!");
        }
    }

    static string GetArg(string name)
    {
        var args = System.Environment.GetCommandLineArgs();

        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == name && args.Length > i + 1)
            {
                return args[i + 1];
            }
        }

        return null;
    }
}
