using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Mech", menuName = "Offensive/Mech", order = 1)]
public class Mech : ScriptableObject
{
    [SerializeField] [Tooltip("The prefab for the mech.")] public GameObject prefab;
    [Tooltip("Name that the game will refer to this mech as:")] public string mechName = "Mech";
    [SerializeField] [Tooltip("The maximum health of this mech.")] public int maximumHealth;

    public Sprite menuIcon;

}
