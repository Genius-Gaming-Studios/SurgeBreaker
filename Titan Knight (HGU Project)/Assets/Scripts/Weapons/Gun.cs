/* Mark Gaskins
 * THIS IS A MULTI-INSTANCE SCRIPT!
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Tooltip("This will modify the values of this gun.")] public GunSettings gunSettings;
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject bulletTargetPrefab;
    [SerializeField] Transform fireDis;

    private float timeToFire = 1;

    private void FixedUpdate()
    {
        
        if (Input.GetMouseButton(0))
        {
            timeToFire -= Time.deltaTime * (1/gunSettings.fireRate);

            if (timeToFire <= 0)
                Fire();
        }

        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }

        if (Input.GetMouseButtonUp(0)) timeToFire = 1;
    }

    private void Fire()
    {
        timeToFire = 1;

        // Spawn a bullet (because it's cooler seeing a real bullet object, instead of an invisible bullet)
        GameObject firedBullet = (GameObject)Instantiate(gunSettings.bulletToFire, firePoint.position, Quaternion.identity); // Spawn the bullet


        /* Problem:*/ GameObject target = Instantiate(bulletTargetPrefab, fireDis.position, Quaternion.identity); // Spawn the despawn target of the bullet


        firedBullet.GetComponent<Bullet>().target = target.transform; // Assign the despawn target of the bullet

    }
}
