using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OverclockButtonUI : MonoBehaviour
{

    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private Image _Icon;
    [SerializeField] private GameObject _abilityStatsMenu;

    public void SetupOverclockButton(OverclockAbility ability)
    { 
        _nameText.text = ability.abilityName;

        // Assign this button the proper onClick functionality
        _button.onClick.AddListener(() => {
            OverclockSelectMenu.Instance.QueueOverclock(ability);
        }); 

    }
}
