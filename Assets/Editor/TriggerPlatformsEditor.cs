using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;

[CustomEditor(typeof(TriggerPlatforms))]
public class TriggerPlatformsEditor : Editor
{
    SerializedProperty triggerType;

    //Disolve
    SerializedProperty timeToDisapear;
    SerializedProperty timeToApear;
    SerializedProperty objectToDisapear;

    //Button
    SerializedProperty isToggle;
    SerializedProperty targetObjects;

    private void OnEnable()
    {
        triggerType = serializedObject.FindProperty("triggerType");

        timeToDisapear = serializedObject.FindProperty("timeToDisapear");
        timeToApear = serializedObject.FindProperty("timeToApear");
        objectToDisapear = serializedObject.FindProperty("objectToDisapear");

        isToggle = serializedObject.FindProperty("isToggle");
        targetObjects = serializedObject.FindProperty("targetObjects");
    }

    public override void OnInspectorGUI()
    {
        TriggerPlatforms script = (TriggerPlatforms)target;

        serializedObject.Update();

        EditorGUILayout.PropertyField(triggerType);

        EditorGUILayout.Space(10);
        switch (script.triggerType)
        {
            case TriggerPlatforms.TriggerType.DisolveFloor:
                EditorGUILayout.PropertyField(timeToDisapear);
                EditorGUILayout.PropertyField(timeToApear);
                EditorGUILayout.PropertyField(objectToDisapear);
                break;

            case TriggerPlatforms.TriggerType.Button:
                EditorGUILayout.LabelField("Object to be open or interact with");
                EditorGUILayout.PropertyField(isToggle);
                EditorGUILayout.PropertyField(targetObjects);
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
