using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    // Create a singleton of this class
    public static UIManager Instance {get; private set;}

    [Space(10)][Header("Build Menu")][Space(5)]
    [SerializeField][Tooltip("Reference to the turret menu UI canvas")]private GameObject _buildCanvas;
    [Tooltip("This will show a (prototype) list of turrets that you can build when you press mouse 0 on the build node.")] [SerializeField] private GameObject _buildMenuUI;
    [SerializeField][Tooltip("The transform component of the turret button container")] private Transform _turretButtonContainerTransform;
    [SerializeField][Tooltip("The prefab of the turret button")] private Transform _turretButtonPrefab;


    [Space(10)][Header("Loadout Menu")][Space(5)]
    [SerializeField][Tooltip("The transform component of the loadout text container")] private Transform _loadoutTextContainerTransform;

    [SerializeField][Tooltip("The prefab of the loadout text object")] private Transform _loadoutTextPrefab;

    [Space(10)][Header("Canvases")][Space(5)]
    [SerializeField] GameObject _combatCanvas;
    [SerializeField] GameObject _mainCanvas;
    [SerializeField] GameObject _gameOverCanvas;
    [SerializeField] GameObject _missionSucessCanvas;



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

    private void CreateTurretMenuButtons()
    {
        // Instantiates the buttons for all 3 turrets in the build menu with the correct loadout
        // This function should only be called once at the start of a level

        foreach (Turret turret in GameManager.Instance.loadout.selectedTurrets)
        {
            // Create an instance of the turret button UI for this turret & cache the Transform of the button 
            Transform turretButtonTransform = Instantiate(_turretButtonPrefab, _turretButtonContainerTransform);
            TurretButtonUI turretButtonUI = turretButtonTransform.GetComponent<TurretButtonUI>();
            turretButtonUI.SetBaseTurret(turret, true); 
        }
    }

    private void CreateLoadoutText()
    {
        // Instantiates UI text objects for the player's weapon, overclock ability, & melee, and displays their respective key bindings

        // Create The Weapon Text
        Transform weaponText = Instantiate(_loadoutTextPrefab, _loadoutTextContainerTransform);
        weaponText.GetComponent<TextMeshProUGUI>().text = GameManager.Instance.loadout.selectedWeapon.gunName + ": Mouse0";

        // Create The Overclock Ability Text
        Transform abilityText = Instantiate(_loadoutTextPrefab, _loadoutTextContainerTransform);
        abilityText.GetComponent<TextMeshProUGUI>().text = GameManager.Instance.loadout.selectedAbility.abilityName + ": " + OverclockManager.Instance.OverclockHotkey.ToString();

        // Create The Meele Text
        Transform meleeText = Instantiate(_loadoutTextPrefab, _loadoutTextContainerTransform);
        meleeText.GetComponent<TextMeshProUGUI>().text = "Meele: " + PlayerManager.Instance.MeleeAttackKey.ToString();

         // Create The Run Text
        Transform runText = Instantiate(_loadoutTextPrefab, _loadoutTextContainerTransform);
        runText.GetComponent<TextMeshProUGUI>().text = "Sprint: Shift";
        
    }

    private void Start() 
    {
        CreateTurretMenuButtons();
        CreateLoadoutText();
        HideBuildMenu();
    }

    public void ShowBuildCanvas()
    {
        _buildCanvas.SetActive(true);
    }

    public void HideBuildCanvas()
    {
        _buildCanvas.SetActive(false);
    }

    public void ShowBuildMenu()
    {
        _buildMenuUI.SetActive(true);
    }

    public void HideBuildMenu()
    {
        _buildMenuUI.SetActive(false);
    }

    public BuildMenu GetBuildMenuUI()
    {
        return _buildMenuUI.GetComponent<BuildMenu>();
    }

    public void ShowCombatCanvas()
    {
        _combatCanvas.SetActive(true);
    }

    public void HideCombatCanvas()
    {
        _combatCanvas.SetActive(false);
    }

    public void ShowMainCanvas()
    {
        _mainCanvas.SetActive(true);
    }

    public void HideMainCanvas()
    {
        _mainCanvas.SetActive(false);
    }

    public void ShowGameOverCanvas()
    {
        _gameOverCanvas.SetActive(true);
    }

    public void HideGameOverCanvas()
    {
        _gameOverCanvas.SetActive(false);
    }

    public void ShowMissionSucessCanvas()
    {
        _missionSucessCanvas.SetActive(true);
    }

    public void HideMissionSucessCanvas()
    {
        _missionSucessCanvas.SetActive(false);
    }

}
