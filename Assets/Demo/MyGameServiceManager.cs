using Simplei18n;
using UnityEngine;

public class MyGameServiceManager : MonoBehaviour
{
    public void LoadCulture(string culture)
    {
        SimpleLocalizationManager.SetLanguage(culture);
    }
    
    public void LoadSystemCulture()
    {
        SimpleLocalizationManager.SetLanguage(SimpleLocalizationManager.GetSystemCulture());
    }

    private void Awake()
    {
        SimpleLocalizationManager.Initialize();
    }
}
