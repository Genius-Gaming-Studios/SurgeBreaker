using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OverclockSelectMenu : MonoBehaviour
{
    public static OverclockSelectMenu Instance {get; private set;}

    [Header("Technical")]
    [SerializeField][Tooltip("The current mech the player has queued for selection of their loadout.")] private OverclockAbility _queuedOverclock;
    [SerializeField] private List<OverclockAbility> _fullOverclockList = new List<OverclockAbility>();

    [Header("Overclock Menu")]
    [SerializeField] private Transform _overclockUIContainer;
    [SerializeField] private Transform _abilityStatsUIContainer;
    [SerializeField] private Transform _overclockButtonUIPrefab;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else 
        {
            Debug.LogError("Only one instance of [OverclockSelectMenu] can exist in the scene!");
            Destroy(this.gameObject);
            return;
        }
    }

    public void QueueOverclock(OverclockAbility overclock)
    {
        //Queues a weapon to display its stats in the stats menu
        _queuedOverclock = overclock;
        UpdateOverclockMenu(overclock);
    }

    private void UpdateOverclockMenu(OverclockAbility ability)
    {
        // Clear any previously existing stats menu
        foreach (Transform transform in _abilityStatsUIContainer) Destroy(transform.gameObject);

        // Instantiate the corresponding stats menu
        Instantiate(ability.abilityStatsMenu, _abilityStatsUIContainer);
    }

    public void UpdateOverclockUI()
    {
        // Clear all previously existing buttons
        foreach (Transform button in _overclockUIContainer) Destroy(button.gameObject);

        // Creates & assigns the buttons in the select overeclock menu with each overclock ability 
        foreach (OverclockAbility ability in _fullOverclockList)
        {
            Transform overclockButtonTransform = Instantiate(_overclockButtonUIPrefab, _overclockUIContainer);
            OverclockButtonUI overclockButton = overclockButtonTransform.GetComponent<OverclockButtonUI>();
            overclockButton.SetupOverclockButton(ability);
        }

        // Automatically queue the first overclock in the menu when opening this menu loads
        _overclockUIContainer.GetChild(0).GetComponent<Button>().onClick.Invoke();
    }

    public void EquipOverclock()
    {
        // Updates the equipped weapon of the loadout scriptable with the currently queued mech
        StartMenu_Controller.Instance.GetEquippedLoadout().selectedAbility = _queuedOverclock;
    } 
}
