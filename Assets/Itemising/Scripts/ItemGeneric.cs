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
        public ItemPassables Passables => passables;
        public Sprite DefaultSprite => defaultSprite;
        public float WeightSingle => weight;
        public float WeightTotal => weight * amount;

        public string DisplayWeight
        {
            get
            {
                string display = "";
                display += amount;
                display += " ";
                switch (measurementType)
                {
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

        [SerializeField] protected ItemPassables passables;
        [SerializeField] protected Sprite defaultSprite;
        [SerializeField] protected Sprite[] altSprites;

        [SerializeField] protected int goldValue;
        [SerializeField] protected float weight;
        [SerializeField] protected float amount;
        [SerializeField] protected MeasurementType measurementType;
        [SerializeField] protected string unitSingle, unitMultiple;
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
            pUnitMultiple;

        private bool unfoldWeight, unfoldSprites, altSpritesEnabled;
        private Vector2 scrollPos;

        private void OnEnable()
        {
            pPassables = serializedObject.FindProperty("passables");
            pDefaultSprite = serializedObject.FindProperty("defaultSprite");
            pAltSprites = serializedObject.FindProperty("altSprites");
            pGoldValue = serializedObject.FindProperty("goldValue"); //todo
            pWeight = serializedObject.FindProperty("weight");
            pAmount = serializedObject.FindProperty("amount");
            pMeasureType = serializedObject.FindProperty("measurementType");
            pUnitSingle = serializedObject.FindProperty("unitSingle");
            pUnitMultiple = serializedObject.FindProperty("unitMultiple");
        }

        public override void OnInspectorGUI()
        {
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
                if (unfoldSprites)
                {
                    pDefaultSprite.objectReferenceValue = EditorGUILayout.ObjectField(pDefaultSprite.displayName,
                        pDefaultSprite.objectReferenceValue, typeof(Sprite), false);

                    altSpritesEnabled = EditorGUILayout.Toggle("Allow alternate sprites", altSpritesEnabled);
                    if (altSpritesEnabled)
                    {
                        pAltSprites.arraySize =
                            EditorGUILayout.IntField("Number of alternate sprites", pAltSprites.arraySize);
                        {
                            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
                            {
                                EditorGUILayout.BeginHorizontal();
                                for (int i = 0; i < pAltSprites.arraySize; i++)
                                {
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
                if (unfoldWeight)
                {
                    EditorGUILayout.PropertyField(pWeight);
                    EditorGUILayout.PropertyField(pMeasureType);
                    switch ((MeasurementType) pMeasureType.enumValueIndex)
                    {
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