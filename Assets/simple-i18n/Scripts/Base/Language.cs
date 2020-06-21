using System;
using System.Collections.Generic;
using UnityEngine;

namespace Simplei18n
{
    [Serializable]
    public class Language
    {
        public string Name 
        {
            get { return name; }
    #if UNITY_EDITOR
            set { name = value; }
    #endif
        }
        
        public List<string> Cultures => cultures;
        public IDictionary<string, string> TranslationsForKeys => translationsForKeys;
        public List<Translations> RawTranslations => translations;

        [SerializeField]
        private string name = string.Empty;
        [SerializeField]
        private List<string> cultures = new List<string>();
        [SerializeField]
        private List<Translations> translations = new List<Translations>(); 
        
        private IDictionary<string, string> translationsForKeys = new Dictionary<string, string>();

        public void GenerateCache()
        {
            translationsForKeys.Clear();
            
            foreach (var translation in translations)
            {
                if (!translationsForKeys.ContainsKey(translation.Key))
                {
                    translationsForKeys.Add(translation.Key, translation.Value);
                }
            }
        }

        public Language(string name, List<string> cultures, IDictionary<string, string> translationsForKeys)
        {
            this.name = name;
            this.cultures = cultures;
            this.translationsForKeys = translationsForKeys;
        }

        public Language()
        {
        }

        [Serializable]
        public class Translations
        {
            public string Key => key;

            public string Value
            {
                get => value;
                set => this.value = value;
            }

            [SerializeField]
            private string key = string.Empty;

            [SerializeField]
            private string value = string.Empty;

            public Translations(string key, string value)
            {
                this.key = key;
                this.value = value;
            }
        }
    }
}
