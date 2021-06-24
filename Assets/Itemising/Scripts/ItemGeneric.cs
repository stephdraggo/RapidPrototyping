using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Itemising
{
    [CreateAssetMenu(fileName = "new generic item", menuName = "Itemising/Generic Item")]
    public class ItemGeneric : ScriptableObject
    {
        #region Variables

        //-------------Public Properties-------------
        public ItemPassables Passables => passables;
        public float WeightSingle => weight;
        public float WeightTotal => weight * amount;

        public string DisplayWeight {
            get {
                string display = "";
                display += amount;
                display += " ";
                switch (measurementType) {
                    case MeasurementType.IntByName:
                        display += name;
                        if (amount != 1) display += "s";
                        break;
                    case MeasurementType.IntByUnit:
                        if (amount != 1) display += unitMultiple;
                        else display += unitSingle;
                        break;
                    case MeasurementType.FloatingUnits:
                        display += unitSingle;
                        display += " of ";
                        display += name;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                return display;
            }
        }

        //--------------Serialised Protected-------------
        [SerializeField]
        protected ItemPassables passables;
        [SerializeField]
        protected bool allowAltSprites;
        [SerializeField]
        protected Sprite defaultSprite;
        [SerializeField,Tooltip("Array of alternate sprites available for this item.")]
        protected Sprite[] altSprites;

        [SerializeField]
        protected int goldValue;
        [SerializeField,Tooltip("Weight of a single item.")]
        protected float weight;
        [SerializeField]
        protected float amount;
        [SerializeField]
        protected MeasurementType measurementType;
        [SerializeField]
        protected string unitSingle, unitMultiple;

        #endregion

        #region Methods

        /// <summary>
        /// Get alt sprite at index if allowed by item, else get default sprite
        /// </summary>
        /// <param name="index">Optional parameter for getting alt sprite</param>
        /// <returns>Sprite</returns>
        public Sprite GetSprite(int index = -1) {
            if (allowAltSprites && index >= 0 && index < altSprites.Length)
                return altSprites[index];
            return defaultSprite;
        }

        #endregion
    }

    [Serializable]
    public struct ItemPassables
    {
        public int id;
        public string name;
        public ItemType type;
    }

#if UNITY_EDITOR

    #region Editor Scripts

    [CustomEditor(typeof(ItemGeneric))]
    [CanEditMultipleObjects]
    public class ItemGenericEditor : Editor
    {
        private SerializedProperty pPassables,
            pDefaultSprite,
            pAltSprites,
            pGoldValue,
            pWeight,
            pAmount,
            pMeasureType,
            pUnitSingle,
            pUnitMultiple,
            pAllowAltSprites;

        private bool unfoldWeight, unfoldSprites;
        private Vector2 scrollPos;

        private void OnEnable() {
            pPassables = serializedObject.FindProperty("passables");
            pDefaultSprite = serializedObject.FindProperty("defaultSprite");
            pAltSprites = serializedObject.FindProperty("altSprites");
            pGoldValue = serializedObject.FindProperty("goldValue"); //todo
            pWeight = serializedObject.FindProperty("weight");
            pAmount = serializedObject.FindProperty("amount");
            pMeasureType = serializedObject.FindProperty("measurementType");
            pUnitSingle = serializedObject.FindProperty("unitSingle");
            pUnitMultiple = serializedObject.FindProperty("unitMultiple");
            pAllowAltSprites = serializedObject.FindProperty("allowAltSprites");
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();

            //item id struct
            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                EditorGUILayout.PropertyField(pPassables);
            }
            EditorGUILayout.EndVertical();

            //sprite
            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                unfoldSprites = EditorGUILayout.Foldout(unfoldSprites, "Sprites", true, EditorStyles.foldout);
                if (unfoldSprites) {
                    pDefaultSprite.objectReferenceValue = EditorGUILayout.ObjectField(pDefaultSprite.displayName,
                        pDefaultSprite.objectReferenceValue, typeof(Sprite), false);

                    EditorGUILayout.PropertyField(pAllowAltSprites);
                    if (pAllowAltSprites.boolValue) {
                        pAltSprites.arraySize =
                            EditorGUILayout.IntField("Number of alternate sprites", pAltSprites.arraySize);
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

            //weight
            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                unfoldWeight = EditorGUILayout.Foldout(unfoldWeight, "Weight Info", true, EditorStyles.foldout);
                if (unfoldWeight) {
                    EditorGUILayout.PropertyField(pWeight);
                    EditorGUILayout.PropertyField(pMeasureType);
                    switch ((MeasurementType) pMeasureType.enumValueIndex) {
                        case MeasurementType.IntByName:
                            int amount = (int) pAmount.floatValue;
                            pAmount.floatValue = EditorGUILayout.IntField(pAmount.displayName, amount);
                            break;

                        case MeasurementType.IntByUnit:
                            int amount2 = (int) pAmount.floatValue;
                            pAmount.floatValue = EditorGUILayout.IntField(pAmount.displayName, amount2);
                            pUnitSingle.stringValue =
                                EditorGUILayout.TextField("Unit of Measurement (Single)", pUnitSingle.stringValue);
                            pUnitMultiple.stringValue =
                                EditorGUILayout.TextField("Unit of Measurement (Multiple)", pUnitMultiple.stringValue);
                            break;

                        case MeasurementType.FloatingUnits:
                            EditorGUILayout.PropertyField(pAmount);
                            pUnitSingle.stringValue =
                                EditorGUILayout.TextField("Unit of Measurement", pUnitSingle.stringValue);
                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            EditorGUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
        }
    }

    #endregion

#endif
}