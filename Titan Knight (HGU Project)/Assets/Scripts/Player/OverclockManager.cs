/// CODED BY MARK GASKINS -- https://www.youtube.com/developerjake :)
/// --------- 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manager of everything related to the OVC abilities. (NOTE: Overclock sometimes abbrv. to 'OVC'.)
/// </summary>
public class OverclockManager : MonoBehaviour
{
    public static OverclockManager Instance { get; private set; }

    [SerializeField, Tooltip("This must be assigned! It is an empty object inside of the Player controller, located at exactly (0,-1,0).")]
    public Transform OverclockLocation; 

    [Header("Overclock Manager"), Tooltip("The Selected overclock ability. (For now, this is manually set, when loadout is finished it will be automatic.)")]
    [SerializeField] 
    public OverclockAbility CurrentOverclockAbility;

    [Tooltip("The hotkey interacted with in order to use the overclock ability.")]
    [SerializeField] 
    public KeyCode OverclockHotkey = KeyCode.Space;

    [Tooltip("Should the player have to DOUBLE CLICK the OverclockHotkey to enable it? (Recommended)")]
    [SerializeField] 
    public bool doubleClickActivate = true;

    [Space(6), Header("Debug")]
    [Tooltip("Can the player currently use the overclock ability? (It is determined via the cooldown period of the current overclock ability) [DEBUG ONLY]")]
    [SerializeField] 
    private bool canOverclock;

    [Tooltip("Time remaining on cooldown. Read only.")]
    [SerializeField] 
    public float cooldownTimer = 0;

    public UnityEngine.UI.Text DEBUG_COOLDOWN_TEXT; // DEBUG ONLY!!

    private float doubleClickTimeThreshold = 0.8f;
    private float lastClickTime;

    private PlayerManager pm;

    public bool doRunTimer = true; // Debug only.

    public bool CanOverclock()
    {
        return cooldownTimer <= 0;
    }

    private void Awake()
    {
        CurrentOverclockAbility = GameManager.Instance.loadout.selectedAbility;

        if (Instance != null)
        {
            Destroy(gameObject);
            Debug.LogError("Cannot Have More Than One Instance of [OverclockManager] In The Scene!");
            return;
        }
        Instance = this;

        canOverclock = true;

        if (CurrentOverclockAbility == null)
            Debug.LogWarning("<b>[Overclock Manager]</b> Current overclock ability is not manually assigned! If this is not set automatically by the layout, there will be errors in game.");
        else
            cooldownTimer = CurrentOverclockAbility.cooldownTime;

        if (OverclockLocation == null)
            Debug.LogWarning("<b>[Overclock Manager]</b> The super important Overclock Location transform object is not assigned! There will be errors in game! <b>Assign it by creating an empty object in the <u>root</u> of the Player Controller, pose it at x=0, y=-1, z=0, and then drag it to the OverclockLocation reference.</b>");

        pm = FindObjectOfType<PlayerManager>();
    }

    private void Update()
    {
        canOverclock = CanOverclock(); 

        if (Input.GetKeyDown(OverclockHotkey) && canOverclock)
        {
            if (doubleClickActivate)
            {
                float timeSinceLastClick = Time.time - lastClickTime;
                if (timeSinceLastClick <= doubleClickTimeThreshold)
                    HandleOverclockUsage(CurrentOverclockAbility);

                lastClickTime = Time.time;
            }
            else
            {
                HandleOverclockUsage(CurrentOverclockAbility);
            }
        }
        else if (Input.GetKeyDown(OverclockHotkey))
        {
            Debug.Log("<color=yellow><b>[Overclock Manager]</b> Overclock attempted to activate, but OVC is still within the cooldown period. Ignoring.</color>");
        }

        if (cooldownTimer <= 0f)
        {
            canOverclock = true;
            doRunTimer = false;
            if (DEBUG_COOLDOWN_TEXT != null) DEBUG_COOLDOWN_TEXT.text = string.Empty;
        }
        else if (doRunTimer)
        {
            cooldownTimer -= Time.deltaTime;
            if (DEBUG_COOLDOWN_TEXT != null) DEBUG_COOLDOWN_TEXT.text = $"ovc cooldown: {cooldownTimer:F2}";
        }

        if (pm == null) pm = FindObjectOfType<PlayerManager>();
    }

    private void StartCooldownTimer(OverclockAbility stats)
    {
        Debug.LogWarning("[Overclock Manager] Timer begun...");
        cooldownTimer = stats.cooldownTime;
    }

    public void HandleOverclockUsage(OverclockAbility ovc)
    {
        Debug.Log($"<b>[Overclock Manager]</b> Activation detected, Handling Overclock Usage... (<i>{ovc.abilityName.ToUpper()} ABILITY UTILIZED!</i>)");
        StartCooldownTimer(ovc);

        switch (ovc.overclockType)
        {
            case OverclockType.Hardener: pm.DoHardenerAbility(ovc); break;
            case OverclockType.MassiveEMPBlast: pm.DoEMPAbility(ovc); break;
            case OverclockType.SelfTune_up: pm.DoSelfTuneUpAbility(ovc); break;
            case OverclockType.SquadTune_up: pm.DoSquadTuneUpAbility(ovc); break;
            case OverclockType.MoveSpeedBoost: pm.DoSpeedAbility(ovc); break;
            case OverclockType.EmergencyRepairKit: pm.DoHealAbility(ovc); break;
            case OverclockType.Berserk: pm.DoBerzerkAbility(ovc); break;
        }

        cooldownTimer = ovc.cooldownTime;
    }
}
