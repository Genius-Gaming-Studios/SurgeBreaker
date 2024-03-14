/// CODED BY MARK GASKINS -- https://www.youtube.com/developerjake :)
/// --------- 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Manager of everthing related to the OVC abilities. (NOTE: Overclock sometimes abbrv. to 'OVC'.)
/// </summary>
public class OverclockManager : MonoBehaviour
{

    public static OverclockManager Instance {get; private set;}
    
    [SerializeField, Tooltip("This must be assigned! It is an empty object inside of the Player controller, located at exactly (0,-1,0).")] public Transform OverclockLocation; // Super important, must be located at (0,-1,0). It is in the root of the Player controller, the only other empty object with Model Parent.

    [Header("Overclock Manager"), Tooltip("The Selected overclock ability. (For now, this is manually set, when loadout is finished it will be automatic.)")]
    [SerializeField] public OverclockAbility CurrentOverclockAbility;
    
    /// (warning stuff here, check overclockmanagereditor below to modify)

    [Tooltip("The hotkey interacted with in order to use the overclock ability."), SerializeField] public KeyCode OverclockHotkey = KeyCode.Space; // Key is Subject to chagne
    [Tooltip("Should the player have to DOUBLE CLICK the OverclockHotkey to enable it? (Recommended)"), SerializeField] public bool doubleClickActivate = true; // Double click to activate overclock ability

    [Space(6), Header("Debug")]
    [Tooltip("Can the player currently use the overclock ability? (It is determined via the cooldown period of the current overclock ability) [DEBUG ONLY]"), SerializeField] private bool canOverclock; // DEBUG ONLY
    [Tooltip("Time remaining on cooldown. Read only."), SerializeField] public float cooldownTimer = 0;

    public UnityEngine.UI.Text DEBUG_COOLDOWN_TEXT; // DEBUG ONLY!!

    /// <summary> The time that the have between when the double click hotkey is clicked once and twice. (Note, only active for if doubleClickActivate is true, duh.)</summary>
    private float doubleClickTimeThreshold = 0.8f;
    private float lastClickTime;

    private PlayerManager pm;

    /// <summary>
    /// Can the player overclock?
    /// </summary>
    /// <returns>Simply returns true if the player is currently able to press the overclock key x number of times to activate the overclock ability.</returns>
    public bool CanOverclock()
    {
        return cooldownTimer <= 0;
    }

    private void Awake()
    {
        // Check if there is already an Instance of this in the scene
        if (Instance != null)
        {   
            // Destroy this extra copy if this is
            Destroy(gameObject);
            Debug.LogError("Cannot Have More Than One Instance of [OverclockManager] In The Scene!");
            return;
        } 
        Instance = this;

        // Init all values here
        canOverclock = true;

        if (CurrentOverclockAbility == null) Debug.LogWarningFormat("<b>[Overclock Manager]</b> Current overclock ability is not manually assigned! If this is not set automatically by the layout, there will be errors in game.");
        else
        {
            cooldownTimer = CurrentOverclockAbility.cooldownTime; // cool beans thats what we like to see
        }

        if (OverclockLocation == null) Debug.LogWarningFormat("<b>[Overclock Manager]</b> The super important Overclock Location transform object is not assigned! There will be errors in game! <b>Assign it by creating an empty object in the <u>root</u> of the Player Controller, pose it at x=0, y=-1, z=0, and then drag it to the OverclockLocation reference.</b>");

        pm = FindObjectOfType<PlayerManager>(); // Assign player manager to reference the overclock mechanics on the player.
    }


    private void Update()
    {
        #region Overclock Detection
        canOverclock = CanOverclock(); // DEBUG ONLY

        if (Input.GetKeyDown(OverclockHotkey) && canOverclock)
        {
            if (doubleClickActivate)
            {
                float timeSinceLastClick = Time.time - lastClickTime;

                if (timeSinceLastClick <= doubleClickTimeThreshold)
                {
                    // Player double clicked hotkey, handle ovc ability (passing in overclockability as a reference just to save time in having to type out that massive word mumbo jumbo)
                    HandleOverclockUsage(CurrentOverclockAbility);
                }

                lastClickTime = Time.time;
            }
            else
                HandleOverclockUsage(CurrentOverclockAbility);
        }
        else if (Input.GetKeyDown(OverclockHotkey))
        {
            Debug.Log("<color=yellow><b>[Overclock Manager]</b> Overclock attempted to activate, but OVC is still within the cooldown period. Ignoring.</color>");
        }
        #endregion

        /// HANDLE COOLDOWN MECHANICS HERE!
        if (cooldownTimer <= 0f)
        {
            canOverclock = true;
            doRunTimer = false;
            Debug.Log("[Overclock Manager] Cooldown completed. Overclock ability interactable.");
           
            if (DEBUG_COOLDOWN_TEXT != null) DEBUG_COOLDOWN_TEXT.text = string.Empty; // debug
        }
        else if (doRunTimer)
        {
            cooldownTimer -= Time.deltaTime;

            if (DEBUG_COOLDOWN_TEXT != null)  DEBUG_COOLDOWN_TEXT.text = $"ovc cooldown: {cooldownTimer:F2}"; // debug
        }
    }

    public bool doRunTimer = true; // Debug only.
    
    /// <summary>
    /// Run the overclock timer. It's handled in the overclock manager manually, and not the player manager.
    /// </summary>
    private void StartCooldownTimer(OverclockAbility stats)
    {
        Debug.LogWarning("[Overclock Manager] Timer begun...");

        cooldownTimer = stats.cooldownTime;
    }

    /// <summary>
    /// Automatically called when the player activates the OVC using the hotkey, by either single or double clicking the hotkey button.
    /// </summary>
    public void HandleOverclockUsage(OverclockAbility ovc)
    {
        // Handle overclock usage
        Debug.Log($"<b>[Overclock Manager]</b> Activation detected, Handling Overclock Usage... (<i>{ovc.abilityName.ToUpper()} ABILITY UTILIZED!</i>)");
        StartCooldownTimer(ovc);
        
        /// Determine which overclock is to be used, and subsequentl, use the method to handle that usage, reset the cooldown clock, all that good stuff.
        switch (ovc.overclockType)
        {
            // Indivisual OVC abilities are directly handled in here.
            case OverclockType.Hardener:
                pm.DoHardenerAbility(ovc); // Non Complete!
                break;
            case OverclockType.MassiveEMPBlast:
                pm.DoEMPAbility(ovc); // Complete
                break;
            case OverclockType.SelfTune_up:
                pm.DoSelfTuneUpAbility(ovc); // Complete!
                break;
            case OverclockType.SquadTune_up:
                // Should do this for the player AND the turrets.
                pm.DoSquadTuneUpAbility(ovc); // Non Complete!
                break;
            case OverclockType.MoveSpeedBoost:
                pm.DoSpeedAbility(ovc); // Non Complete!
                break;
            case OverclockType.EmergencyRepairKit:
                pm.DoHealAbility(ovc); // Complete!
                break;
            case OverclockType.Berserk:
                pm.DoBerzerkAbility(ovc); // Complete!
                break;
        }
        
        cooldownTimer = ovc.cooldownTime;
    }

}



/// <summary>
/// UNITY EDITOR MECHANICS EXCERPT ---- 
/// Used to make the screen show different value depending on which enum value is selected, useful for these attacks, sense they're all so different.
/// </summary>

[CustomEditor(typeof(OverclockManager))]
public class OverclockManagerEditor : Editor
{
    // my custom layout is here for debugging stuff
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        OverclockManager overclockManager = (OverclockManager)target;

        EditorGUILayout.PropertyField(serializedObject.FindProperty("CurrentOverclockAbility"));

        // Warnings fo current overclock ability
        if (overclockManager.CurrentOverclockAbility == null)
        {
            EditorGUILayout.HelpBox("Current overclock ability is not manually assigned! If this is not set automatically by the layout, there will be errors in game.", MessageType.Warning);
        }
        else
        {
            // normal, this means its manual mode 
            EditorGUILayout.HelpBox("[EXPERIMENTAL] Current overclock ability was MANUALLY assigned. This is an experimental feature!", MessageType.Info);
        }
        EditorGUILayout.Space(6);
        
        EditorGUILayout.PropertyField(serializedObject.FindProperty("OverclockHotkey"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("doubleClickActivate"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("OverclockLocation"));


        // warnings for double click bool
        if (!overclockManager.doubleClickActivate)
        {
            EditorGUILayout.HelpBox("[EXPERIMENTAL] Allowing the player to double click prevents them from accidentally activating OVC ability. Turning this off is just for experimental use only.", MessageType.Info);
            EditorGUILayout.Space(6);
        }

        EditorGUILayout.PropertyField(serializedObject.FindProperty("canOverclock"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("DEBUG_COOLDOWN_TEXT"));

        serializedObject.ApplyModifiedProperties();
    }

}
