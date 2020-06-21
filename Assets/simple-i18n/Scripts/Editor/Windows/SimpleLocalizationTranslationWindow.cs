using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Simplei18n;
using UnityEditor;
using UnityEngine;

public class SimpleLocalizationTranslationWindow : SimpleLocalizationWindow
{
    private SimpleLanguageData _currentLanguageData;

    private int newKeyIndex;
    private string newTranslationValue = string.Empty;

    private string _addError = string.Empty;
    
    public static void ShowWindow(SimpleLanguageData languageData)
    {
        var window = GetWindow<SimpleLocalizationTranslationWindow>(string.Format("Editing translations for : {0}", languageData.Language.Name));
        window._currentLanguageData = languageData;
    }

    protected override void OnDrawGUI()
    {
        DrawTranslations();
        
        EditorWindowHelper.DrawUILine(Color.grey);

        DrawAddTranslation();

        EditorWindowHelper.DrawUILine(Color.grey);
        
        if (GUILayout.Button("Save"))
        {
            EditorUtility.SetDirty(_currentLanguageData);
            AssetDatabase.SaveAssets();
            Close();
        }
    }

    private void DrawAddTranslation()
    {
        newKeyIndex = EditorGUILayout.Popup("Key: ",newKeyIndex, CurrentKeys.Keys.ToArray());
        
        EditorGUILayout.LabelField("Translation for key:");
        
        EditorStyles.textField.wordWrap = true;
        newTranslationValue = EditorGUILayout.TextArea(newTranslationValue, GUILayout.Height(150));

        if (GUI.changed)
            _addError = string.Empty;

        if (GUILayout.Button("Add"))
        {
            _addError = string.Empty;
            if (string.IsNullOrWhiteSpace(newTranslationValue))
            {
                _addError += "Translation text cannot be empty.";
                return;
            }

            if (_currentLanguageData.Language.RawTranslations.All(x => x.Key != CurrentKeys.Keys[newKeyIndex]))
            {
                _currentLanguageData.Language.RawTranslations.Add(new Language.Translations(CurrentKeys.Keys[newKeyIndex], newTranslationValue));
                newTranslationValue = string.Empty;
            }
            else
            {
                _addError += "Key already added to this language";
                return;
            }
        }

        if (!string.IsNullOrEmpty(_addError))
        {
            EditorGUILayout.HelpBox(_addError, MessageType.Error);
        }
    }

    private void DrawTranslations()
    {
        EditorGUILayout.LabelField(string.Format("Translations for '{0}'", _currentLanguageData.Language.Name));

        for (int i = _currentLanguageData.Language.RawTranslations.Count - 1; i >= 0; i--)
        {
            DrawTranslation(_currentLanguageData.Language.RawTranslations[i]);
        }

        if (!_currentLanguageData.Language.RawTranslations.Any())
        {
            EditorGUILayout.HelpBox("No translations to show.\nTry adding a new one!", MessageType.Info);
        }
    }

    private void DrawTranslation(Language.Translations translation)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(translation.Key);
        var hRect = GUILayoutUtility.GetLastRect();
        translation.Value = EditorGUI.TextField(new Rect(hRect.x + 100, hRect.y, hRect.width - 130, hRect.height), translation.Value);
        
        EditorGUILayout.EndHorizontal();
        
        if (GUI.Button(new Rect(hRect.width - 25, hRect.y, 25, hRect.height), "-"))
        {
            _currentLanguageData.Language.RawTranslations.Remove(translation);
        }
    }
}
