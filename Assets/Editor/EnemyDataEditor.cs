using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

//v0.01 / 2026.03.10 / 03:08
[CustomEditor(typeof(EnemyData))]  
public class EnemyDataEditor : Editor
{
    private SerializedProperty enemyNameProp;
    private SerializedProperty enemyDescriptionProp;
    private SerializedProperty maxHealthProp;
    private SerializedProperty baseSpeedProp;

    private SerializedProperty enemySpriteProp;
    private SerializedProperty animatorControllerProp;

    private SerializedProperty thumbNailProp;

    private SerializedProperty patternListProp;

    private List<Type> actionTypes;
    private string[] actionTypeNames;

    private void OnEnable()
    {
        enemyNameProp = serializedObject.FindProperty("enemyName");
        enemyDescriptionProp = serializedObject.FindProperty("enemyDescription");
        maxHealthProp = serializedObject.FindProperty("maxHealth");
        baseSpeedProp = serializedObject.FindProperty("baseSpeed");

        enemySpriteProp = serializedObject.FindProperty("enemySprite");
        animatorControllerProp = serializedObject.FindProperty("animatorController");

        thumbNailProp = serializedObject.FindProperty("thumbNail");

        patternListProp = serializedObject.FindProperty("patternList");

        actionTypes = TypeCache.GetTypesDerivedFrom<PatternActionBase>()
            .Where(t => !t.IsAbstract && !t.IsGenericType)
            .OrderBy(t => t.Name)
            .ToList();

        actionTypeNames = actionTypes.Select(t => t.Name).ToArray();
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultSection();
        EditorGUILayout.Space(8);
        DrawPatternList();

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawDefaultSection()
    {
        EditorGUILayout.LabelField("Enemy Profile", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(enemyNameProp);
        EditorGUILayout.PropertyField(enemyDescriptionProp);
        EditorGUILayout.PropertyField(maxHealthProp);
        EditorGUILayout.PropertyField(baseSpeedProp);
        EditorGUILayout.Space(8);

        EditorGUILayout.LabelField("Visual", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(enemySpriteProp);
        EditorGUILayout.PropertyField(animatorControllerProp);

        EditorGUILayout.LabelField("UI Setting", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(thumbNailProp);
    }

    private void DrawPatternList()
    {
        EditorGUILayout.LabelField("Pattern List", EditorStyles.boldLabel);

        if (patternListProp == null)
        {
            EditorGUILayout.HelpBox("patternList property not found.", MessageType.Error);
            return;
        }

        for (int i = 0; i < patternListProp.arraySize; i++)
        {
            SerializedProperty patternElement = patternListProp.GetArrayElementAtIndex(i);

            SerializedProperty patternNameProp = patternElement.FindPropertyRelative("patternName");
            SerializedProperty patternTypeProp = patternElement.FindPropertyRelative("patternType");
            SerializedProperty intentIconProp = patternElement.FindPropertyRelative("intentIcon");
            SerializedProperty patternActionListProp = patternElement.FindPropertyRelative("patternActionList");

            string patternName = string.IsNullOrEmpty(patternNameProp.stringValue)
                ? $"Pattern {i}"
                : patternNameProp.stringValue;

            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.BeginHorizontal();

            patternElement.isExpanded = EditorGUILayout.Foldout(
                patternElement.isExpanded,
                $"{i}. {patternName}",
                true
            );

            GUI.backgroundColor = new Color(1f, 0.6f, 0.6f);
            if (GUILayout.Button("Delete Pattern", GUILayout.Width(110)))
            {
                DeletePatternAt(i);
                GUI.backgroundColor = Color.white;
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
                break;
            }
            GUI.backgroundColor = Color.white;

            EditorGUILayout.EndHorizontal();

            if (patternElement.isExpanded)
            {
                EditorGUI.indentLevel++;

                EditorGUILayout.PropertyField(patternNameProp);
                EditorGUILayout.PropertyField(patternTypeProp);
                EditorGUILayout.PropertyField(intentIconProp);

                EditorGUILayout.Space(4);
                DrawPatternActionList(patternActionListProp, i);

                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.Space(4);

        if (GUILayout.Button("Add Pattern", GUILayout.Height(24)))
        {
            AddPattern();
        }
    }

    private void DrawPatternActionList(SerializedProperty patternActionListProp, int patternIndex)
    {
        EditorGUILayout.LabelField("Pattern Action List", EditorStyles.boldLabel);

        if (patternActionListProp == null)
        {
            EditorGUILayout.HelpBox("patternActionList property not found.", MessageType.Error);
            return;
        }

        for (int i = 0; i < patternActionListProp.arraySize; i++)
        {
            SerializedProperty actionElement = patternActionListProp.GetArrayElementAtIndex(i);

            EditorGUILayout.BeginVertical("helpbox");
            EditorGUILayout.BeginHorizontal();

            string typeName = GetManagedReferenceTypeName(actionElement);
            actionElement.isExpanded = EditorGUILayout.Foldout(
                actionElement.isExpanded,
                $"Action {i} : {typeName}",
                true
            );

            GUI.backgroundColor = new Color(1f, 0.7f, 0.7f);
            if (GUILayout.Button("Delete", GUILayout.Width(60)))
            {
                DeletePatternActionAt(patternActionListProp, i);
                GUI.backgroundColor = Color.white;
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
                break;
            }
            GUI.backgroundColor = Color.white;

            EditorGUILayout.EndHorizontal();

            if (actionElement.isExpanded)
            {
                if (actionElement.managedReferenceValue == null)
                {
                    EditorGUILayout.HelpBox("No action type assigned.", MessageType.Warning);
                }
                else
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(actionElement, true);
                    EditorGUI.indentLevel--;
                }
            }

            EditorGUILayout.EndVertical();
        }

        Rect buttonRect = GUILayoutUtility.GetRect(0f, 24f, GUILayout.ExpandWidth(true));
        if (GUI.Button(buttonRect, "Add Pattern Action"))
        {
            ShowAddPatternActionMenu(buttonRect, patternActionListProp);
        }
    }

    private void ShowAddPatternActionMenu(Rect buttonRect, SerializedProperty patternActionListProp)
    {
        GenericMenu menu = new GenericMenu();

        if (actionTypes == null || actionTypes.Count == 0)
        {
            menu.AddDisabledItem(new GUIContent("No PatternAction types found"));
        }
        else
        {
            for (int i = 0; i < actionTypes.Count; i++)
            {
                Type type = actionTypes[i];
                menu.AddItem(new GUIContent(type.Name), false, () => AddPatternAction(patternActionListProp, type));
            }
        }

        menu.DropDown(buttonRect);
    }

    private void AddPattern()
    {
        serializedObject.Update();

        int index = patternListProp.arraySize;
        patternListProp.InsertArrayElementAtIndex(index);

        SerializedProperty newPattern = patternListProp.GetArrayElementAtIndex(index);
        newPattern.isExpanded = true;

        SerializedProperty patternNameProp = newPattern.FindPropertyRelative("patternName");
        SerializedProperty patternActionListProp = newPattern.FindPropertyRelative("patternActionList");

        if (patternNameProp != null && string.IsNullOrEmpty(patternNameProp.stringValue))
        {
            patternNameProp.stringValue = $"New Pattern {index}";
        }

        if (patternActionListProp != null && patternActionListProp.isArray)
        {
            while (patternActionListProp.arraySize > 0)
            {
                patternActionListProp.DeleteArrayElementAtIndex(0);
            }
        }

        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(target);
    }

    private void DeletePatternAt(int index)
    {
        serializedObject.Update();
        patternListProp.DeleteArrayElementAtIndex(index);
        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(target);
    }

    private void AddPatternAction(SerializedProperty patternActionListProp, Type type)
    {
        serializedObject.Update();

        int index = patternActionListProp.arraySize;
        patternActionListProp.InsertArrayElementAtIndex(index);

        SerializedProperty newElement = patternActionListProp.GetArrayElementAtIndex(index);
        newElement.managedReferenceValue = Activator.CreateInstance(type);
        newElement.isExpanded = true;

        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(target);
    }

    private void DeletePatternActionAt(SerializedProperty patternActionListProp, int index)
    {
        serializedObject.Update();

        SerializedProperty element = patternActionListProp.GetArrayElementAtIndex(index);

        if (element.managedReferenceValue != null)
        {
            element.managedReferenceValue = null;
        }

        patternActionListProp.DeleteArrayElementAtIndex(index);

        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(target);
    }

    private string GetManagedReferenceTypeName(SerializedProperty property)
    {
        if (property == null || property.managedReferenceValue == null)
            return null;

        return property.managedReferenceValue.GetType().Name;
    }
}
