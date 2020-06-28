using System;
using UnityEditor;
using UnityEngine;

namespace Simplei18n
{
    [CustomEditor(typeof(SimpleLanguageData))]
    public class SimpleLanguageDataEditor : Editor
    {
        private SimpleLanguageData _languageData;
        private void OnEnable()
        {
            _languageData = (SimpleLanguageData) target;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField(string.Format("Language name: {0}", _languageData.Language.Name));
            EditorGUILayout.LabelField(string.Format("Cultures: {0}", _languageData.Language.Cultures.Count));
            EditorGUILayout.LabelField(string.Format("Translations: {0}", _languageData.Language.RawTranslations.Count));

            if (GUILayout.Button("Edit Language"))
            {
                SimpleLanguageEditWindow.ShowWindow(_languageData);
            }
        }
    }
}