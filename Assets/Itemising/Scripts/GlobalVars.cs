using Itemising;
using UnityEngine;

public class GlobalVars : MonoBehaviour
{
    public static GlobalVars Instance;

    public ItemManager itemManager;

#if UNITY_EDITOR
    public GUIStyle[] myStyles;
#endif

    private void OnValidate() {
        Instance = this;
    }

    private void Awake() {
        Instance = this;
    }
}

