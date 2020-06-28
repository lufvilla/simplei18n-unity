
using System;
using UnityEditor;
using UnityEngine;

namespace Simplei18n
{
    public class SimpleLocalizedEditor : Editor
    {
        private static readonly string[] _expcludeProps = {"m_Script", "localizationKey"};

        private int _selectedKeyIndex;

        private string[] _availableKeys = new string[0];
        
        private SerializedProperty _localizationKey;

        private void OnEnable()
        {
            _localizationKey = serializedObject.FindProperty("localizationKey");

            _selectedKeyIndex = SimpleLocalizationWindow.CurrentKeys.Keys.IndexOf(_localizationKey.stringValue);
            if(SimpleLocalizationWindow.CurrentKeys != null)
                _availableKeys = SimpleLocalizationWindow.CurrentKeys.Keys.ToArray();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            DrawCustomInspector();
            
            DrawPropertiesExcluding(serializedObject, _expcludeProps);
            serializedObject.ApplyModifiedProperties();
        }

        private void OnSceneGUI()
        {
            if (GUI.Button(new Rect( 5, 5, 60, 30), "+ Node"))
                Debug.Log("+ Node");
        }

        private void DrawCustomInspector()
        {

            if (SimpleLocalizationWindow.CurrentKeys != null)
            {
                _selectedKeyIndex = EditorGUILayout.Popup("Key", _selectedKeyIndex, _availableKeys);
                
                if (GUI.changed)
                {
                    _localizationKey.stringValue = _availableKeys[_selectedKeyIndex];
                }
                
                if (GUILayout.Button("Edit Keys file"))
                {
                    Selection.activeObject = SimpleLocalizationWindow.CurrentKeys;
                }
                
                if (!SimpleLocalizationWindow.CurrentKeys.Keys.Contains(_localizationKey.stringValue))
                {
                    EditorGUILayout.HelpBox(string.Format("Key '{0}' not found in Keys file.\nCheck your Keys files in '{1}'", _localizationKey.stringValue, SimpleLocalizationWindow.KeysFilePath), MessageType.Warning);
                }
            }
            else
            {
                EditorGUILayout.PropertyField(_localizationKey);

                if (GUI.changed)
                {
                    _localizationKey.stringValue = _localizationKey.stringValue.Trim();
                }
                EditorGUILayout.HelpBox("Warning, keys file not found. Key validation is disabled.", MessageType.Warning);
            } 
            
            EditorWindowHelper.DrawUILine(Color.grey);
        }
    }
}