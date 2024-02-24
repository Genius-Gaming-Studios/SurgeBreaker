using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurretButtonUI : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _nameText;

    [SerializeField] private TextMeshProUGUI _costText;
    [SerializeField] private Image _turretIcon;
    

    public void SetBaseTurret(Turret turret)
    {
        //Assigns this button to the type of turret it is for
        _nameText.text = turret.turretName;
        _costText.text = turret.cost.ToString();
        _turretIcon.sprite = turret.menuIcon;

        // Assign this button the proper onClick functionality
        _button.onClick.AddListener(() => {
            BuildMenu.Instance.BuildButtonPressed(turret);
        });

    }
}
