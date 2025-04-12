using UnityEditor;
using UnityEngine;
using echo17.EndlessBook;

[CustomEditor(typeof(EndlessBook))]
public class EndlessBookEditor : Editor
{
    private SerializedProperty bookLeftProperty;
    private SerializedProperty bookRightProperty;
    private SerializedProperty pageDataProperty;

    private void OnEnable()
    {
        bookLeftProperty = serializedObject.FindProperty("_bookLeft");
        bookRightProperty = serializedObject.FindProperty("_bookRight");
        pageDataProperty = serializedObject.FindProperty("pageData");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(bookLeftProperty, new GUIContent("Book Left"));
        EditorGUILayout.PropertyField(bookRightProperty, new GUIContent("Book Right"));

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Page Data", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(pageDataProperty, true);

        serializedObject.ApplyModifiedProperties();
    }
} 