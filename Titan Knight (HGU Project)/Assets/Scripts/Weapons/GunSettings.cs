/* Mark Gaskins
 */

using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Weapons/Basic Gun", order = 1)]
public class GunSettings : ScriptableObject
{

    [Header("Gun Settings")]
    [Tooltip("Rate in which the gun fires bullets.")][SerializeField] [Range(0.1f, 2.0f)] public float fireRate = 1;
    [Tooltip("Name that the game will refer to this gun as:")] public string gunName = "Basic Gun";
    //[Tooltip("How far can this gun's bullets go until they despawn?")] [SerializeField] [Range(1f, 100)] public int bulletDespawnDistance = 35;

    public GameObject bulletToFire;

}