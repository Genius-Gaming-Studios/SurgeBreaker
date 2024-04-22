using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurretSelectMenu : MonoBehaviour
{
    public static TurretSelectMenu Instance {get; private set;}

    [Header("Technical")]
    [SerializeField][Tooltip("The current turret the player has queued for selection of their loadout.")] private Turret _queuedTurret;
    [SerializeField] private List<Turret> _fullTurretList = new List<Turret>();
    [SerializeField] private int _loadoutIndex = 0;

    [Header("Turret Menu")]
    [SerializeField] private Transform _TurretUIContainer;
    [SerializeField] private Transform _TurretButtonUIPrefab;

    [Header("Turret Stats Menu")]
    //[SerializeField] private TMP_Text _flavorText;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _costText;
    [SerializeField] private TMP_Text _dmgText;
    [SerializeField] private TMP_Text _projectileSpeedText;
    [SerializeField] private TMP_Text _fireRateText;
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Only one instance of [TurretSelectMenu] can exist in the scene!");
            Destroy(this.gameObject);
            return;
        }

        Instance = this; 
    }

    public void UpdateTurretUI()
    {
        // Clear all previously existing buttons
        foreach (Transform button in _TurretUIContainer) Destroy(button.gameObject);

        int loadoutIndex = 0;
        // Creates & assigns the buttons in the select mech menu with each mech 
        foreach (Turret turret in _fullTurretList)
        {
            Transform turretButtonTransform = Instantiate(_TurretButtonUIPrefab, _TurretUIContainer);
            Button turretButton = turretButtonTransform.GetComponent<Button>();
            TurretButtonUI buttonUI = turretButtonTransform.GetComponent<TurretButtonUI>();
            buttonUI.SetBaseTurret(turret, false);

            turretButton.onClick.AddListener(() => {
                QueueTurret(turret);
            });
        }

        // Automatically queue the first turret in the menu when opening this menu loads
        _TurretUIContainer.GetChild(0).GetComponent<Button>().onClick.Invoke();
    }
    private void UpdateTurretStatsMenu()
    {
        // Updates all the text UI with the respective information of currently queued weapon

        _nameText.text = _queuedTurret.turretName;
        //_flavorText.text = _queuedMech.flavorText;
        _costText.text = "Cost: [" + _queuedTurret.cost.ToString() + "]";
        _dmgText.text = "DMG: [" + _queuedTurret.projectileDMG.ToString() + "]";
        _projectileSpeedText.text = "Bullet Speed: [" + _queuedTurret.projectileSpeed.ToString() + "]";
        _fireRateText.text = "Fire Rate: [" + _queuedTurret.fireRate.ToString() + "]";
    }

    private void QueueTurret(Turret turret)
    {
        //Queues a turret to display its stats in the stats menu
        _queuedTurret = turret;
        UpdateTurretStatsMenu();
    }

    public void EquipTurret()
    {
        // Updates the equipped weapon of the loadout scriptable with the currently queued mech
        StartMenu_Controller.Instance.GetEquippedLoadout().selectedTurrets[LoadoutUI.Instance.queuedTurretButtonSlot.index] = _queuedTurret;
    } 

      
}
