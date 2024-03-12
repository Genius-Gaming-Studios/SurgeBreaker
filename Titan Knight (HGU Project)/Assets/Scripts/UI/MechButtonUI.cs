using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MechButtonUI : MonoBehaviour
{

    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private Image _mechIcon;

    public void SetupMechButton(Mech mech)
    { 
        //Assigns this button to the type of turret it is for
        _nameText.text = mech.mechName;
        _mechIcon.sprite = mech.menuIcon;

        // Assign this button the proper onClick functionality
        /*_button.onClick.AddListener(() => {
            BuildMenu.Instance.BuildButtonPressed(turret);
        }); */

    }
}
