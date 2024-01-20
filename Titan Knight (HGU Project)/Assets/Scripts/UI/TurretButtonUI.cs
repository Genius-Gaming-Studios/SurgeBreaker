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
    

    public void SetBaseTurret(Turret turret)
    {
        //Assigns this button to the type of turret it is for
        _buttonText.text = turret.turretName;
        _turretIcon.sprite = turret.menuIcon;
    }
}
