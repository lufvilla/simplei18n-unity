using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Simplei18n
{
    public class SimpleLocalizationConfigWindow : SimpleLocalizationWindow
    {
        [MenuItem("Simple Localization/Configure Localization")]
        public static void DisplayWindow()
        {
            var window = GetWindow<SimpleLocalizationConfigWindow>("Simple Localization - Configuration");
            window.checkConfigExists = false;
        }

        protected override void OnDrawGUI()
        {
            if (CurrentConfig == null)
            {
                DrawNoConfigDetected();
                return;
            }
            
            if (CurrentKeys == null)
            {
                DrawNoKeysDetected();
                return;
            }

            if (!CurrentConfig.Languages.Any())
            {
                EditorWindowHelper.HorizontalLayout(() =>
                {
                    EditorGUILayout.HelpBox("No languages detected!", MessageType.Info);
                });
            }
            
            EditorGUILayout.LabelField("Basic Configuration");
            
            EditorGUI.BeginChangeCheck();

            CurrentConfig.AutodetectLanguage =
                EditorGUILayout.Toggle("Autodetect Language: ", CurrentConfig.AutodetectLanguage);

            if (GUI.changed)
            {
                EditorUtility.SetDirty(CurrentConfig);
                AssetDatabase.SaveAssets();
            }

            EditorGUI.EndChangeCheck();

            DrawLanguages();
            
            EditorWindowHelper.HorizontalLayout(() =>
            {
                if (GUILayout.Button("Create Language!"))
                {
                    EditOrCreateLanguage();
                }
            });
        }

        private void DrawNoConfigDetected()
        {
            EditorGUILayout.HelpBox(string.Format("No config found in path {0}\n\n You want to create one?", ConfigFilePath), MessageType.Info);

            EditorWindowHelper.HorizontalLayout(() =>
            {
                if (GUILayout.Button("Create Config"))
                {
                    AssetDatabase.CreateFolder("Assets/Resources", "Localization");
                    var newConfig = ScriptableObject.CreateInstance<SimpleLanguageConfig>();
                    AssetDatabase.CreateAsset(newConfig, ConfigFilePath);
                    EditorUtility.SetDirty(newConfig);
                    AssetDatabase.SaveAssets();
                }
            });
        }
        
        private void DrawNoKeysDetected()
        {
            EditorGUILayout.HelpBox(string.Format("No keys found in path {0}\n\n You want to create one?", KeysFilePath), MessageType.Info);

            EditorWindowHelper.HorizontalLayout(() =>
            {
                if (GUILayout.Button("Create Keys file"))
                {
                    var newFile = ScriptableObject.CreateInstance<SimpleLanguageKeys>();
                    AssetDatabase.CreateAsset(newFile, KeysFilePath);
                    EditorUtility.SetDirty(newFile);
                    AssetDatabase.SaveAssets();
                }
            });
        }

        private void DrawLanguages()
        {
            if (CurrentConfig.DefaultLanguage != null)
            {
                EditorGUILayout.LabelField("The default language is: ", CurrentConfig.DefaultLanguage.Language.Name);
            }
            else
            {
                EditorGUILayout.HelpBox("Default language not set. Please select one to prevent errors.\nIf no one is selected in the build the first language will be selected as default.", MessageType.Warning);
            }

            EditorGUILayout.LabelField("Available Languages:");
            for (int i = 0; i < CurrentConfig.Languages.Count; i++)
            {
                var index = i;
                EditorWindowHelper.VerticalLayout(() =>
                {
                    EditorGUILayout.LabelField(string.Format("- {0} ({1} cultures)", CurrentConfig.Languages[index].Language.Name, CurrentConfig.Languages[index].Language.Cultures.Count()));
                    
                    var rect = GUILayoutUtility.GetLastRect();
                    if (GUI.Button(new Rect(rect.x + 150, rect.y, 40, rect.height), "Edit"))
                    {
                        EditOrCreateLanguage(CurrentConfig.Languages[index]);
                    }

                    if (CurrentConfig.DefaultLanguage != CurrentConfig.Languages[index])
                    {
                        if (GUI.Button(new Rect(rect.x + 190, rect.y, 90, rect.height), "Set Default"))
                        {
                            SetDefeaultLanguage(CurrentConfig.Languages[index]);
                        }
                    }
                });
            }
        }

        private void SetDefeaultLanguage(SimpleLanguageData languageData)
        {
            CurrentConfig.DefaultLanguage = languageData;
            EditorUtility.SetDirty(CurrentConfig);
            AssetDatabase.SaveAssets();
        }

        private void EditOrCreateLanguage(SimpleLanguageData languageData = null)
        {
            SimpleLanguageEditWindow.ShowWindow(languageData);
        }
    }
}
