using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Data;
namespace Yodo1.Localization
{
    [CustomEditor(typeof(LanguageSettings))]
    public class ATTLanguageSettingsEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Import Config", GUILayout.Height(32)))
            {
                LoadConfig();
            }

            if (GUILayout.Button("Apply", GUILayout.Height(32)))
            {
                SaveSettings();
            }
        }

        void SaveSettings()
        {
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        void LoadConfig()
        {
            string configFile = EditorUtility.OpenFilePanel("请选择配置文件文件(xlsx, xlx))", "Assets/Yodo1SDK_Extension/Editor/Localization/", "xlsx,xlx,XLSX,XLX");
            if (string.IsNullOrEmpty(configFile))
            {
                return;
            }
            LanguageSettings.Instance.Clear();
            XlsReader xlsReader = new XlsReader(configFile);

            int rowLength = xlsReader.GetRowLength(0);
            int colLength = xlsReader.GetColLength(0);
            List<string> headList = new List<string>();
            DataRow dataRow = xlsReader.GetRowData(0);

            for (int index = 0; index < colLength; ++index)
            {
                string headerValue = dataRow[index].ToString();
                if (string.IsNullOrEmpty(headerValue))
                {
                    break;
                }
                headList.Add(headerValue);
                if (index >= 1)
                {
                    LanguageSettings.Instance.ParseLanguageCode(headerValue);
                }
            }
            colLength = headList.Count;
            for (int rowIndex = 1; rowIndex < rowLength; ++rowIndex)
            {
                DataRow dataRow1 = xlsReader.GetRowData(rowIndex);
                if (dataRow1 == null)
                {
                    return;
                }
                LanguageSettingsData settingsData = LanguageSettingsData.Create(dataRow1[0].ToString());
                if (settingsData == null)
                {
                    continue;
                }
                for (int colIndex = 1; colIndex < colLength; ++colIndex)
                {
                    settingsData.Add(LanguageSettingsItemData.Create(headList[colIndex], dataRow1[colIndex].ToString()));
                }
                LanguageSettings.Instance.AddLanguageSettingsData(settingsData);
            }
            LanguageSettings.Instance.Save();
            Debug.Log("Import Config Success !");
        }
    }
}
