﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AssetsEditor
{
    [MenuItem("Assets/PSDtoPNG", priority = 5)]
    public static void PSDtoPNG()
    {
        // 获取选择的对象列表
        GameObject[] selectList = Selection.GetFiltered<GameObject>(SelectionMode.TopLevel);
        Debug.Log("egret tools： " + selectList.Length);
        for (int i = 0; i < selectList.Length; i++)
        {
            UnityEngine.Debug.Log(selectList[i].gameObject.name);
            //Egret3DExportTools.ExportPrefabTools.ExportPrefab(null);
        }
    }

    [MenuItem("Assets/PrintSelect", priority = 0)]
    public static void callbackEgretTools()
    {

        
        
    }
}