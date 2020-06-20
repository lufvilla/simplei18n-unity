using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SimpleLocalization/Language Keys", fileName = "New Keys")]
public class SimpleLanguageKeys : ScriptableObject
{
    public List<string> Keys = new List<string>();
}
