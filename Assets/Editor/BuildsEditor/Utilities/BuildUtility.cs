﻿using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace SoftLiu.Utilities
{
    public class BuildUtility : AutoGeneratedSingleton<BuildUtility>
    {
        private string m_buildDirectory = string.Empty;


        public void CopyFileRecursively(DirectoryInfo source, DirectoryInfo target)
        {
            foreach (DirectoryInfo dir in source.GetDirectories())
            {
                CopyFileRecursively(dir, target.CreateSubdirectory(dir.Name));
            }
            foreach (FileInfo file in source.GetFiles())
            {
                // never copy meta files
                if (!file.Name.Contains(".meta"))
                {
                    file.CopyTo(Path.Combine(target.FullName, file.Name), true);
                }
            }
        }

        private bool DeleteEmptyDirs(string directory)
        {
            bool didDelete = false;
            string[] directoriesdirectories = Directory.GetDirectories(directory);
            for (int i = 0; i < directoriesdirectories.Length; i++)
            {
                string dir = directoriesdirectories[i];
                int fileCount = Directory.GetFiles(dir).Length + Directory.GetDirectories(dir).Length;
                if (fileCount > 0)
                {
                    if (DeleteEmptyDirs(dir))
                    {
                        i--;
                    }
                }
                else
                {
                    Directory.Delete(dir);
                    didDelete = true;
                }
            }

            return didDelete;
        }

        public void RunGradleProcess(string buildPath, string gradleBuildType, string packageType = "assemble")
        {
            string directory = Path.Combine(buildPath, Application.productName);

            string executable = Path.Combine(directory, "gradlew.bat");
            string arguments = packageType + gradleBuildType;

            // Run python to start build
            ProcessStartInfo procStartInfo = new ProcessStartInfo();
            procStartInfo.FileName = executable;
            procStartInfo.Arguments = arguments;
            procStartInfo.UseShellExecute = false;
            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.RedirectStandardError = true;
            procStartInfo.WorkingDirectory = directory;
            procStartInfo.CreateNoWindow = true;
            Process proc = new Process();
            Debug.Log("RunGradleProcess: " + executable + " " + arguments);
            proc.StartInfo = procStartInfo;
            proc.Start();
            string result = proc.StandardOutput.ReadToEnd();
            string error = proc.StandardError.ReadToEnd();

            string gradleLog = "/gradle_" + packageType + ".log";
            string gradleErrorLog = "/gradle_error_" + packageType + ".log";
            if (result.Length > 1)
            {
                if (File.Exists(buildPath + gradleLog))
                {
                    File.Delete(buildPath + gradleLog);
                }
                File.WriteAllText(buildPath + gradleLog, result);
            }
            if (error.Length > 0)
            {
                if (File.Exists(buildPath + gradleErrorLog))
                {
                    File.Delete(buildPath + gradleErrorLog);
                }
                File.WriteAllText(buildPath + gradleErrorLog, error);
            }
            proc.Close();
        }
    }
}
