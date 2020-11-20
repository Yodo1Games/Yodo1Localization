using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
#if UNITY_EDITOR
namespace Yodo1.Localization
{
    public class LanguageSettings : ScriptableObject
    {
        [SerializeField] private List<LanguageSettingsData> _languageSettingsDatas = new List<LanguageSettingsData>();
        [SerializeField] private List<string> _languageCodes = new List<string>();

        public List<LanguageSettingsData> GetLanguageSettingsData()
        {
            return _languageSettingsDatas;
        }

        public List<string> GetLanguageCodes()
        {
            return _languageCodes;
        }


        public void AddLanguageSettingsData(LanguageSettingsData settingsData)
        {
            _languageSettingsDatas.Add(settingsData);
        }

        public void ParseLanguageCode(string language)
        {
            //Chinese (Simplified)
            language = language.Replace(" ", "");
            language = language.Replace(")", "");
            language = language.Replace("(", "/");

            List<string> temp = LanguageUtils.GetLanguageCode(language);
            foreach (string itemData in temp)
            {
                if (_languageCodes.Contains(itemData))
                {
                    continue;
                }
                _languageCodes.Add(itemData);
            }
        }

        private static LanguageSettings _instance;

        public static LanguageSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GetOrCreateInstance();
                }
                return _instance;
            }
        }
        #region Saving/Loading
        private static LanguageSettings GetOrCreateInstance()
        {
            LanguageSettings instance = null;

            // Debug.Log(">>> AssetPath: " + AssetPath);
            instance = AssetDatabase.LoadAssetAtPath<LanguageSettings>(AssetPath);
            // Debug.Log(">>> instance: " + instance);       





            if (instance == null)
            {
                // Create instance
                instance = CreateInstance<LanguageSettings>();
                Debug.Log(">>>> Create new Settings: " + AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(instance)));


                // Get resources folder path

                if (Directory.Exists(ResourcesPath) == false)
                {
                    // Create directory if it doesn't exist
                    Directory.CreateDirectory(ResourcesPath);
                }
                Debug.LogFormat("Creating settings asset at {0}/{1}", ResourcesPath, SettingsFileName);

                // Save instance if in editor
                AssetDatabase.CreateAsset(instance, AssetPath);
                instance.Save();
            }

            return instance;
        }
        #endregion



        public void Save()
        {
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public void Clear()
        {
            _languageSettingsDatas.Clear();
            _languageCodes.Clear();
        }

        private static string SettingsFileName { get { return "LanguageSettings"; } }
        private static string ResourcesPath { get { return Application.dataPath + "/Yodo1SDK_Extension/Editor/Localization/"; } }
        private static string AssetPath { get { return "Assets/Yodo1SDK_Extension/Editor/Localization/" + SettingsFileName + ".asset"; } }
    }
}
#endif
