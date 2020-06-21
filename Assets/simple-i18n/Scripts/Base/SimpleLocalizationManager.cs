using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Simplei18n.Utils;
using UnityEngine;

namespace Simplei18n
{
    public class SimpleLocalizationManager: SingletonBehaviour<SimpleLocalizationManager>
    {
        public static event Action OnLocalizationUpdated = delegate {};
        public static Language Language => Instance._currentLanguage;

        public static void SetLanguage(string culture, bool useDefault = true)
        {
            if (!IsInitialized)
            {
                OnLog(LogType.Error,"Can't find culture ({0}). Localization not initialized.", culture);
                return;
            }

            Instance.SetLanguageForCulture(culture, useDefault);
        }
        
        public static string GetTranslationForKey(string key)
        {
            if (!IsInitialized)
            {
                OnLog(LogType.Error,"Can't find key ({0}). Localization not initialized.", key);
                return key;
            }

            if (Instance._currentLanguage.TranslationsForKeys.TryGetValue(key, out string translation))
            {
                return translation;
            }

            return key;
        }

        public static string GetTranslationForKey(string key, params object[] args)
        {
            return string.Format(GetTranslationForKey(key), args);
        }

        public static void Initialize()
        {
            Instance.OnInitialize();
        }

        public static bool IsInitialized => Instance._config != null && Instance._currentLanguage != null;

        private Language _currentLanguage;
        private SimpleLanguageConfig _config;

        private enum LogType
        {
            Debug,
            Warning,
            Error,
            None
        }
        
        private static void OnLog(LogType type,string text, params object[] args)
        {
            string logText = string.Format("[Simplei18n] {0}: {1}", type, string.Format(text, args));

            switch (type)
            {
                case LogType.Error:
                    Debug.LogError(logText);
                    break;
                case LogType.Warning:
                    Debug.LogWarning(logText);
                    break;
                default:
                    Debug.Log(logText);
                    break;
            }
        }

        private void SetLanguageForCulture(string culture, bool useDefault = true)
        {
            Language cultureLanguage = GetLanguageForCulture(culture);

            if (cultureLanguage == null)
            {
                if(useDefault)
                {
                    cultureLanguage = _config.DefaultLanguage.Language;
                    OnLog(LogType.Warning,"No language detected for culture {0}, using default language: {1}", culture, _currentLanguage.Name);
                }
                else
                {
                    OnLog(LogType.Error, "No language for culture: {0}", culture);
                    return;
                }
            }
            
            SetLanguage(cultureLanguage);
        }

        private void SetLanguage(Language language)
        {
            if (language == null)
            {
                OnLog(LogType.Error, "The given language is null.");
                return;
            }

            if (language == _currentLanguage)
            {
                OnLog(LogType.Warning, "Setting the same language again ({0}). Omitting...", language.Name);
                return;
            }
            
            OnLog(LogType.Debug,"Setting language: {0}", language.Name);
            
            _currentLanguage = language;
            _currentLanguage.GenerateCache();

            OnLocalizationUpdated?.Invoke();
        }

        private Language GetLanguageForCulture(string culture)
        {
            if (string.IsNullOrEmpty(culture))
            {
                OnLog(LogType.Error,"Error setting language, the given culture is empty");
                return null;
            }
            
            if (_currentLanguage != null && _currentLanguage.Cultures.Contains(culture))
            {
                return _currentLanguage;
            }

            return _config.Languages
                .Where(x => x.Language.Cultures.Contains(culture))
                .Select(x=> x.Language)
                .FirstOrDefault();
        }
        
        public static string GetSystemCulture()
        {
            return CultureInfo.InstalledUICulture.Name;
        }

        public static List<Language> GetAvailableLanguages
        {
            get
            {
                return Instance._config.Languages.Select(x => Language).ToList();
            }
        }

        private void OnInitialize()
        {
            OnLog(LogType.Debug,"Initializing...");
            LoadConfig("Localization/LocalizationConfig");
        }

        private void LoadConfig(string path)
        {
            OnLog(LogType.Debug,"Loading configuration...");
            _config = Resources.Load<SimpleLanguageConfig>(path);

            if (_config == null)
            {
                OnLog(LogType.Error,"Config file not found at Path: Resources/{0}", path);
                return;
            }
            
            ParseConfig();
        }

        private void ParseConfig()
        {
            OnLog(LogType.Debug,"Config loaded! Parsing...");

            var defaultLanguage = _config.DefaultLanguage;

            if (defaultLanguage == null)
            {
                if (_config.Languages.Any())
                {
                    defaultLanguage = _config.Languages.FirstOrDefault(x => x != null);
                    
                    if(defaultLanguage != null)
                        OnLog(LogType.Warning, "Default language was empty, using first language in list: {0}", defaultLanguage.Language.Name);
                    else
                        OnLog(LogType.Error, "Setting defaultLanguage was unsuccessful. Default language is empty.");
                }
                else
                {
                    OnLog(LogType.Warning, "No languages detected. Localization service will not initialize.");
                    return;
                }
            }

            if (_config.AutodetectLanguage)
            {
                string detectedCulture = GetSystemCulture();

                SetLanguageForCulture(detectedCulture);
            }
            else if(defaultLanguage != null)
            {
                SetLanguage(defaultLanguage.Language);
            }
            else
            {
                OnLog(LogType.Warning, "No languages to initialize. Localization service will not initialize.");
            }
        }
    }
}

