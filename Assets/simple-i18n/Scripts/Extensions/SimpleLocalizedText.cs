using UnityEngine;
using UnityEngine.UI;

namespace Simplei18n
{
    public class SimpleLocalizedText : Text
    {
        [SerializeField]
        private string localizationKey;
        
        public void SetLocalizationKey(string key, bool refresh = true)
        {
            localizationKey = key;

            if (refresh)
                RefreshLocalization();
        }

        public void RefreshLocalization()
        {
            if(Application.isPlaying)
                text = SimpleLocalizationManager.GetTranslationForKey(localizationKey);
        }
        
        protected override void Awake()
        {
            base.Awake();
            RefreshLocalization();
        }
    }
}
