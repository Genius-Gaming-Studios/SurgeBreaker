/* Mark Gaskins
 */

using UnityEngine;

public enum WeaponType
{
    Bullet,
    Energy,
    Explosive,
        // Add more weapon types as needed
}

[CreateAssetMenu(fileName = "Gun", menuName = "Offensive/Basic Gun", order = 1)]
public class GunSettings : ScriptableObject
{

    [Header("Gun Settings")]
    [Tooltip("Name that the game will refer to this gun as:")] public string gunName = "Basic Gun";
    [Tooltip("The type of weapon this gun is.")]public WeaponType weaponType = WeaponType.Bullet;
    [Tooltip("The amount of damage each projectiles of this weapon deals.")][SerializeField] public float damage = 1f;
    [Tooltip("The speed of this weapon's projectile")][Range(10, 150)] public float projectileSpeed = 1f;
    [Tooltip("Rate in which the gun fires bullets.")][SerializeField] [Range(0.1f, 8.0f)] public float fireRate = 1;
    [Tooltip("The sound that can be heard when the gun is fired.")][SerializeField] public AudioClip fireSound;
    
    

    public GameObject bulletToFire;

}