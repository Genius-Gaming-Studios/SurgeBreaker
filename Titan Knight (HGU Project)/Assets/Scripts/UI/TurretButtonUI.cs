using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurretButtonUI : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _buttonText;

    [SerializeField] private Image _turretIcon;

    private int _turretLoadoutIndex = 0; // A private int to track which turret to instantiate & program 
    

    public void SetBaseTurret(Turret turret)
    {
        //Assigns this button to the type of turret it is for
        _buttonText.text = turret.turretName;
        _turretIcon.sprite = turret.menuIcon;

        // Assign this button the proper onClick functionality
        _button.onClick.AddListener(() => {
            BuildMenu.Instance.BuildButtonPressed(GameManager.Instance.loadout.selectedTurrets[_turretLoadoutIndex]);
            _turretLoadoutIndex ++;
        });
    }
}
