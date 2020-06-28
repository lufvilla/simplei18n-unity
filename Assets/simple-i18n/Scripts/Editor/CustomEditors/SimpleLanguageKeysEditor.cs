using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Simplei18n
{
    [CustomEditor(typeof(SimpleLanguageKeys))]
    public class SimpleLanguageKeysEditor : Editor
    {
        private Vector2 _scrollPos = Vector3.zero;

        private SimpleLanguageKeys _keys;

        private string _keyFilter = string.Empty;
        private string _keyToAdd = string.Empty;

        private string _errors = string.Empty;

        private List<string> _filteredKeys = new List<string>();
        
        private void OnEnable()
        {
            _keys = (SimpleLanguageKeys) target;
            
            _filteredKeys = _keys.Keys;
        }
        
        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Localization Keys");
            
            _keyFilter = EditorGUILayout.TextField("Search: ", _keyFilter);
            
            EditorWindowHelper.DrawUILine(Color.gray);
            
            var hRect = EditorGUILayout.BeginHorizontal();
            
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos,false, false, GUILayout.Width(hRect.width), GUILayout.MaxHeight(300));
            
            for (int i = _keys.Keys.Count - 1; i >= 0; i--)
            {
                int realIndex = _keys.Keys.Count - 1 - i;
                
                if(!_keys.Keys[realIndex].Contains(_keyFilter))
                    continue;
                    
                EditorGUILayout.LabelField("");
                Rect rect = GUILayoutUtility.GetLastRect();
                _keys.Keys[realIndex] = GUI.TextField(new Rect(rect.x, rect.y,rect.width - 45,20), _keys.Keys[realIndex]);

                if (GUI.Button(new Rect(rect.width - 40, rect.y, 35, 20), "-"))
                {
                    Undo.RecordObject(_keys, string.Format("Removed key ({0})", _keys.Keys[realIndex]));
                    _keys.Keys.RemoveAt(realIndex);
                    EditorUtility.SetDirty(_keys);
                    AssetDatabase.SaveAssets();
                }
            }
            
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndHorizontal();
            
            EditorWindowHelper.DrawUILine(Color.gray);
            
            EditorGUI.BeginChangeCheck();
            _keyToAdd = EditorGUILayout.TextField("Key To Add: ", _keyToAdd);

            if (GUI.changed)
            {
                _errors = string.Empty;
            }

            EditorGUI.EndChangeCheck();

            if (GUILayout.Button("Add") || EditorWindowHelper.DetectKey(KeyCode.Return) || EditorWindowHelper.DetectKey(KeyCode.KeypadEnter))
            {
                string cleanKey = _keyToAdd.Trim();

                if (!string.IsNullOrWhiteSpace(cleanKey))
                {
                    if (!_keys.Keys.Contains(cleanKey))
                    {
                        Undo.RecordObject(_keys, string.Format("Added key ({0})", cleanKey));
                        _keys.Keys.Add(cleanKey);
                        _keyToAdd = string.Empty;
                        EditorUtility.SetDirty(_keys);
                        AssetDatabase.SaveAssets();
                    }
                    else
                    {
                        _errors = string.Format("Key '{0}' already exist in dictionary", cleanKey);
                    }
                }
                else
                {
                    _errors = "Key cannot be empty";
                }
            }
            
            if(!string.IsNullOrEmpty(_errors))
                EditorGUILayout.HelpBox(_errors, MessageType.Error);
        }
    }
}