using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Create a singleton of this class
    public static UIManager Instance {get; private set;}

    [SerializeField][Tooltip("Reference to the Player's loadout scriptable object used during gameplay")]
    private Loadout _loadout;

    [SerializeField][Tooltip("The transform component of the turret button container")]
    private Transform _turretButtonContainerTransform;

    [SerializeField][Tooltip("The prefab of the turret button")]
    private Transform _turretButtonPrefab;

    private void Awake()
    {
        // Check if there is already an Instance of this in the scene
        if (Instance != null)
        {   
            // Destroy this extra copy if this is
            Destroy(gameObject);
            Debug.LogError("Cannot Have More Than One Instance of [UIManager] In The Scene!");
            return;
        } 
        Instance = this;
    }

    private void Start() 
    {
        CreateTurretMenuButtons();
    }

    private void CreateTurretMenuButtons()
    {
        // Instantiates the buttons for all 3 turrets in the build menu with the correct loadout
        // This function should only be called once at the start of a level

        foreach (Turret turret in _loadout.selectedTurrets)
        {
            // Create an instance of the turret button UI for this turret & cache the Transform of the button 
            Transform turretButtonTransform = Instantiate(_turretButtonPrefab, _turretButtonContainerTransform);
            TurretButtonUI turretButtonUI = turretButtonTransform.GetComponent<TurretButtonUI>();
            turretButtonUI.SetBaseTurret(turret); 
        }
    }
}
