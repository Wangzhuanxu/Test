using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build.Content;
using UnityEditor.SceneManagement;
using UnityEngine;

public class TEstEditor 
{
    public static string[] GetLevelsFromBuildSettings()
    {
        PreDealScenes ();
            
        List<string> levels = new List<string>();
        for (int i = 0; i < EditorBuildSettings.scenes.Length; ++i)
        {
            if (EditorBuildSettings.scenes[i].enabled)
                levels.Add(EditorBuildSettings.scenes[i].path);
        }

        return levels.ToArray();
    }

    private static void PreDealScenes ()
    {
        EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
        foreach (EditorBuildSettingsScene scene in scenes)
        {
            if(scene.path.Contains ("PveSkill"))
            {
                scene.enabled = false;
            }
        }
        // 这里重新赋值给 EditorBuildSettings.scenes ，否则设置失败！
        EditorBuildSettings.scenes = scenes;
    }
    public static void Build()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = GetLevelsFromBuildSettings();
        buildPlayerOptions.locationPathName = "";
        buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
        buildPlayerOptions.options = BuildOptions.None;
        BuildPipeline.BuildPlayer(buildPlayerOptions);
    }
}
