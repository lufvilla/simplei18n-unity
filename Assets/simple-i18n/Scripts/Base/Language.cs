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
        
        public List<string> Cultures
        {
            get { return cultures; }
    #if UNITY_EDITOR
            set { cultures = value; }
    #endif
        }
        public IDictionary<string, string> TranslationsForKeys
        {
            get { return translationsForKeys; }
    #if UNITY_EDITOR
            set { translationsForKeys = value; }
    #endif
        }

        [SerializeField]
        private string name = string.Empty;
        [SerializeField]
        private List<string> cultures = new List<string>();
        [SerializeField]
        private IDictionary<string, string> translationsForKeys = new Dictionary<string, string>();

        public Language(string name, List<string> cultures, IDictionary<string, string> translationsForKeys)
        {
            this.name = name;
            this.cultures = cultures;
            this.translationsForKeys = translationsForKeys;
        }

        public Language()
        {
        }
    }
}
