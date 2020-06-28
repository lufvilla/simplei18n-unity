using UnityEditor;
using UnityEngine;

namespace Simplei18n
{
    [CustomEditor(typeof(SimpleLanguageConfig))]
    public class SimpleLanguageConfigEditor : Editor
    {
        private SimpleLanguageConfig _config;
        private void OnEnable()
        {
            _config = (SimpleLanguageConfig) target;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Simple localization Configuration");
            
            if(_config.AutodetectLanguage)
                EditorGUILayout.HelpBox("Autodetect language is enabled.", MessageType.Info);
            else
                EditorGUILayout.HelpBox("Autodetect language is disabled.\nThis will use the default language unless you change it in runtime.", MessageType.Warning);
            
            if(_config.DefaultLanguage != null)
                EditorGUILayout.HelpBox(string.Format("Default language is {0}", _config.DefaultLanguage.Language.Name), MessageType.Info);
            else
                EditorGUILayout.HelpBox("Setup a default language to prevent errors at runtime.", MessageType.Warning);
            
            EditorGUILayout.HelpBox(string.Format("You have {0} languages configured", _config.Languages.Count), MessageType.Info);
            
            EditorWindowHelper.DrawUILine(Color.grey);

            if (SimpleLocalizationWindow.CurrentConfig != _config)
            {
                EditorGUILayout.HelpBox("This config isn't the Default config. You're unable to edit external configurations.", MessageType.Warning);
                return;
            }

            if (GUILayout.Button("Open Config Editor"))
            {
                SimpleLocalizationConfigWindow.DisplayWindow();
            }
        }
    }
}