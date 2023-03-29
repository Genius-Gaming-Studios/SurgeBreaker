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
    [SerializeField] GameObject FxObject;
    [Tooltip("The sound that can be heard when the gun is fired.")][SerializeField] AudioClip fireSound;

    private float timeToFire = 1;

    private bool doFire; // True when the gun is firing.
    private AudioSource coreFXPlayer;

    private void OnEnable()
    {
        coreFXPlayer = FindObjectOfType<GameManager>().CoreFXPlayer;
    }
    private void FixedUpdate()
    {
        
        if (timeToFire >= 0) timeToFire -= Time.deltaTime * (1 / gunSettings.fireRate); // Automatically reduce the time till the next bullet even though the gun is being spammed

        if (doFire && timeToFire <= 0) Fire();

        if (Input.GetMouseButton(0))
        {
            doFire = true;
        }
        else
        {
            doFire = false;
        }

    }

    private void Fire()
    {
        timeToFire = 1;

        // Spawn a bullet (because it's cooler seeing a real bullet object, instead of an invisible bullet)
        GameObject firedBullet = (GameObject)Instantiate(gunSettings.bulletToFire, firePoint.position, Quaternion.identity); // Spawn the bullet


        /* Problem:*/
        GameObject target = Instantiate(bulletTargetPrefab, fireDis.position, Quaternion.identity); // Spawn the despawn target of the bullet


        firedBullet.GetComponent<Bullet>().target = target.transform; // Assign the despawn target of the bullet

        // Instantiate a sound object in order to give it a custom pitch
        GameObject soundObject = Instantiate(FxObject, coreFXPlayer.gameObject.transform);
        AudioSource audioSource = soundObject.GetComponent<AudioSource>();
        audioSource.pitch = Random.Range(.7f, .9f);
        audioSource.volume = 0.25f;
        audioSource.clip = fireSound;
        audioSource.Play();
        Destroy(soundObject, fireSound.length);

    }
}
