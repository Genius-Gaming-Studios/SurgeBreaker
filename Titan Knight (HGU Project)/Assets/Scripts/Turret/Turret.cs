using UnityEngine;

[CreateAssetMenu(fileName = "Turret", menuName = "Offensive/Basic Turret", order = 1)]
public class Turret : ScriptableObject
{
    [SerializeField] [Tooltip("This is the Turret's prefab that will spawn in when the player builds.")] public GameObject prefab;
    [Tooltip("Name that the game will refer to this turret as:")] public string turretName = "Turret";

    private int cost;
}
