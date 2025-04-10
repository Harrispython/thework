using UnityEditor;
using UnityEngine;
using echo17.EndlessBook;

[CustomEditor(typeof(EndlessBook))]
public class BookInspector : Editor
{
    private SerializedProperty bookLeftProperty;
    private SerializedProperty bookRightProperty;
    private SerializedProperty pageDataProperty;

    private void OnEnable()
    {
        Debug.Log("BookInspector OnEnable called");
        bookLeftProperty = serializedObject.FindProperty("_bookLeft");
        bookRightProperty = serializedObject.FindProperty("_bookRight");
        pageDataProperty = serializedObject.FindProperty("pageData");

        if (bookLeftProperty == null)
            Debug.LogError("Could not find _bookLeft property");
        if (bookRightProperty == null)
            Debug.LogError("Could not find _bookRight property");
        if (pageDataProperty == null)
            Debug.LogError("Could not find pageData property");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        if (bookLeftProperty != null)
            EditorGUILayout.PropertyField(bookLeftProperty, new GUIContent("Book Left"));
        else
            EditorGUILayout.HelpBox("Book Left property not found", MessageType.Error);

        if (bookRightProperty != null)
            EditorGUILayout.PropertyField(bookRightProperty, new GUIContent("Book Right"));
        else
            EditorGUILayout.HelpBox("Book Right property not found", MessageType.Error);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Page Data", EditorStyles.boldLabel);
        if (pageDataProperty != null)
            EditorGUILayout.PropertyField(pageDataProperty, true);
        else
            EditorGUILayout.HelpBox("Page Data property not found", MessageType.Error);

        serializedObject.ApplyModifiedProperties();
    }
} 