using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

//v0.02 / 2026.03.08 / 16:28
[CustomEditor(typeof(CardData))]
public class CardDataEditor : Editor
{
    private SerializedProperty idProp;
    private SerializedProperty cardNameProp;
    private SerializedProperty typeProp;
    private SerializedProperty colorProp;
    private SerializedProperty cardTextProp;

    private SerializedProperty cardSpriteProp;
    private SerializedProperty dragIconProp;
    private SerializedProperty cardAnimTriggerProp;

    private SerializedProperty cardTargetProp;
    private SerializedProperty activeConditionListProp;

    private SerializedProperty usableWithoutTargetProp;
    private SerializedProperty banishAfterUsedProp;
    private SerializedProperty cardActionListProp;

    private List<Type> actionTypes;
    private String[] actionTypeNames;

    private void OnEnable()
    {
        idProp = serializedObject.FindProperty("id");
        cardNameProp = serializedObject.FindProperty("cardName");
        typeProp = serializedObject.FindProperty("type");
        colorProp = serializedObject.FindProperty("color");
        cardTextProp = serializedObject.FindProperty("cardText");

        cardSpriteProp = serializedObject.FindProperty("cardSprite");
        dragIconProp = serializedObject.FindProperty("dragIcon");
        cardAnimTriggerProp = serializedObject.FindProperty("cardAnimTrigger");

        cardTargetProp = serializedObject.FindProperty("cardTarget");
        activeConditionListProp = serializedObject.FindProperty("activeConditionList");

        usableWithoutTargetProp = serializedObject.FindProperty("usableWithoutTarget");
        banishAfterUsedProp = serializedObject.FindProperty("banishAfterUsed");
        cardActionListProp = serializedObject.FindProperty("cardActionList");

        actionTypes = TypeCache.GetTypesDerivedFrom<CardActionBase>()
            .Where(t => !t.IsAbstract && !t.IsGenericType)
            .OrderBy(t => t.Name)
            .ToList();

        actionTypeNames = actionTypes.Select(t => t.Name).ToArray();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawDefaultSections();
        EditorGUILayout.Space(8);
        DrawCardActionList();

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawDefaultSections()
    {
        EditorGUILayout.LabelField("Card Profile", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(idProp);
        EditorGUILayout.PropertyField (cardNameProp);
        EditorGUILayout.PropertyField(typeProp);
        EditorGUILayout.PropertyField(colorProp);
        EditorGUILayout.PropertyField(cardTextProp);

        EditorGUILayout.Space(6);
        EditorGUILayout.LabelField("Visual", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(cardSpriteProp);
        EditorGUILayout.PropertyField(dragIconProp);
        EditorGUILayout.PropertyField(cardAnimTriggerProp);

        EditorGUILayout.Space(6);
        EditorGUILayout.LabelField("Active Condition", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(cardTargetProp);
        EditorGUILayout.PropertyField(activeConditionListProp, true);

        EditorGUILayout.Space(6);
        EditorGUILayout.LabelField("Action Settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(usableWithoutTargetProp);
        EditorGUILayout.PropertyField(banishAfterUsedProp);
    }

    private void DrawCardActionList()
    {
        EditorGUILayout.LabelField("Card Action List", EditorStyles.boldLabel);

        if(cardActionListProp == null)
        {
            EditorGUILayout.HelpBox("cardActionList property not found.", MessageType.Error); 
            return;
        }

        for(int i = 0; i< cardActionListProp.arraySize; i++)
        {
            SerializedProperty element = cardActionListProp.GetArrayElementAtIndex(i);

            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.BeginHorizontal();

            string typeName = GetManagedRefernceTypeName(element);
            element.isExpanded = EditorGUILayout.Foldout(
                element.isExpanded,
                $"Element {i} : {typeName}",
                true
            );

            GUI.backgroundColor = new Color(1f, 0.6f, 0.6f);
            if(GUILayout.Button("Delete", GUILayout.Width(60)))
            {
                DeleteElementAt(i);
                GUI.backgroundColor = Color.white;
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
                break;
            }
            GUI.backgroundColor = Color.white;

            EditorGUILayout.EndHorizontal();

            if(element.isExpanded)
            {
                if(element.managedReferenceValue == null)
                {
                    EditorGUILayout.HelpBox("No action type assigned.", MessageType.Warning);
                }
                else
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(element, true);
                    EditorGUI.indentLevel--;
                }
            }
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.Space(4);

        Rect buttonRect = GUILayoutUtility.GetRect(0f, 24f, GUILayout.ExpandWidth(true));
        if(GUI.Button(buttonRect, "Add Action"))
        {
            ShowAddActionMenu(buttonRect);
        }
    }

    private void ShowAddActionMenu(Rect buttonRect)
    {
        GenericMenu menu = new GenericMenu();

        if(actionTypes == null || actionTypes.Count == 0)
        {
            menu.AddDisabledItem(new GUIContent("No CardAction types found"));
        }
        else
        {
            for(int i = 0; i < actionTypes.Count; i++)
            {
                Type type = actionTypes[i];
                menu.AddItem(new GUIContent(type.Name), false, () => AddAction(type));
            }
        }
        menu.DropDown(buttonRect);
    }

    private void AddAction(Type type)
    {
        serializedObject.Update();

        int index = cardActionListProp.arraySize;
        cardActionListProp.InsertArrayElementAtIndex(index);

        SerializedProperty newElement = cardActionListProp.GetArrayElementAtIndex(index);
        newElement.managedReferenceValue = Activator.CreateInstance(type);
        newElement.isExpanded = true;

        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(target);
    }

    private void DeleteElementAt(int index)
    {
        serializedObject.Update();
        SerializedProperty element = cardActionListProp.GetArrayElementAtIndex(index);

        if(element.managedReferenceValue != null)
        {
            element.managedReferenceValue = null;
        }

        cardActionListProp.DeleteArrayElementAtIndex(index);
        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(target);
    }

    private string GetManagedRefernceTypeName(SerializedProperty property)
    {
        if(property == null || property.managedReferenceValue == null)
            return "Null";
        
        return property.managedReferenceValue.GetType().Name;
    }

}
