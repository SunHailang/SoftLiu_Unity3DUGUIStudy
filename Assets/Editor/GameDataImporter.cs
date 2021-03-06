﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[InitializeOnLoad]
public class GameDataImporter
{
    static GameDataImporter()
    {
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
    }

    public static string GameDBJsonPath
    {
        get { return string.Format("Assets/Misc/{0}.json", GameDataManager.GameDBPath); }
    }

    public static string GameAssetDBJsonPath
    {
        get { return string.Format("Assets/Resources/{0}.json", /*GameDatabases.GameAssetsDBPath*/0); }
    }

    public static string AIDBJsonPath
    {
        get { return string.Format("Assets/Resources/{0}.json", /*AIDatabase2.AIDBPath*/0); }
    }

    static void OnPlayModeChanged(PlayModeStateChange playModeStateChange)
    {
        if (!EditorApplication.isPlaying && EditorApplication.isPlayingOrWillChangePlaymode)
        {
            //ProcessGameDB();
            //ProcessGameAssetDB();
        }
    }
}
