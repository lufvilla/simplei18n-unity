using System;
using System.Collections;
using System.Collections.Generic;
using Simplei18n;
using UnityEngine;

public class MyGameServiceManager : MonoBehaviour
{
    private void Awake()
    {
        SimpleLocalizationManager.Initialize();
    }
}
