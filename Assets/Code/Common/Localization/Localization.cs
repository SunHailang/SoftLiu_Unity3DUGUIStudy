﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using SoftLiu.Utilities;

namespace SoftLiu.Localization
{
    public class Localization : AutoGeneratedSingleton<Localization>
    {
        private Dictionary<string, string> m_localizationVals = new Dictionary<string, string>();

        private const string m_path = "/Resources/Localization_Chinese.txt";

        private string m_pathLanguage = string.Empty;

        public Localization()
        {
            m_pathLanguage = Application.dataPath + m_path;
            ReadCSVFile(m_pathLanguage);
        }

        public void Init()
        {
            
        }

        private void ReadCSVFile(string path)
        {
            Dictionary<string, string> mTemp = new Dictionary<string, string>();
            mTemp.Clear();
            string[] lines = File.ReadAllLines(path, Encoding.UTF8);
            if (lines.Length <= 0)
            {
                return;
            }
            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrEmpty(lines[i]))
                {
                    continue;
                }
                int index = lines[i].IndexOf(',');
                string key = lines[i].Substring(0, index);
                string value = lines[i].Substring(index + 1, lines[i].Length - index - 1);

                mTemp.Add(key, value);
            }
            m_localizationVals = mTemp;
        }

        public string Get(string key)
        {
            if (!m_localizationVals.ContainsKey(key))
            {
                return key;
            }
            return m_localizationVals[key];
        }

        public string Format(string key, params object[] parameters)
        {
            if (!m_localizationVals.ContainsKey(key))
            {
                return key;
            }
            return string.Format(Get(key), parameters);
        }
    }
}
