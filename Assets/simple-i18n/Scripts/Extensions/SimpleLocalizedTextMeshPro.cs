using TMPro;
using UnityEngine;

namespace Simplei18n
{
    public class SimpleLocalizedTextMeshPro : TextMeshPro
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

        protected override void OnDestroy()
        {
            SimpleLocalizationManager.OnLocalizationUpdated -= RefreshLocalization;
            base.OnDestroy();
        }


        protected override void Start()
        {
            base.Start();
            RefreshLocalization();
            SimpleLocalizationManager.OnLocalizationUpdated += RefreshLocalization;
        }
    }

}