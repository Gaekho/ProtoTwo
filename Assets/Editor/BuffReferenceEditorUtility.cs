#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class BuffReferenceEditorUtility
{
    private static List<Type> cachedBuffTypes;

    public static List<Type> BuffTypes
    {
        get
        {
            if (cachedBuffTypes == null)
            {
                cachedBuffTypes = TypeCache.GetTypesDerivedFrom<BuffBase>()
                    .Where(t => !t.IsAbstract && !t.IsGenericType)
                    .OrderBy(t => t.Name)
                    .ToList();
            }
            return cachedBuffTypes;
        }
    }

    public static void DrawBuffReferenceField(SerializedProperty buffProp, string label = "Buff")
    {
        if (buffProp == null)
        {
            EditorGUILayout.HelpBox("Buff property not found.", MessageType.Error);
            return;
        }

        EditorGUILayout.BeginVertical("box");

        EditorGUILayout.LabelField(label, EditorStyles.boldLabel);

        string currentTypeName = buffProp.managedReferenceValue == null
            ? "None"
            : buffProp.managedReferenceValue.GetType().Name;

        if (EditorGUILayout.DropdownButton(
                new GUIContent($"Buff Type : {currentTypeName}"),
                FocusType.Passive))
        {
            ShowBuffTypeMenu(buffProp);
        }

        if (buffProp.managedReferenceValue != null)
        {
            EditorGUILayout.Space(4);
            EditorGUILayout.PropertyField(buffProp, new GUIContent("Buff Data"), true);
        }

        EditorGUILayout.EndVertical();
    }

    private static void ShowBuffTypeMenu(SerializedProperty buffProp)
    {
        GenericMenu menu = new GenericMenu();

        SerializedObject so = buffProp.serializedObject;
        string propertyPath = buffProp.propertyPath;

        menu.AddItem(new GUIContent("None"), buffProp.managedReferenceValue == null, () =>
        {
            so.Update();
            SerializedProperty prop = so.FindProperty(propertyPath);
            prop.managedReferenceValue = null;
            so.ApplyModifiedProperties();
            EditorUtility.SetDirty(so.targetObject);
        });

        foreach (Type type in BuffTypes)
        {
            bool selected = buffProp.managedReferenceValue != null &&
                            buffProp.managedReferenceValue.GetType() == type;

            menu.AddItem(new GUIContent(type.Name), selected, () =>
            {
                so.Update();
                SerializedProperty prop = so.FindProperty(propertyPath);
                prop.managedReferenceValue = Activator.CreateInstance(type);
                prop.isExpanded = true;
                so.ApplyModifiedProperties();
                EditorUtility.SetDirty(so.targetObject);
            });
        }

        menu.ShowAsContext();
    }
}
#endif