using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace BigBoi.PlayerController
{
    [AddComponentMenu("BigBoi/Player Controllers/Required Static Info")]
    public class StaticControllerInfo : MonoBehaviour
    {
        #region Variables

        //-----------------Public--------------------------------
        public static StaticControllerInfo Instance;

        public Dictionary<string, KeyBind> keyBinds = new Dictionary<string, KeyBind>();

        public KeyBindDefaults defaults;

        public LayerMask walkable;

        #endregion

        #region Unity Methods (OnValidate - Awake)

        /// <summary>
        /// Set class instance.
        /// </summary>
        private void OnValidate() {
            //set instance
            if (Instance == null) {
                Instance = this;
            }
            else if (Instance != this) {
                Destroy(gameObject);
                return;
            }

            
        }

        /// <summary>
        /// Call OnValidate once more before Start.
        /// </summary>
        private void Awake() {
            //validate again
            OnValidate();

            
        }

        #endregion
    }

    public static class Generics
    {
        #region Variables

        private static Dictionary<string, KeyBind> KeyBinds {
            get => StaticControllerInfo.Instance.keyBinds;
            set => StaticControllerInfo.Instance.keyBinds = value;
        }

        #endregion

        #region Static Methods (CompileDictionary - AddToDictionary - EmptyDictionary)

        /// <summary>
        /// Checks dictionary for keys and adds them if not found.
        /// </summary>
        /// <param name="keyBinds">array of keybinds being sent to dictionary</param>
        public static void CompileDictionary(this KeyBind[] keyBinds) {
            foreach (KeyBind keyBind in keyBinds) {
                if (!KeyBinds.ContainsKey(keyBind.name)) {
                    keyBind.AddToDictionary();
                }
            }
        }

        /// <summary>
        /// Add a single entry to the dictionary, does not check if value
        /// </summary>
        /// <param name="keyBind">keybind to add</param>
        /// <param name="check">should the method spend time checking if this keybind has already been added to the dictionary?</param>
        public static void AddToDictionary(this KeyBind keyBind, bool check = false) {
            if (check) {
                if (!KeyBinds.ContainsKey(keyBind.name)) {
                    KeyBinds.Add(keyBind.name, keyBind);
                }
            }
            else {
                KeyBinds.Add(keyBind.name, keyBind);
            }
        }

        /// <summary>
        /// Clear all keybinds from dictionary.
        /// </summary>
        public static void EmptyDictionary() {
            KeyBinds.Clear();
            KeyBinds = new Dictionary<string, KeyBind>();
        }

        #endregion
    }

    [Serializable]
    public class KeyBind
    {
        #region Variables (Constructor)

        [Tooltip("The string used when adding this keybind to the dictionary.")]
        public string name;
        public KeyCode key;
        private KeyBindDefaults defaults;

        public KeyBind(string name, KeyCode key) {
            this.name = name;
            this.key = key;
        }

        #endregion

        #region Methods (OnValidateName - OnDuplicateNameFound - OnDuplicateKeyFound)

        /// <summary>
        /// Check that this keybind has a name, else assign default name
        /// </summary>
        public void OnValidateName() {
            defaults ??= StaticControllerInfo.Instance.defaults;
            if (string.IsNullOrEmpty(name)) {
                name = defaults.defaultKeyName;
            }
        }

        /// <summary>
        /// Increments name with passed int
        /// </summary>
        public void OnDuplicateNameFound() {
            defaults ??= StaticControllerInfo.Instance.defaults;
            name += defaults.GetIncrement();
        }

        /// <summary>
        /// Logs a warning, duplicate keycodes should not break game
        /// </summary>
        public void OnDuplicateKeyFound() {
            Debug.LogWarning($"{name} keybind set shares the keycode {key} with a previous keybind.");
        }

        #endregion
    }

    [Serializable]
    public class KeyBindDefaults
    {
        #region Variables

        [Tooltip("Name given to keys with empty strings for names.")]
        public string defaultKeyName = "key";
        [SerializeField, Min(0),
         Tooltip("Do not change this value unless you want to start counting duplicate keys at a value other than 1.")]
        private int doublesIncrement = 1;

        #endregion

        #region Methods (GetIncrement)

        /// <summary>
        /// Gets increment int as string and increases for next use.
        /// </summary>
        public string GetIncrement() {
            string increment = doublesIncrement.ToString();
            doublesIncrement++;
            return increment;
        }

        #endregion
    }

    #region Editor

#if UNITY_EDITOR

    [CanEditMultipleObjects]
    [CustomEditor(typeof(StaticControllerInfo))]
    public class StaticControllerInfoEditor : Editor
    {
        private SerializedProperty pDefaults,pWalkable;

        private void OnEnable() {
            pDefaults = serializedObject.FindProperty("defaults");
            pWalkable = serializedObject.FindProperty("walkable");
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();

            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                EditorGUILayout.PropertyField(pDefaults);
                EditorGUILayout.PropertyField(pWalkable);
            }
            EditorGUILayout.EndVertical();


            serializedObject.ApplyModifiedProperties();
        }
    }

    [CustomPropertyDrawer(typeof(KeyBindDefaults))]
    public class KeyBindDefaultsEditor : PropertyDrawer
    {
        //number of field spaces taken by the fields total, set manually here
        //if 3 fields but one takes 2 lines of space, spacesCount should be 1+1+2=4
        private static int spacesCount;

        private Rect localPos;
        private int lineCounter, fieldCounter;

        //one singleLineHeight for each line spaces a given field should take
        //add lineSpacing and previous field's height to next y coordinate
        private float singleLineHeight = EditorGUIUtility.singleLineHeight,
            lineSpacing = EditorGUIUtility.standardVerticalSpacing;

        GUIContent helpBox =
            new GUIContent(
                "Do not change the increment unless you want to start counting from a number other than 1.",
                EditorGUIUtility.IconContent("console.infoicon").image);

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            SerializedProperty pDefaultKeyName = property.FindPropertyRelative("defaultKeyName");
            SerializedProperty pDoublesIncrement = property.FindPropertyRelative("doublesIncrement");

            EditorGUI.BeginProperty(position, label, property);
            {
                lineCounter = 0;
                fieldCounter = 0;
                localPos = position;

                RenderPropertyField(pDefaultKeyName);
                RenderPropertyField(pDoublesIncrement);
                RenderLabelField(helpBox, EditorStyles.helpBox, 2);
            }
            EditorGUI.EndProperty();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="property"></param>
        /// <param name="size">standard line count this field should take up, default 1</param>
        private void RenderPropertyField(SerializedProperty property, int size = 1) {
            IncrementRect(size);
            EditorGUI.PropertyField(localPos, property);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        /// <param name="style"></param>
        /// <param name="size">standard line count this field should take up, default 1</param>
        private void RenderLabelField(GUIContent label, GUIStyle style, int size = 1) {
            IncrementRect(size);
            EditorGUI.LabelField(localPos, label, style);
        }

        private void IncrementRect(int size) {
            fieldCounter++;
            localPos.y = singleLineHeight * lineCounter + lineSpacing * (fieldCounter + 2);
            localPos.height = singleLineHeight * size;
            lineCounter += size;
            if (spacesCount < lineCounter) {
                spacesCount = lineCounter;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return base.GetPropertyHeight(property, label) * spacesCount + lineSpacing;
        }
    }

#endif

    #endregion
}