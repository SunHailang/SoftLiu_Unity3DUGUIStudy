﻿using SoftLiu.Build;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using Force.Crc32;
using SoftLiu.AssetBundles.Downloader;


public class AssetBundlesMenuEditor
{
    [MenuItem("SoftLiu/AssetBundles/Android/Build Development", false, 0)]
    public static void AssetBundles_BuildAndroidDev()
    {
        BuildTarget platform = BuildTarget.Android;
        BuildType buildType = BuildType.Development;
        string buildDir = Application.dataPath + "/../Builds/AssetBundles/" + platform.ToString() + "/" + buildType.ToString() + "/" + Application.version;
        if (Directory.Exists(buildDir))
        {
            Directory.Delete(buildDir, true);
        }
        Directory.CreateDirectory(buildDir);
        BuildPipeline.BuildAssetBundles(buildDir, BuildAssetBundleOptions.UncompressedAssetBundle, platform);
        GenerateCRCFileInfoPlatform(platform, buildType);
    }


    public static void GenerateCRCFileInfoPlatform(BuildTarget platform, BuildType buildType)
    {
        UnityEngine.Debug.Log("GenerateCRCFileInfo " + platform.ToString());
        try
        {
            string buildDir = Application.dataPath + "/../Builds/AssetBundles/" + platform.ToString() + "/" + buildType.ToString() + "/" + Application.version;
            if (Directory.Exists(buildDir))
            {
                if (File.Exists(buildDir + "/assetbundles.crc"))
                {
                    File.Delete(buildDir + "/assetbundles.crc");
                }
                List<AssetBundleCRCInfo> m_crcInfoList = new List<AssetBundleCRCInfo>();
                string[] assetBundles = Directory.GetFiles(buildDir);
                foreach (string abFileName in assetBundles)
                {
                    FileInfo abFile = new FileInfo(abFileName);
                    if (abFile.FullName.Contains(".manifest"))
                    {
                        File.Delete(abFile.FullName);
                    }
                    else if (abFile.Name != platform.ToString() && abFile.Name != Application.version)
                    {
                        AssetBundleCRCInfo abcrci = new AssetBundleCRCInfo();
                        abcrci.m_name = abFile.Name;
                        abcrci.m_fileSizeBytes = abFile.Length;
                        abcrci.m_CRC = GenerateCRC32FromFile(abFile.FullName);
                        m_crcInfoList.Add(abcrci);
                    }
                }
                string fileData = "";
                foreach (AssetBundleCRCInfo abcrci in m_crcInfoList)
                {
                    if (fileData != "")
                    {
                        fileData = fileData + "\n";
                    }
                    fileData = fileData + abcrci.m_name + "|" + abcrci.m_CRC.ToString() + "|" + abcrci.m_fileSizeBytes;
                }
                File.WriteAllText(buildDir + "/assetbundles.crc", fileData);
            }
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.Log("GenerateCRCFileInfoPlatform Failed " + e.Message);
        }
    }

    public static uint GenerateCRC32FromFile(string fileName)
    {
#if UNITY_EDITOR
        Stopwatch x = new Stopwatch();
        x.Start();
#endif
        uint crc = 0;
        FileStream fs = null;
        try
        {
            fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            int i = 0;
            int block = 4096;
            byte[] buffer = new byte[block];
            int l = (int)fs.Length;
            while (i < l)
            {
                fs.Read(buffer, 0, block);
                crc = Crc32Algorithm.Append(crc, buffer);
                i += block;
            }
            fs.Close();
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError("AssetBundleUtils | GenerateCRC32FromFile failed to generate CRC: " + fileName + ". Exception: " + e.ToString());
        }
        finally
        {
            if (fs != null)
            {
                fs.Close();
            }
        }
#if UNITY_EDITOR
        x.Stop();
        UnityEngine.Debug.Log("CRC32 " + fileName + ":" + x.ElapsedMilliseconds + "ms <> " + crc.ToString());
#endif
        return crc;
    }
}