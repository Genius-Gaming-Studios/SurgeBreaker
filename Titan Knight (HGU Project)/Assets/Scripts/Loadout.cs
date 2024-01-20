using UnityEngine;

[CreateAssetMenu(fileName = "Loadout", menuName = "Loadout")]
public class Loadout :  ScriptableObject
{
    // This class represents the currently selected loadout of the player. 
    // A loadout consists of a mech, weapon, overclock ability, and 3 turrets

    [Tooltip("The selected mech weapon in this loadout")][SerializeField] public GunSettings selectedWeapon;
    [Tooltip("The type of weapon this gun is.")][SerializeField]  public OverclockAbility selectedAbility;
    public Turret[] selectedTurrets = new Turret[3];
}
