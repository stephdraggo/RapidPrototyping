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
        protected ItemType type;
        [SerializeField]
        protected new string name;
        [SerializeField]
        protected int id;

        //sprites
        [SerializeField]
        protected bool allowAltSprites;
        [SerializeField]
        protected Sprite defaultSprite;
        [SerializeField]
        protected Sprite[] altSprites;

        //value
        [SerializeField]
        protected int value;
        [SerializeField]
        protected bool valueFluctuates;
        [SerializeField]
        protected Vector2 valueLimits;

        //measuring
        [SerializeField]
        protected float weightSingle;
        [SerializeField]
        protected MeasurementType measurementType;
        [SerializeField]
        protected string unitSingle, unitMultiple;

        #endregion

        #region Methods

        /// <summary>
        /// Get alt sprite at index if allowed by item, else get default sprite
        /// </summary>
        /// <param name="index">Optional parameter for getting alternate sprite</param>
        /// <returns>Sprite</returns>
        public Sprite GetSprite(int index = -1) {
            if (allowAltSprites && index >= 0 && index < altSprites.Length)
                return altSprites[index];
            return defaultSprite;
        }

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

        /// <summary>
        /// Get pretty display text for an item. Includes amount, units (if applicable) and item name
        /// </summary>
        /// <param name="amount">how much/many of item</param>
        /// <returns>display-ready string</returns>
        public string GetDisplayWeightOrAmount(float amount) {
            string display = "";
            display += amount;
            display += " ";
            switch (measurementType) {
                case MeasurementType.CountByName:
                    display += name;
                    if (amount != 1) display += "s";
                    break;
                case MeasurementType.CountByUnit:
                    if (amount != 1) display += unitMultiple;
                    else display += unitSingle;
                    display += " of ";
                    display += name;
                    break;
                case MeasurementType.ChunkByUnits:
                    display += unitSingle;
                    display += " of ";
                    display += name;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return display;
        }

        #endregion
    }


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

        private void OnEnable() {
            pValueFluctuates = serializedObject.FindProperty("valueFluctuates");
            pAllowAltSprites = serializedObject.FindProperty("allowAltSprites");
            pMeasurementType = serializedObject.FindProperty("measurementType");
            pDefaultSprite = serializedObject.FindProperty("defaultSprite");
            pUnitMultiple = serializedObject.FindProperty("unitMultiple");
            pValueLimits = serializedObject.FindProperty("valueLimits");
            pAltSprites = serializedObject.FindProperty("altSprites");
            pUnitSingle = serializedObject.FindProperty("unitSingle");
            pWeight = serializedObject.FindProperty("weightSingle");
            pValue = serializedObject.FindProperty("value");
            pName = serializedObject.FindProperty("name");
            pType = serializedObject.FindProperty("type");
            pId = serializedObject.FindProperty("id");
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();

            GUIStyle foldoutBold = GlobalVars.Instance.myStyles[0];

            //basic info
            BasicInfoDisplay(foldoutBold);

            //sprite
            SpriteDisplay(foldoutBold);

            //Value
            ValueDisplay(foldoutBold);

            //Measurement
            MeasurementDisplay(foldoutBold);

            serializedObject.ApplyModifiedProperties();
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
        }

        private void ValueDisplay(GUIStyle style) {
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
        }

        private void MeasurementDisplay(GUIStyle style) {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                unfoldMeasurements = EditorGUILayout.Foldout(unfoldMeasurements, "Measure", true, style);
                if (unfoldMeasurements) {
                    EditorGUILayout.PropertyField(pMeasurementType);
                    switch ((MeasurementType) pMeasurementType.enumValueIndex) {
                        case MeasurementType.CountByName:
                            break;

                        case MeasurementType.CountByUnit:
                            EditorGUILayout.PropertyField(pWeight);
                            pUnitSingle.stringValue =
                                EditorGUILayout.TextField("Unit of Measurement (Single)", pUnitSingle.stringValue);
                            pUnitMultiple.stringValue =
                                EditorGUILayout.TextField("Unit of Measurement (Multiple)", pUnitMultiple.stringValue);
                            break;

                        case MeasurementType.ChunkByUnits:
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
        }
    }

    #endregion

#endif
}