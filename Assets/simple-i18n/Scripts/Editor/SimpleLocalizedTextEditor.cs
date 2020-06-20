using System;
using UnityEditor;
using UnityEngine;

namespace Simplei18n
{
    [CustomEditor(typeof(SimpleLocalizedText))]
    public class SimpleLocalizedTextEditor : Editor
    {
        private static readonly string[] _expcludeProps = {"m_Script"};
        
        private SerializedProperty _localizationKey;
        void OnEnable()
        {
            _localizationKey = serializedObject.FindProperty("localizationKey");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            DrawCustomInspector();
            
            DrawPropertiesExcluding(serializedObject, _expcludeProps);
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawCustomInspector()
        {
            EditorGUILayout.PropertyField(_localizationKey);

            if (GUI.changed)
            {
                _localizationKey.stringValue = _localizationKey.stringValue.Trim();
            }

            if (SimpleLocalizationWindow.CurrentKeys == null)
            {
                EditorGUILayout.HelpBox("Warning, keys file now found. Key validation is disabled.", MessageType.Warning);
            }
            else if (!SimpleLocalizationWindow.CurrentKeys.Keys.Contains(_localizationKey.stringValue))
            {
                EditorGUILayout.HelpBox(string.Format("Key '{0}' not found in Keys file.\nCheck your Keys files in '{1}'", _localizationKey.stringValue, SimpleLocalizationWindow.KeysFilePath), MessageType.Warning);
                /*EditorWindowHelper.HorizontalLayout(() =>
                {
                    if (GUILayout.Button(string.Format("Add '{0}' key", _localizationKey.stringValue)))
                    {
                        SimpleLocalizationWindow.CurrentKeys.Keys.Add(_localizationKey.stringValue);
                        EditorUtility.SetDirty(SimpleLocalizationWindow.CurrentKeys);
                        AssetDatabase.SaveAssets();
                    }
                });*/
            }
            
            EditorWindowHelper.DrawUILine(Color.grey);
        }
    }
}
