/// CODED BY MARK GASKINS => https://www.youtube.com/developerjake :)
/// --------- 

using UnityEngine;
using UnityEditor;

/// <summary>
/// These are the overclock types that the script understands, ccording to the Overclock ability sheet on the google drive => https://docs.google.com/spreadsheets/d/116DQaC2F2QB0XkeNy8ylFG-T6ufD4h4sE6i-83MT_MM/edit#gid=0
/// </summary>
public enum OverclockType
{
    Hardener, // Damage reduction by damageReductionPercent%
    MassiveEMPBlast, // AOE attack that slows enemies on the field
    SelfTune_up, // Increase weapon fire rate & damage
    SquadTune_up, // Increase turret damage & fire rate
    MoveSpeedBoost, // Increase mech movement speed
    EmergencyRepairKit, // Heals the mech  over healTime
    Berserk // Increase meele speed & damage
}


[CreateAssetMenu(fileName = "Overclock Ability", menuName = "Offensive/Base Overclock")]
public class OverclockAbility : ScriptableObject
{
    #region Fields
        [Header("General Fields")]
        [Tooltip("The name of this ability.")] public string abilityName = "Sample Overclock";
        [Tooltip("How long does it take for this ability to recharge? (seconds)")] [Range(0,90)] public int cooldownTime = 10;
        public GameObject abilityStatsMenu;


        [Space(2)]
        [Header("Type-Specific Attributes")]
        [Tooltip("The overclock type that the entire script is dependant on.")] [SerializeField] public OverclockType overclockType;
        #region Hardener Fields
        [DrawIf("overclockType", OverclockType.Hardener)] [Range(1, 60)] [Tooltip("Duration of the overclock ability.")] public int duration_hrd = 15;
        [DrawIf("overclockType", OverclockType.Hardener), Tooltip("The packaged prefab which contain the vfx package for this overclock.")] public GameObject vfx_hrd;
        [DrawIf("overclockType", OverclockType.Hardener)] [Range(5, 100)] [Tooltip("This is the amount of damage that is reduced during the attack.")] public int damageReductionPercent = 80;
         
        #endregion
        #region EMP blast fields
        [DrawIf("overclockType", OverclockType.MassiveEMPBlast)] [Tooltip("The explosion vfx, spawns at the center of the player.")] public GameObject ExplosionVFX;
        [DrawIf("overclockType", OverclockType.MassiveEMPBlast)] [Tooltip("The explosion sound effects, spawns at the center of the player.")] public AudioClip ExplosionSFX;
        [DrawIf("overclockType", OverclockType.MassiveEMPBlast)] [Range(5, 50)] [Tooltip("This is the AOE range that the script calculates damage for when the attack is preformed.")] public float AOERange = 7;
        [DrawIf("overclockType", OverclockType.MassiveEMPBlast)] [Range(50,200)] [Tooltip("This is the maximum damage that one can recieve within the AOE.")] public int explosionPower = 95;
        [DrawIf("overclockType", OverclockType.MassiveEMPBlast)] [Range(2, 7)] [Tooltip("This is the new speed that enemies are slowed to in the AOE's [A] and [B] perimeters.")] public float slowTo = 4;
        [DrawIf("overclockType", OverclockType.MassiveEMPBlast)] [Range(0.5f, 5)] [Tooltip("This is the amount of time that enemies are slowed to in the AOE's [A] and [B] perimeters.")] public float slowTime = 2;

        #endregion
        #region Self tune-up fields
        [DrawIf("overclockType", OverclockType.SelfTune_up)] [Range(5, 60)] [Tooltip("Duration of the overclock ability.")] public int duration_stu = 15;
        [DrawIf("overclockType", OverclockType.SelfTune_up)] [Tooltip("The packaged prefab which contain the vfx package for this overclock.")] public GameObject vfx_stu;
        [DrawIf("overclockType", OverclockType.SelfTune_up)] [Range(.1f, 0.20f)] [Tooltip("New Weapon fire rate. (IMPORTANT NOTE, original fire rate = 0.23)")] public float newFireRate_stu = 0.15f;
        [DrawIf("overclockType", OverclockType.SelfTune_up)] [Range(1.1f, 2.5f)] [Tooltip("Damage boost multiplier. (IMPORTANT NOTE, if the original damage was 15, and the multiplier is 2, the new damage becomes 30! Be careful when using those higher values.)")] public float damageBoostMultiplier_stu = 1.5f;
        #endregion
        #region Squad tune-up fields    
        [DrawIf("overclockType", OverclockType.SquadTune_up)] [Range(1, 60)] [Tooltip("Duration of the overclock ability.")] public int duration_squ = 15;
        [DrawIf("overclockType", OverclockType.SquadTune_up)] [Tooltip("The packaged prefab which contain the vfx package for this overclock.")] public GameObject vfx_squ;
        [DrawIf("overclockType", OverclockType.SquadTune_up)] [Tooltip("The effect on the TURRETS (hence: 't' vfx). The packaged prefab which contain the vfx package for this overclock.")] public GameObject tVfx;
        [DrawIf("overclockType", OverclockType.SquadTune_up)] [Range(.1f, 0.20f)] [Tooltip("New Weapon fire rate FOR PLAYER GUNS. Turret guns is handled internally. Sorry! (IMPORTANT NOTE, original fire rate = 0.23)")] public float newFireRate_squ = 0.15f;
        [DrawIf("overclockType", OverclockType.SquadTune_up)] [Range(1.1f, 2.5f)] [Tooltip("Damage boost multiplier. (IMPORTANT NOTE, if the original damage was 30, and the multiplier is 2, the new damage becomes 60! Be careful when using those higher values.)")] public float damageBoostMultiplier_squ = 1.5f;
        #endregion
        #region Move Speed Boost fields
        [DrawIf("overclockType", OverclockType.MoveSpeedBoost)] [Range(1, 60)] [Tooltip("Duration of the overclock ability.")] public int duration_msb = 15;
        [DrawIf("overclockType", OverclockType.MoveSpeedBoost)] [Tooltip("The packaged prefab which contain the vfx package for this overclock.")] public GameObject vfx_msb;
        [DrawIf("overclockType", OverclockType.MoveSpeedBoost)] [Range(1.4f, 2.6f)] [Tooltip("Speed boost multiplier. (Be careful when using those higher values.)")] public float speedBoost = 1.5f;
        [HideInInspector] public float originalSpeed;
        #endregion
        #region Emergency repair kit (heal) fields
        // Heals the mech to 50% of its Max health, which will usually just give 50 health points to the player after healTime seconds.
        [DrawIf("overclockType", OverclockType.EmergencyRepairKit)] [Range(1, 15)] [Tooltip("How long does it take for the mech to be healed by 50% of its max health? (seconds)")] public int healTime = 10;
        [DrawIf("overclockType", OverclockType.EmergencyRepairKit), Tooltip("The packaged prefab which contain the vfx package for this overclock.")] public GameObject vfx_heal;
        [DrawIf("overclockType", OverclockType.EmergencyRepairKit), Tooltip("[DEBUG] Lowers the player health by EIGHTY POINTS to test stuff out without being hurt. Do not forget to turn that crap off.")] public bool lowerPlayerHealth = false;
        #endregion 
        #region Berserk
        [DrawIf("overclockType", OverclockType.Berserk)] [Range(1, 60)] [Tooltip("Duration of the overclock ability.")] public int duration_bzk = 15;
        [DrawIf("overclockType", OverclockType.Berserk), Tooltip("The packaged prefab which contain the vfx package for this overclock.")] public GameObject vfx_bzk;
        [DrawIf("overclockType", OverclockType.Berserk)] [Range(0.01f, 1.00f)] [Tooltip("The time between melee attacks in seconds. (IMPORTANT NOTE, the original time between attacks is 0.5 seconds.)")] public float newMeleeAttackDelay = 0.15f;
        [DrawIf("overclockType", OverclockType.Berserk)] [Range(1.1f, 2.5f)] [Tooltip("Damage boost multiplier. (IMPORTANT NOTE, if the original damage was 15, and the multiplier is 2, the new damage becomes 30! Be careful when using those higher values.)")] public float damageBoostMultiplier_bzk = 1.5f;
        [HideInInspector] public float originalFirerate, originalDamage; // Calculations only
        #endregion

    #endregion


}


/// <summary>
/// UNITY EDITOR MECHANICS EXCERPT ---- 
/// Used to make the screen show different value depending on which enum value is selected, useful for these attacks, sense they're all so different.
/// </summary>
[CustomEditor(typeof(OverclockAbility))]
public class HelpBoxes : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        OverclockAbility overclockAbility = (OverclockAbility)target;

        EditorGUILayout.PropertyField(serializedObject.FindProperty("abilityName"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("cooldownTime"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("abilityStatsMenu"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("overclockType"));
        

        bool itsDefaultLol = false; // why is it default lol whos making this game 
        switch (overclockAbility.overclockType)
        {
            case OverclockType.Hardener:
                itsDefaultLol = true; //dBro what? who is even making the ability right now who is this hello?
                //  (I will be so surprised if anyone ever reads this mess)
                // Mark, I read this mess
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
            default:
                break;
        }

        if (!itsDefaultLol)
        {//good
            EditorGUILayout.Space(5);
            EditorGUILayout.HelpBox($"[Balance Note] The values for '{overclockAbility.overclockType}' are not the same as the values for the other types.\n\nPlease REVIEW the tooltips for each value by hovering over them when balancing.", MessageType.Warning, true);
        }

        serializedObject.ApplyModifiedProperties();
    }
}

public class DrawIfAttribute : PropertyAttribute
    {
        public string conditionFieldName;
        public object expectedValue;

        public DrawIfAttribute(string conditionFieldName, object expectedValue)
        {
            this.conditionFieldName = conditionFieldName;
            this.expectedValue = expectedValue;
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
            default:
                Debug.LogError("Unsupported property type: " + conditionProperty.propertyType);
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
            Debug.LogError("Cannot find property with name: " + drawIf.conditionFieldName);
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
            default:
                Debug.LogError("Unsupported property type: " + conditionProperty.propertyType);
                break;
        }

        return show ? base.GetPropertyHeight(property, label) : 0f;
    }
}
