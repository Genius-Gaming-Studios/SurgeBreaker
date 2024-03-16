using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponSelectMenu : MonoBehaviour
{
    public static WeaponSelectMenu Instance {get; private set;}

    [Header("Technical")]
    [SerializeField][Tooltip("The current mech the player has queued for selection of their loadout.")] private GunSettings _queuedWeapon;
    [SerializeField] private List<GunSettings> _fullWeaponList = new List<GunSettings>();

    [Header("Weapon Menu")]
    [SerializeField] private Transform _weaponUIContainer;
    [SerializeField] private Transform _weaponButtonUIPrefab;
    

    [Header("Weapon Stats Menu")]
    //[SerializeField] private TMP_Text _flavorText;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _TypeText;
    [SerializeField] private TMP_Text _dmgText;
    [SerializeField] private TMP_Text _projectileSpeedText;
    [SerializeField] private TMP_Text _fireRateText;
    

    void Awake()
    {
        if (Instance == null) Instance = this;
        else 
        {
            Debug.LogError("Only one instance of [WeaponSelectMenu] can exist in the scene!");
            Destroy(this.gameObject);
            return;
        }
    }

    public void UpdateWeaponUI()
    {
        // Clear all previously existing buttons
        foreach (Transform button in _weaponUIContainer) Destroy(button.gameObject);

        // Creates & assigns the buttons in the select mech menu with each mech 
        foreach (GunSettings weapon in _fullWeaponList)
        {
            Transform weaponButtonTransform = Instantiate(_weaponButtonUIPrefab, _weaponUIContainer);
            weaponButtonTransform.GetChild(0).GetComponent<TextMeshProUGUI>().text = weapon.gunName;
            Button weaponButton = weaponButtonTransform.GetComponent<Button>();


            weaponButton.onClick.AddListener(() => {
                QueueWeapon(weapon);
            });
        }

        // Automatically queue the first mech in the menu when opening this menu loads
        //_mechUIContainer.GetChild(0).GetComponent<Button>().onClick.Invoke();
    }

    private void UpdateWeaponMenu()
    {
        // Updates all the text UI with the respective information of currently queued weapon

        _nameText.text = _queuedWeapon.gunName;
        //_flavorText.text = _queuedMech.flavorText;
        _TypeText.text = "[" + _queuedWeapon.weaponType.ToString() + "]";
        _dmgText.text = "[" + _queuedWeapon.damage.ToString() + "]";
        _projectileSpeedText.text = "[" + _queuedWeapon.projectileSpeed.ToString() + "]";
        _fireRateText.text = "[" + _queuedWeapon.fireRate.ToString() + "]";
    }  
 
    private void QueueWeapon(GunSettings weapon)
    {
        //Queues a weapon to display its stats in the stats menu
        _queuedWeapon = weapon;
        UpdateWeaponMenu();
    }

    public void EquipWeapon()
    {
        // Updates the equipped weapon of the loadout scriptable with the currently queued mech
        StartMenu_Controller.Instance.GetEquippedLoadout().selectedWeapon = _queuedWeapon;
    } 
}
