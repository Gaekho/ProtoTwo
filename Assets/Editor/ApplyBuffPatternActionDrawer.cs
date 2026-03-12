#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ApplyBuffPatternAction))]
public class ApplyBuffPatternActionDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (!property.isExpanded)
            return EditorGUIUtility.singleLineHeight;

        SerializedProperty actionTargetProp = property.FindPropertyRelative("actionTarget");
        SerializedProperty buffProp = property.FindPropertyRelative("buff");

        float height = 0f;
        float spacing = EditorGUIUtility.standardVerticalSpacing;

        height += EditorGUIUtility.singleLineHeight;
        height += spacing + EditorGUI.GetPropertyHeight(actionTargetProp, true);
        height += spacing + EditorGUIUtility.singleLineHeight;

        if (buffProp != null && buffProp.managedReferenceValue != null)
        {
            height += spacing + EditorGUI.GetPropertyHeight(buffProp, true);
        }

        height += 8f;
        return height;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty actionTargetProp = property.FindPropertyRelative("actionTarget");
        SerializedProperty buffProp = property.FindPropertyRelative("buff");

        float line = EditorGUIUtility.singleLineHeight;
        float spacing = EditorGUIUtility.standardVerticalSpacing;

        Rect rect = new Rect(position.x, position.y, position.width, line);

        property.isExpanded = EditorGUI.Foldout(rect, property.isExpanded, label, true);

        if (!property.isExpanded)
            return;

        EditorGUI.indentLevel++;

        rect.y += line + spacing;
        rect.height = EditorGUI.GetPropertyHeight(actionTargetProp, true);
        EditorGUI.PropertyField(rect, actionTargetProp, true);

        rect.y += rect.height + spacing;
        DrawBuffPickerGUI(rect, buffProp);

        if (buffProp != null && buffProp.managedReferenceValue != null)
        {
            rect.y += line + spacing;
            rect.height = EditorGUI.GetPropertyHeight(buffProp, true);
            EditorGUI.PropertyField(rect, buffProp, new GUIContent("Buff Data"), true);
        }

        EditorGUI.indentLevel--;
    }

    private void DrawBuffPickerGUI(Rect rect, SerializedProperty buffProp)
    {
        Rect leftRect = new Rect(rect.x, rect.y, 70f, EditorGUIUtility.singleLineHeight);
        Rect rightRect = new Rect(rect.x + 75f, rect.y, rect.width - 75f, EditorGUIUtility.singleLineHeight);

        EditorGUI.LabelField(leftRect, "Buff");

        string currentTypeName = buffProp.managedReferenceValue == null
            ? "None"
            : buffProp.managedReferenceValue.GetType().Name;

        if (GUI.Button(rightRect, currentTypeName, EditorStyles.popup))
        {
            ShowBuffTypeMenu(buffProp);
        }
    }

    private void ShowBuffTypeMenu(SerializedProperty buffProp)
    {
        GenericMenu menu = new GenericMenu();
        string propertyPath = buffProp.propertyPath;
        SerializedObject so = buffProp.serializedObject;

        menu.AddItem(new GUIContent("None"), buffProp.managedReferenceValue == null, () =>
        {
            so.Update();
            SerializedProperty prop = so.FindProperty(propertyPath);
            prop.managedReferenceValue = null;
            so.ApplyModifiedProperties();
            EditorUtility.SetDirty(so.targetObject);
        });

        foreach (System.Type type in BuffReferenceEditorUtility.BuffTypes)
        {
            bool selected = buffProp.managedReferenceValue != null &&
                            buffProp.managedReferenceValue.GetType() == type;

            menu.AddItem(new GUIContent(type.Name), selected, () =>
            {
                so.Update();
                SerializedProperty prop = so.FindProperty(propertyPath);
                prop.managedReferenceValue = System.Activator.CreateInstance(type);
                prop.isExpanded = true;
                so.ApplyModifiedProperties();
                EditorUtility.SetDirty(so.targetObject);
            });
        }

        menu.ShowAsContext();
    }
}
#endif