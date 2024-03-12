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

    public void SetupOverclockButton(OverclockAbility ability)
    { 
        //Assigns this button to the type of turret it is for
        _nameText.text = ability.abilityName;
        //_mechIcon.sprite = ADD SPRITES FOR OVERCLOCK ABILITIES

        // Assign this button the proper onClick functionality
        /*_button.onClick.AddListener(() => {
            BuildMenu.Instance.BuildButtonPressed(turret);
        }); */

    }
}
