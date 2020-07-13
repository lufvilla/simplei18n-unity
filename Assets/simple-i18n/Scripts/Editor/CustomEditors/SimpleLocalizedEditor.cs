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

        private Transform _transform;
        private Vector3 _screenPos;
        private string _errorText;

        private void OnEnable()
        {
            _localizationKey = serializedObject.FindProperty("localizationKey");

            _transform = ((MonoBehaviour)target).transform;

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

        protected void OnSceneGUI()
        {
            _screenPos = HandleUtility.WorldToGUIPoint(_transform.position);

            DrawHandles();
        }

        private void DrawHandles()
        {
            if (!string.IsNullOrEmpty(_errorText))
            {
                Handles.BeginGUI();
                if (GUI.Button(new Rect(_screenPos.x - 75, _screenPos.y - 70, 150, 40), string.Format("-- Translation Error --\n{0}", _errorText)))
                        Debug.LogWarning("Check the Simple Localized Text component and fix the error!");
                Handles.EndGUI();
            }
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
                    _errorText = "Key not found or empty";
                    EditorGUILayout.HelpBox(string.Format("Key '{0}' not found in Keys file.\nCheck your Keys files in '{1}'", _localizationKey.stringValue, SimpleLocalizationWindow.KeysFilePath), MessageType.Warning);
                }
                else
                {
                    _errorText = string.Empty;
                }
            }
            else
            {
                EditorGUILayout.PropertyField(_localizationKey);

                if (GUI.changed)
                {
                    _localizationKey.stringValue = _localizationKey.stringValue.Trim();
                }
                _errorText = "Keys file not found";
                EditorGUILayout.HelpBox("Warning, keys file not found. Key validation is disabled.", MessageType.Warning);
            } 
            
            EditorWindowHelper.DrawUILine(Color.grey);
        }
    }
}