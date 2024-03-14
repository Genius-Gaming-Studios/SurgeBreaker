using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MechSelectMenu : MonoBehaviour
{
    public static MechSelectMenu Instance {get; private set;}

    [Header("Technical")]
    [SerializeField][Tooltip("The current mech the player has queued for selection of their loadout.")] private Mech _queuedMech;
    [SerializeField] private List<Mech> _fullMechList = new List<Mech>();

    [Header("Mech Menu")]
    [SerializeField] private Transform _mechUIContainer;
    [SerializeField] private Transform _mechButtonUIPrefab;
    

    [Header("Mech Stats Menu")]
    [SerializeField] private TMP_Text _flavorText;
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _walkSpeedText;
    [SerializeField] private TMP_Text _meleeDMGText;
    [SerializeField] private TMP_Text _meleeSpeedText;
    [SerializeField] private TMP_Text _startCurrencyText;
    

    void Awake()
    {
        if (Instance == null) Instance = this;
        else 
        {
            Debug.LogError("Only one instance of [MechSelectMenu] can exist in the scene!");
            Destroy(this.gameObject);
            return;
        }
    }

    public void UpdateMechUI()
    {
        // Creates & assigns the buttons in the select mech menu with each mech 
        foreach (Mech mech in _fullMechList)
        {
            Transform mechButtonTransform = Instantiate(_mechButtonUIPrefab, _mechUIContainer);
            Button mechButton = mechButtonTransform.GetComponent<Button>();
            mechButton.image.sprite = mech.menuIcon;

            mechButton.onClick.AddListener(() => {
                QueueMech(mech);
            });
        }
    }

    private void UpdateMechStatsMenu()
    {
        // Updates all the text UI with the respective information of currently queued mech

        _nameText.text = _queuedMech.mechName;
        _flavorText.text = _queuedMech.flavorText;
        _healthText.text = "ARMOR INTEGRITY [" + _queuedMech.maximumHealth.ToString() + "]";
        _walkSpeedText.text = "SERVOS [" + _queuedMech.walkSpeed.ToString() + "]";
        _meleeDMGText.text = "HYDRAULIC STR [" + _queuedMech.meleeDamage.ToString() + "]";
        _meleeSpeedText.text = "HYDRAULIC SPEED [" + _queuedMech.meleeSpeed.ToString() + "]";
        _startCurrencyText.text = "START $ [" + _queuedMech.startCurrency.ToString() + "]";
    }

    private void QueueMech(Mech mech)
    {
        //Queues a mech to display its stats in the stats menu
        _queuedMech = mech;
        UpdateMechStatsMenu();
    }
}
