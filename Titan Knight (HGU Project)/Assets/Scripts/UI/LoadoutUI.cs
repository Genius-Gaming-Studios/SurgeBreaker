using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadoutUI : MonoBehaviour
{
    public static LoadoutUI Instance {get; private set;}

    [SerializeField] private GameObject _equippedLoadoutMenu;
    [SerializeField] private GameObject _mechSelectMenu;
    [SerializeField] private GameObject _weaponSelectMenu;
    [SerializeField] private GameObject _overclockSelectMenu;

    [Header("EQUIPPED LOADOUT MENU")]  

    [Header("Mech")]
    [SerializeField][Tooltip("The prefab of the mech button")] private Transform _mechButton;
    [SerializeField][Tooltip("The transform the mech model will be spawned a child of in the loadout menu")] private Transform _mechModelTransform;

    [Header("Weapon")]
    [SerializeField][Tooltip("The prefab of the weapon button")] private Transform _weaponButton;

    [Header("Turrets")]
    [SerializeField][Tooltip("")] private Transform _turretUIContainerTransform;

    [Header("Overclock Ability")]
    [SerializeField][Tooltip("The prefab of the overclock ability button")] private Transform _overclockAbilityButton;




    void Awake()
    {
        if (Instance == null) Instance = this;
        else 
        {
            Debug.LogError("Only one instance of [LoadoutUI] can exist in the scene!");
            Destroy(this.gameObject);
            return;
        }
    }

    // When this UI is loaded, it accesses the current player loadout, then populates the loadout UI with the respective mech, turrets, and overclock abilities
    private void UpdateEquipedLoadoutUI()
    {
        // Hide all other loadout submenus
        _mechSelectMenu.SetActive(false);
        _weaponSelectMenu.SetActive(false);

        //Clear the old mech model & instantiate the model that is currently equipped
        if (_mechModelTransform.childCount > 0) Destroy(_mechModelTransform.GetChild(0).gameObject);
        Instantiate(StartMenu_Controller.Instance.GetEquippedLoadout().selectedMech.prefab, _mechModelTransform, false);

        // Populate the Mech UI
        MechButtonUI mechButtonUI = _mechButton.GetComponent<MechButtonUI>();
        mechButtonUI.SetupMechButton(StartMenu_Controller.Instance.GetEquippedLoadout().selectedMech);

        // Populate the Weapon UI
        WeaponButtonUI weaponButtonUI = _weaponButton.GetComponent<WeaponButtonUI>();
        weaponButtonUI.SetupWeaponButton(StartMenu_Controller.Instance.GetEquippedLoadout().selectedWeapon);

        int index = 0;
        //Populate the turret UI
        foreach (Transform button in _turretUIContainerTransform)
        {
            TurretButtonUI turretButtonUI = button.GetComponent<TurretButtonUI>();
            turretButtonUI.SetBaseTurret(StartMenu_Controller.Instance.GetEquippedLoadout().selectedTurrets[index], false);
            index++; 
        }

        // Populate Overclock UI
        OverclockButtonUI overclockButtonUI = _overclockAbilityButton.GetComponent<OverclockButtonUI>();
        overclockButtonUI.SetupOverclockButton(StartMenu_Controller.Instance.GetEquippedLoadout().selectedAbility);
    }

    public void OpenMechSelectMenu()
    {
        _equippedLoadoutMenu.SetActive(false);
        _mechSelectMenu.SetActive(true); 
        _weaponSelectMenu.SetActive(false);
        _overclockSelectMenu.SetActive(false);

        MechSelectMenu.Instance.UpdateMechUI();
    }

    public void OpenWeaponSelectMenu()
    {
        _equippedLoadoutMenu.SetActive(false);
        _mechSelectMenu.SetActive(false); 
        _weaponSelectMenu.SetActive(true);
        _overclockSelectMenu.SetActive(false);

        WeaponSelectMenu.Instance.UpdateWeaponUI();
    }

    public void OpenOverclockSelectMenu()
    {
        _equippedLoadoutMenu.SetActive(false);
        _mechSelectMenu.SetActive(false); 
        _weaponSelectMenu.SetActive(false);
        _overclockSelectMenu.SetActive(true);

        OverclockSelectMenu.Instance.UpdateOverclockUI();
    }

    public void OpenEquippedLoadoutMenu()
    {
        _equippedLoadoutMenu.SetActive(true);
        _mechSelectMenu.SetActive(false); 
        _overclockSelectMenu.SetActive(false);
        UpdateEquipedLoadoutUI();
    }
}
