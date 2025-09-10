using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(OverclockManager))]
public class OverclockManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        OverclockManager overclockManager = (OverclockManager)target;

        EditorGUILayout.PropertyField(serializedObject.FindProperty("CurrentOverclockAbility"));

        if (overclockManager.CurrentOverclockAbility == null)
        {
            EditorGUILayout.HelpBox("Current overclock ability is not manually assigned! If this is not set automatically by the layout, there will be errors in game.", MessageType.Warning);
        }
        else
        {
            EditorGUILayout.HelpBox("[EXPERIMENTAL] Current overclock ability was MANUALLY assigned. This is an experimental feature!", MessageType.Info);
        }
        EditorGUILayout.Space(6);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("OverclockHotkey"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("doubleClickActivate"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("OverclockLocation"));

        if (!overclockManager.doubleClickActivate)
        {
            EditorGUILayout.HelpBox("[EXPERIMENTAL] Allowing the player to double click prevents them from accidentally activating OVC ability. Turning this off is just for experimental use only.", MessageType.Info);
            EditorGUILayout.Space(6);
        }

        EditorGUILayout.PropertyField(serializedObject.FindProperty("canOverclock"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("DEBUG_COOLDOWN_TEXT"), true);

        serializedObject.ApplyModifiedProperties();
    }
}
