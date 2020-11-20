using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Yodo1.Localization
{
    [System.Serializable]
    public class LanguageSettingsItemData
    {
        [SerializeField] private string _language;
        [SerializeField] private string _languageContent;

        public string language { get { return _language; } }
        public string languageContent { get { return _languageContent; } }

        private void Init(string lan, string lanContent)
        {
            _language = lan;
            _languageContent = lanContent;
        }

        public static LanguageSettingsItemData Create(string lan, string lanContent)
        {
            LanguageSettingsItemData ret = new LanguageSettingsItemData();
            ret.Init(lan, lanContent);
            return ret;
        }
    }

    [System.Serializable]
    public class LanguageSettingsData
    {
        [SerializeField] private string _key;
        [SerializeField] private List<LanguageSettingsItemData> _languages = new List<LanguageSettingsItemData>();

        public List<LanguageSettingsItemData> languages { get { return _languages; } }
        public string key { get { return _key; } }

        public void Add(LanguageSettingsItemData lanData)
        {
            _languages.Add(lanData);
        }

        private bool Init(string lanKey)
        {
            if (string.IsNullOrEmpty(lanKey))
            {
                return false;
            }
            _key = lanKey;
            return true;
        }

        public static LanguageSettingsData Create(string key)
        {
            LanguageSettingsData result = new LanguageSettingsData();
            if (result.Init(key))
            {
                return result;
            }
            return null;
        }
    }
}