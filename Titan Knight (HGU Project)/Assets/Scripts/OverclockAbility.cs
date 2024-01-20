using UnityEngine;

[CreateAssetMenu(fileName = "Overclock Ability", menuName = "Offensive/Base Overclock")]
public class OverclockAbility : ScriptableObject
{

    [Tooltip("Name that the game will refer to this ability as:")] public string abilityName;
    public float duration;
    

}