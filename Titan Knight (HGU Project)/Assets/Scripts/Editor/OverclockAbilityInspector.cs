using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(OverclockAbility))]
public class OverclockAbilityEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        OverclockAbility overclockAbility = (OverclockAbility)target;

        EditorGUILayout.PropertyField(serializedObject.FindProperty("abilityName"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("cooldownTime"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("abilityStatsMenu"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("overclockType"));

        bool itsDefaultLol = false;

        switch (overclockAbility.overclockType)
        {
            case OverclockType.Hardener:
                itsDefaultLol = true;
                EditorGUILayout.PropertyField(serializedObject.FindProperty("duration_hrd"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("vfx_hrd"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("damageReductionPercent"));
                break;

            case OverclockType.MassiveEMPBlast:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("AOERange"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("ExplosionVFX"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("ExplosionSFX"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("explosionPower"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("slowTo"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("slowTime"));
                break;

            case OverclockType.SelfTune_up:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("duration_stu"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("vfx_stu"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("newFireRate_stu"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("damageBoostMultiplier_stu"));
                break;

            case OverclockType.SquadTune_up:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("duration_squ"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("vfx_squ"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("tVfx"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("newFireRate_squ"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("damageBoostMultiplier_squ"));
                break;

            case OverclockType.MoveSpeedBoost:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("duration_msb"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("vfx_msb"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("speedBoost"));
                break;

            case OverclockType.EmergencyRepairKit:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("healTime"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("vfx_heal"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("lowerPlayerHealth"));
                break;

            case OverclockType.Berserk:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("duration_bzk"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("vfx_bzk"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("newMeleeAttackDelay"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("damageBoostMultiplier_bzk"));
                break;
        }

        if (!itsDefaultLol)
        {
            EditorGUILayout.Space(5);
            EditorGUILayout.HelpBox($"[Balance Note] The values for '{overclockAbility.overclockType}' are not the same as the values for the other types.\n\nPlease REVIEW the tooltips for each value by hovering over them when balancing.", MessageType.Warning, true);
        }

        serializedObject.ApplyModifiedProperties();
    }
}

[CustomPropertyDrawer(typeof(DrawIfAttribute))]
public class DrawIfPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        DrawIfAttribute drawIf = attribute as DrawIfAttribute;
        SerializedProperty conditionProperty = property.serializedObject.FindProperty(drawIf.conditionFieldName);

        if (conditionProperty == null)
        {
            Debug.LogError("Cannot find property with name: " + drawIf.conditionFieldName);
            return;
        }

        bool show = false;
        switch (conditionProperty.propertyType)
        {
            case SerializedPropertyType.Boolean:
                show = conditionProperty.boolValue == (bool)drawIf.expectedValue;
                break;
            case SerializedPropertyType.Enum:
                show = conditionProperty.enumValueIndex == (int)drawIf.expectedValue;
                break;
        }

        if (show)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        DrawIfAttribute drawIf = attribute as DrawIfAttribute;
        SerializedProperty conditionProperty = property.serializedObject.FindProperty(drawIf.conditionFieldName);

        if (conditionProperty == null)
        {
            return base.GetPropertyHeight(property, label);
        }

        bool show = false;
        switch (conditionProperty.propertyType)
        {
            case SerializedPropertyType.Boolean:
                show = conditionProperty.boolValue == (bool)drawIf.expectedValue;
                break;
            case SerializedPropertyType.Enum:
                show = conditionProperty.enumValueIndex == (int)drawIf.expectedValue;
                break;
        }

        return show ? base.GetPropertyHeight(property, label) : 0f;
    }
}
