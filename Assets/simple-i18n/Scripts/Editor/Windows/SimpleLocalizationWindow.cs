using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEditor;

namespace Simplei18n
{
    public abstract class SimpleLocalizationWindow : EditorWindow
    {
    #if SIMPLEI18NDEBUG
        // Debug variables
        private bool _showDebug;
    #endif
        
        protected const string LOCALIZATION_PATH = "Assets/Resources/Localization";
        protected const string LANGUAGES_FOLDER = "Languages";
        protected const string LOCALIZATION_CONFIG_NAME = "LocalizationConfig";
        protected const string LOCALIZATION_KEYS_NAME = "LocalizationKeys";

        public static string LanguagesPath => Path.Combine(LOCALIZATION_PATH, LANGUAGES_FOLDER);
        public static string ConfigFilePath => Path.Combine(LOCALIZATION_PATH, string.Format("{0}.asset", LOCALIZATION_CONFIG_NAME));
        
        public static string KeysFilePath => Path.Combine(LOCALIZATION_PATH, string.Format("{0}.asset", LOCALIZATION_KEYS_NAME));

        private static SimpleLanguageConfig _currentConfig;

        public static SimpleLanguageConfig CurrentConfig
        {
            get
            {
                if (_currentConfig != null)
                    return _currentConfig;
                
                _currentConfig = AssetDatabase.LoadAssetAtPath<SimpleLanguageConfig>(ConfigFilePath);

                return _currentConfig;
            }
        }
        
        private static SimpleLanguageKeys _currentKeys;

        public static SimpleLanguageKeys CurrentKeys
        {
            get
            {
                if (_currentKeys != null)
                    return _currentKeys;
                
                _currentKeys = AssetDatabase.LoadAssetAtPath<SimpleLanguageKeys>(KeysFilePath);

                return _currentKeys;
            }
        }

        private static string[] _cultures;
        protected static string[] Cultures
        {
            get
            {
                if (_cultures != null)
                    return _cultures;
                
                _cultures = CultureInfo
                    .GetCultures(CultureTypes.UserCustomCulture | CultureTypes.SpecificCultures)
                    .Select(x => x.Name)
                    .ToArray();
                
                return _cultures;
            }
        }
        
        protected static IEnumerable<SimpleLanguageData> GetLanguagesForCulture(string culture)
        {
            return _currentConfig.Languages.Where(x => x.Language.Cultures.Contains(culture)).ToList();
        }

        protected bool checkConfigExists = true;
        

        protected virtual void OnGUI()
        {
            DrawDebug();
            
            if (checkConfigExists && CurrentConfig == null)
            {
                EditorGUILayout.HelpBox("Something went wrong. Close this window and open it again.", MessageType.Error);
                return;
            }
            
            OnDrawGUI();
        }

        protected abstract void OnDrawGUI();

        private void DrawDebug()
        {
    #if SIMPLEI18NDEBUG
            _showDebug = EditorGUILayout.Toggle("Show Debug? ", _showDebug);
            if (_showDebug)
            {
                EditorWindowHelper.DebugShowWindowBounds(position);
            }
    #endif
        }
        
        private void OnSelectionChange()
        {
            Repaint();
        }
    }
}
