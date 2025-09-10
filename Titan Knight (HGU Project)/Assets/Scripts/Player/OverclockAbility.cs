/// CODED BY MARK GASKINS => https://www.youtube.com/developerjake :)
/// --------- 

using UnityEngine;

/// <summary>
/// These are the overclock types that the script understands, 
/// according to the Overclock ability sheet on the google drive
/// </summary>
public enum OverclockType
{
    Hardener,
    MassiveEMPBlast,
    SelfTune_up,
    SquadTune_up,
    MoveSpeedBoost,
    EmergencyRepairKit,
    Berserk
}

[CreateAssetMenu(fileName = "Overclock Ability", menuName = "Offensive/Base Overclock")]
public class OverclockAbility : ScriptableObject
{
    #region Fields
    [Header("General Fields")]
    [Tooltip("The name of this ability.")] 
    public string abilityName = "Sample Overclock";

    [Tooltip("How long does it take for this ability to recharge? (seconds)")] 
    [Range(0, 90)] public int cooldownTime = 10;

    public GameObject abilityStatsMenu;

    [Space(2)]
    [Header("Type-Specific Attributes")]
    [Tooltip("The overclock type that the entire script is dependant on.")] 
    [SerializeField] public OverclockType overclockType;

    #region Hardener Fields
    [DrawIf("overclockType", OverclockType.Hardener)] [Range(1, 60)]
    public int duration_hrd = 15;
    [DrawIf("overclockType", OverclockType.Hardener)] 
    public GameObject vfx_hrd;
    [DrawIf("overclockType", OverclockType.Hardener)] [Range(5, 100)]
    public int damageReductionPercent = 80;
    #endregion

    #region EMP blast fields
    [DrawIf("overclockType", OverclockType.MassiveEMPBlast)] 
    public GameObject ExplosionVFX;
    [DrawIf("overclockType", OverclockType.MassiveEMPBlast)] 
    public AudioClip ExplosionSFX;
    [DrawIf("overclockType", OverclockType.MassiveEMPBlast)] [Range(5, 50)]
    public float AOERange = 7;
    [DrawIf("overclockType", OverclockType.MassiveEMPBlast)] [Range(50, 200)]
    public int explosionPower = 95;
    [DrawIf("overclockType", OverclockType.MassiveEMPBlast)] [Range(2, 7)]
    public float slowTo = 4;
    [DrawIf("overclockType", OverclockType.MassiveEMPBlast)] [Range(0.5f, 5)]
    public float slowTime = 2;
    #endregion

    #region Self tune-up fields
    [DrawIf("overclockType", OverclockType.SelfTune_up)] [Range(5, 60)]
    public int duration_stu = 15;
    [DrawIf("overclockType", OverclockType.SelfTune_up)] 
    public GameObject vfx_stu;
    [DrawIf("overclockType", OverclockType.SelfTune_up)] [Range(.1f, 0.20f)]
    public float newFireRate_stu = 0.15f;
    [DrawIf("overclockType", OverclockType.SelfTune_up)] [Range(1.1f, 2.5f)]
    public float damageBoostMultiplier_stu = 1.5f;
    #endregion

    #region Squad tune-up fields
    [DrawIf("overclockType", OverclockType.SquadTune_up)] [Range(1, 60)]
    public int duration_squ = 15;
    [DrawIf("overclockType", OverclockType.SquadTune_up)] 
    public GameObject vfx_squ;
    [DrawIf("overclockType", OverclockType.SquadTune_up)] 
    public GameObject tVfx;
    [DrawIf("overclockType", OverclockType.SquadTune_up)] [Range(.1f, 0.20f)]
    public float newFireRate_squ = 0.15f;
    [DrawIf("overclockType", OverclockType.SquadTune_up)] [Range(1.1f, 2.5f)]
    public float damageBoostMultiplier_squ = 1.5f;
    #endregion

    #region Move Speed Boost fields
    [DrawIf("overclockType", OverclockType.MoveSpeedBoost)] [Range(1, 60)]
    public int duration_msb = 15;
    [DrawIf("overclockType", OverclockType.MoveSpeedBoost)] 
    public GameObject vfx_msb;
    [DrawIf("overclockType", OverclockType.MoveSpeedBoost)] [Range(1.4f, 2.6f)]
    public float speedBoost = 1.5f;
    [HideInInspector] public float originalSpeed;
    #endregion

    #region Emergency repair kit fields
    [DrawIf("overclockType", OverclockType.EmergencyRepairKit)] [Range(1, 15)]
    public int healTime = 10;
    [DrawIf("overclockType", OverclockType.EmergencyRepairKit)] 
    public GameObject vfx_heal;
    [DrawIf("overclockType", OverclockType.EmergencyRepairKit)] 
    public bool lowerPlayerHealth = false;
    #endregion

    #region Berserk
    [DrawIf("overclockType", OverclockType.Berserk)] [Range(1, 60)]
    public int duration_bzk = 15;
    [DrawIf("overclockType", OverclockType.Berserk)] 
    public GameObject vfx_bzk;
    [DrawIf("overclockType", OverclockType.Berserk)] [Range(0.01f, 1.00f)]
    public float newMeleeAttackDelay = 0.15f;
    [DrawIf("overclockType", OverclockType.Berserk)] [Range(1.1f, 2.5f)]
    public float damageBoostMultiplier_bzk = 1.5f;
    [HideInInspector] public float originalFirerate, originalDamage;
    #endregion
    #endregion
}

/// <summary>
/// Custom attribute used for conditional field drawing
/// </summary>
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
