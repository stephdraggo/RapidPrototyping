using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using Itemising;
using UnityEditor;
using UnityEngine;

public class GlobalVars : MonoBehaviour
{
    public static GlobalVars Instance;
    
#if UNITY_EDITOR
    public GUIStyle[] myStyles;
#endif
    private void OnValidate() {
        Instance = this;

        if (myStyles.Length<2) {
            GUIStyle[] newStyles = new GUIStyle[2];
            newStyles[1]=EditorStyles.foldout;
            myStyles = newStyles;
        }
    }
}

public enum ItemType
{
    Food,
    Scroll,
    Gem,
    Misc,
}

public enum MeasurementType
{
    IntByName,
    IntByUnit,
    FloatingUnits,
}