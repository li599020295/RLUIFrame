using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public static class ExtensionMethod
{ 
    public static bool IsEquals(this float a,float b)
    {
        bool areEqual = Math.Abs(a - b) < 1e-9;
        return areEqual;
    }

    public static void AddListener(this Button btn,UnityAction call,bool removeAll = true)
    {
        if(removeAll)
            btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(call);
    }

    public static void RemoveListener(this Button btn, UnityAction call)
    { 
        btn.onClick.RemoveListener(call);
    }
}
