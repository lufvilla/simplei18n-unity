using System.Collections.Generic;
using UnityEngine;

namespace Simplei18n
{
    public class SimpleLanguageConfig : ScriptableObject
    {
        public bool AutodetectLanguage
        {
            get => autodetectLanguage;
    #if UNITY_EDITOR
            set => autodetectLanguage = value;
    #endif
        }

        public SimpleLanguageData DefaultLanguage
        {
            get => defaultLanguage;
    #if UNITY_EDITOR
            set => defaultLanguage = value;
    #endif
        }

        public List<SimpleLanguageData> Languages => languages;
        
        [SerializeField]
        private bool autodetectLanguage;
        
        [SerializeField]
        private SimpleLanguageData defaultLanguage;

        [SerializeField]
        private List<SimpleLanguageData> languages = new List<SimpleLanguageData>();
    }
}
