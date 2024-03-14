using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Mech", menuName = "Offensive/Mech", order = 1)]
public class Mech : ScriptableObject
{
    [Tooltip("The prefab for the mech.")] public GameObject prefab;
    [Tooltip("Name that the game will refer to this mech as:")] public string mechName = "Mech";
    [Tooltip("The maximum health of this mech.")] public int maximumHealth;
    [Tooltip("The move speed of this mech.")] public float walkSpeed;
    [Tooltip("The time (seconds) between each melee attack that the player may operate.")] public float meleeSpeed;
    [Tooltip("The amount of melee DMG dealt per melee hit.")] public int meleeDamage;
    [Tooltip("The amount of currency that the player starts with.")] public int startCurrency;

    [Tooltip("The flavor text to appear in the mech selection menu")] public string flavorText;

    public Sprite menuIcon;

}
