using System.Collections.Generic;
using UnityEngine;

namespace Simplei18n
{
    [CreateAssetMenu(menuName = "SimpleLocalization/Create Language", fileName = "NewLanguage")]
    public class SimpleLanguageData : ScriptableObject
    {
        public Language Language
        {
            get { return language; }
#if UNITY_EDITOR
            set { language = value; }
#endif
        }

        [SerializeField]
        private Language language;
    }
}