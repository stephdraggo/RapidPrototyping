//the following definitions allow and prevent certain functionality in this class
//comment out the definition for each functionality which you:
//----- do NOT want implemented in your system, or
//----- want to use a DIFFERENT system for

#define Sprites
#define Value
#define Measurement

//don't touch these
using System;
using UnityEngine;
using UnityEditor;

namespace Itemising
{
    [CreateAssetMenu(fileName = "new generic item", menuName = "Itemising/Generic Item")]
    public class ItemGeneric : ScriptableObject
    {
        #region Variables

        //-------------Public Properties-------------
        public int Id => id;
        public string Name => name;
        public ItemType Type => type;
        public float WeightSingle => weightSingle;

        //--------------Serialised Protected-------------
        //basics
        [SerializeField]
        protected ItemType type = ItemType.Misc;
        [SerializeField]
        protected new string name = "new item";
        [SerializeField]
        protected int id = 0;
#if Sprites
        //sprites
        [SerializeField]
        protected bool allowAltSprites = false;
        [SerializeField]
        protected Sprite defaultSprite;
        [SerializeField]
        protected Sprite[] altSprites;
#endif
#if Value
        //value
        [SerializeField]
        protected int value = 0;
        [SerializeField]
        protected bool valueFluctuates = false;
        [SerializeField]
        protected Vector2 valueLimits = Vector2.zero;
#endif
#if Measurement
        //measuring
        [SerializeField]
        protected float weightSingle = 1;
        [SerializeField]
        protected MeasurementType measurementType = MeasurementType.CountByVolume;
        [SerializeField]
        protected string unitSingle = "piece", unitMultiple = "pieces";
#endif

        #endregion

        #region Methods

#if Sprites
        /// <summary>
        /// Get alt sprite at index if allowed by item, else get default sprite
        /// </summary>
        /// <param name="index">Optional parameter for getting alternate sprite. Index out of bound will return default sprite.</param>
        /// <returns>Sprite</returns>
        public Sprite GetSprite(int index = -1) {
            if (allowAltSprites && index >= 0 && index < altSprites.Length)
                return altSprites[index];
            return defaultSprite;
        }
#endif
#if Value
        /// <summary>
        /// Get value of item with modifier if allowed
        /// </summary>
        /// <param name="valueModifier">value between -1 and 1</param>
        /// <returns>int value</returns>
        /// <exception cref="ArgumentException">exit if trying to use an unsupported value modifier</exception>
        public int GetValue(float valueModifier = 0) {
            if (!valueFluctuates || valueModifier == 0) return value;

            if (valueModifier < -1 || valueModifier > 1)
                throw new ArgumentException("value modifier must be between -1 and 1, where 0 returns the base value");

            float returnValue = value;
            if (valueModifier < 0) {
                returnValue = (value - valueLimits.x) * valueModifier + value;
            }
            else if (valueModifier > 0) {
                returnValue = (value - valueLimits.y) * valueModifier + value;
            }

            return Mathf.RoundToInt(returnValue);
        }
#endif
#if Measurement
        /// <summary>
        /// Get pretty display text for measuring item. Includes amount, units (if applicable) and item name
        /// </summary>
        /// <param name="amount">how much/many of item</param>
        /// <returns>display-ready string</returns>
        public string GetDisplayMeasure(float amount) {
            string display = "";
            display += amount;
            display += " ";
            switch (measurementType) {
                case MeasurementType.CountByName:
                    display += name;
                    if (amount != 1) display += "s";
                    break;
                case MeasurementType.CountByDiscreteUnit:
                    if (amount != 1) display += unitMultiple;
                    else display += unitSingle;
                    display += " of ";
                    display += name;
                    break;
                case MeasurementType.CountByVolume:
                    display += unitSingle;
                    display += " of ";
                    display += name;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return display;
        }
#endif

        #endregion
    }

    #region Enums
    public enum ItemType
    {
        Food,
        Scroll,
        Gem,
        Misc,
    }

    public enum MeasurementType
    {
        CountByName,
        CountByDiscreteUnit,
        CountByVolume,
    }
    #endregion

#if UNITY_EDITOR

    #region Editor Scripts

    [CustomEditor(typeof(ItemGeneric))]
    [CanEditMultipleObjects]
    public class ItemGenericEditor : Editor
    {
        private SerializedProperty
            pValueFluctuates,
            pAllowAltSprites,
            pMeasurementType,
            pDefaultSprite,
            pUnitMultiple,
            pValueLimits,
            pAltSprites,
            pUnitSingle,
            pWeight,
            pValue,
            pName,
            pType,
            pId;

        private bool unfoldMeasurements, unfoldSprites, unfoldBasic, unfoldValue;
        
        private Vector2 scrollPos;

        private GUIStyle foldoutBold;

        private void OnEnable() {
            foldoutBold = GlobalVars.Instance.myStyles[0] ?? EditorStyles.foldout;
            
            pName = serializedObject.FindProperty("name");
            pType = serializedObject.FindProperty("type");
            pId = serializedObject.FindProperty("id");
#if Sprites
            pAllowAltSprites = serializedObject.FindProperty("allowAltSprites");
            pDefaultSprite = serializedObject.FindProperty("defaultSprite");
            pAltSprites = serializedObject.FindProperty("altSprites");
#endif
#if Value
            pValueFluctuates = serializedObject.FindProperty("valueFluctuates");
            pValueLimits = serializedObject.FindProperty("valueLimits");
            pValue = serializedObject.FindProperty("value");
#endif
#if Measurement
            pMeasurementType = serializedObject.FindProperty("measurementType");
            pUnitMultiple = serializedObject.FindProperty("unitMultiple");
            pUnitSingle = serializedObject.FindProperty("unitSingle");
            pWeight = serializedObject.FindProperty("weightSingle");
#endif
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();

            //basic info
            BasicInfoDisplay(foldoutBold);

            //sprite
            SpriteDisplay(foldoutBold);

            //Value
            ValueDisplay(foldoutBold);

            //Measurement
            MeasurementDisplay(foldoutBold);

            //help
            HelpDisplay();

            serializedObject.ApplyModifiedProperties();
        }

        private void HelpDisplay() {
            string helpText1 =
                "Each boxed out section (except Basic Info) can be removed or changed without affecting any other section.";
            string helpText2 =
                "If you do not want to use a particular functionality in your system, simply remove it by commenting out a single line of code.";
            string helpText3 =
                "To remove parts/functionality from this class, refer to the definitions at the top of the script.";
            EditorGUILayout.HelpBox(helpText1, MessageType.Info);
            EditorGUILayout.HelpBox(helpText2, MessageType.Info);
            EditorGUILayout.HelpBox(helpText3, MessageType.Info);
        }

        private void BasicInfoDisplay(GUIStyle style) {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                unfoldBasic = EditorGUILayout.Foldout(unfoldBasic, "Basic Info", true, style);
                if (unfoldBasic) {
                    EditorGUILayout.PropertyField(pName);
                    EditorGUILayout.PropertyField(pId);
                    EditorGUILayout.PropertyField(pType);
                }
            }
            EditorGUILayout.EndVertical();
        }

        private void SpriteDisplay(GUIStyle style) {
#if Sprites
            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                unfoldSprites = EditorGUILayout.Foldout(unfoldSprites, "Sprites", true, style);
                if (unfoldSprites) {
                    pDefaultSprite.objectReferenceValue = EditorGUILayout.ObjectField(pDefaultSprite.displayName,
                        pDefaultSprite.objectReferenceValue, typeof(Sprite), false);

                    EditorGUILayout.PropertyField(pAllowAltSprites);
                    if (pAllowAltSprites.boolValue) {
                        pAltSprites.arraySize =
                            EditorGUILayout.IntField("Alternate sprites", pAltSprites.arraySize);
                        {
                            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
                            {
                                EditorGUILayout.BeginHorizontal();
                                for (int i = 0; i < pAltSprites.arraySize; i++) {
                                    pAltSprites.GetArrayElementAtIndex(i).objectReferenceValue =
                                        EditorGUILayout.ObjectField("",
                                            pAltSprites.GetArrayElementAtIndex(i).objectReferenceValue, typeof(Sprite),
                                            false, GUILayout.MaxWidth(60));
                                }

                                EditorGUILayout.EndHorizontal();
                            }
                            EditorGUILayout.EndScrollView();
                        }
                    }
                }
            }
            EditorGUILayout.EndVertical();
#endif
        }

        private void ValueDisplay(GUIStyle style) {
#if Value
            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                unfoldValue = EditorGUILayout.Foldout(unfoldValue, "Value", true, style);
                if (unfoldValue) {
                    EditorGUILayout.PropertyField(pValue);
                    EditorGUILayout.PropertyField(pValueFluctuates);
                    if (pValueFluctuates.boolValue) {
                        float x = EditorGUILayout.FloatField("Lower Limit", pValueLimits.vector2Value.x);
                        float y = EditorGUILayout.FloatField("Upper Limit", pValueLimits.vector2Value.y);

                        int value = pValue.intValue; //get value once instead of 4 times
                        if (x > value) {
                            x = value;
                            Debug.LogWarning("Lower limit must be equal to or less than base value.");
                        }

                        if (y < value) {
                            y = value;
                            Debug.LogWarning("Upper limit must be equal to or greater than base value.");
                        }

                        pValueLimits.vector2Value = new Vector2(x, y);
                    }
                }
            }
            EditorGUILayout.EndVertical();
#endif
        }

        private void MeasurementDisplay(GUIStyle style) {
#if Measurement
            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                unfoldMeasurements = EditorGUILayout.Foldout(unfoldMeasurements, "Measure", true, style);
                if (unfoldMeasurements) {
                    EditorGUILayout.PropertyField(pMeasurementType);
                    switch ((MeasurementType) pMeasurementType.enumValueIndex) {
                        case MeasurementType.CountByName:
                            break;

                        case MeasurementType.CountByDiscreteUnit:
                            EditorGUILayout.PropertyField(pWeight);
                            pUnitSingle.stringValue =
                                EditorGUILayout.TextField("Unit of Measurement (Single)", pUnitSingle.stringValue);
                            pUnitMultiple.stringValue =
                                EditorGUILayout.TextField("Unit of Measurement (Multiple)", pUnitMultiple.stringValue);
                            break;

                        case MeasurementType.CountByVolume:
                            EditorGUILayout.PropertyField(pWeight);
                            pUnitSingle.stringValue =
                                EditorGUILayout.TextField("Unit of Measurement", pUnitSingle.stringValue);
                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            EditorGUILayout.EndVertical();
#endif
        }
    }

    #endregion

#endif
    
}