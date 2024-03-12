using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponButtonUI : MonoBehaviour
{

    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private Image _Icon;

    public void SetupWeaponButton(GunSettings gun)
    { 
        _nameText.text = gun.gunName;
        //_mechIcon.sprite = ADD SPRITES FOR OVERCLOCK ABILITIES

        // Assign this button the proper onClick functionality
        /*_button.onClick.AddListener(() => {
            BuildMenu.Instance.BuildButtonPressed(turret);
        }); */

    }
}
