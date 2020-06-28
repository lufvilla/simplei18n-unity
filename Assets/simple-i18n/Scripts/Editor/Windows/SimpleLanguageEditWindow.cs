using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Simplei18n
{
    public class SimpleLanguageEditWindow : SimpleLocalizationWindow
    {
        private bool _isNew = false;
        private SimpleLanguageData _currentLanguageData;
        
        private string _errors = string.Empty;
        private string _cultureErrors = string.Empty;

        private int _selectedCulture;
        
        public static void ShowWindow(SimpleLanguageData languageData = null)
        {
            
            if (languageData == null)
            {
                var window = GetWindow<SimpleLanguageEditWindow>("Creating Language");
                window._isNew = true;
                window._currentLanguageData = ScriptableObject.CreateInstance<SimpleLanguageData>();
                window._currentLanguageData.Language = new Language();
            }
            else
            {
                var window = GetWindow<SimpleLanguageEditWindow>(string.Format("Editing: {0}", languageData.Language.Name));
                window._currentLanguageData = languageData;
            }
        }

        protected override void OnDrawGUI()
        {
            _currentLanguageData.Language.Name = EditorGUILayout.TextField("Language Name:", _currentLanguageData.Language.Name);

            DrawCultures();
            
            EditorWindowHelper.DrawUILine(Color.grey);

            DrawTranslations();
            
            EditorWindowHelper.DrawUILine(Color.grey);
            
            if (GUILayout.Button("Save"))
            {
                if (IsFileValid() && AreCulturesValid())
                {
                    if (_isNew)
                        CreateAsset();
                    else
                        SaveAssetAndClose();
                }
            }
            
            if(!string.IsNullOrEmpty(_errors))
                EditorGUILayout.HelpBox(_errors, MessageType.Error);
        }

        private void DrawTranslations()
        {
            if (_isNew)
            {
                EditorGUILayout.HelpBox("Save the language before start editing translations", MessageType.Info);
            }
            else
            {
                EditorGUILayout.HelpBox(string.Format("Translations: {0}\nCompletion: {1}%", _currentLanguageData.Language.RawTranslations.Count, GetCompletionRate()), MessageType.None);
                
                if (GUILayout.Button("Edit translations"))
                {
                    SimpleLocalizationTranslationWindow.ShowWindow(_currentLanguageData);
                }
            }
        }

        private int GetCompletionRate()
        {
            int completion = 0;

            if (CurrentKeys.Keys.Any())
            {
                completion = (int)(_currentLanguageData.Language.RawTranslations.Count / (float)CurrentKeys.Keys.Count * 100f);
            }

            return completion;
        }

        private void DrawCultures()
        {
            EditorGUILayout.LabelField("Selected Cultures:");

            if (!_currentLanguageData.Language.Cultures.Any())
            {
                EditorGUILayout.HelpBox("Add at least one culture", MessageType.Info);
            }

            for (int i = _currentLanguageData.Language.Cultures.Count - 1; i >= 0; i--)
            {
                var culture = _currentLanguageData.Language.Cultures[i];
                
                EditorWindowHelper.HorizontalLayout(() =>
                {
                    EditorGUILayout.LabelField(culture);
                    var rect = GUILayoutUtility.GetLastRect();
                    if (GUI.Button(new Rect(rect.x + 100, rect.y, 25, rect.height), "-"))
                    {
                        _currentLanguageData.Language.Cultures.Remove(culture);
                    }
                });
            }
            
            EditorGUI.BeginChangeCheck();
            _selectedCulture = EditorGUILayout.Popup("Add Culture:", _selectedCulture, Cultures);

            if (GUI.changed)
            {
                IsCultureAddValid();
            }

            EditorGUI.EndChangeCheck();
            
            if (!string.IsNullOrEmpty(_cultureErrors))
            {
                EditorGUILayout.HelpBox(_cultureErrors, MessageType.Error);
            }
            
            if (GUILayout.Button("Add Culture") && IsCultureAddValid())
            {
                _currentLanguageData.Language.Cultures.Add(Cultures[_selectedCulture]);
            }
        }

        private bool IsCultureAddValid()
        {
            bool isValid = false;
            
            _cultureErrors = string.Empty;
            if (!_currentLanguageData.Language.Cultures.Contains(Cultures[_selectedCulture]))
            {
                var languages = GetLanguagesForCulture(Cultures[_selectedCulture]);
                if (languages.Any())
                {
                    foreach (var language in languages)
                    {
                        _cultureErrors += string.Format("Selected culture is already used in Language: {0}\n", language.Language.Name);
                    }
                }
                else
                {
                    isValid = true;
                }
            }
            else
            {
                _cultureErrors += "Selected culture is already present in this language.\n";
            }

            return isValid;
        }

        private bool AreCulturesValid()
        {
            bool isValid = _currentLanguageData.Language.Cultures.Any();

            _errors += "Select at least 1 culture to save this language\n";
            
            return isValid;
        }

        private bool IsFileValid()
        {
            _errors = string.Empty;
            
            bool nameValid = _currentLanguageData.Language != null
                             && !string.IsNullOrEmpty(_currentLanguageData.Language.Name)
                             && _currentLanguageData.Language.Name.Length > 1;

            if (!nameValid)
                _errors += "Name should have at least 2 letters to be valid.\n";

            return nameValid;
        }

        private void SaveAssetAndClose()
        {
            AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(_currentLanguageData), string.Format("{0}_language.asset", _currentLanguageData.Language.Name));
            EditorUtility.SetDirty(_currentLanguageData);
            EditorUtility.SetDirty(CurrentConfig);
            AssetDatabase.SaveAssets();
            
            Close();
        }

        private void CreateAsset()
        {
            if(!AssetDatabase.IsValidFolder(Path.Combine(LOCALIZATION_PATH, LANGUAGES_FOLDER)))
                AssetDatabase.CreateFolder(LOCALIZATION_PATH, LANGUAGES_FOLDER);
            AssetDatabase.CreateAsset(_currentLanguageData,  Path.Combine(LanguagesPath, string.Format("{0}_language.asset", _currentLanguageData.Language.Name)));
            CurrentConfig.Languages.Add(_currentLanguageData);
            
            SaveAssetAndClose();
        }
    }
}