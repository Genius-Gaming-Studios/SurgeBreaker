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
    [HideInInspector] Vector3 __fireDisPos;
    [SerializeField] bool doOverrideFirePos = true;

    private float timeToFire = 1;

    private bool doFire; // True when the gun is firing.
    private AudioSource coreFXPlayer;

    private void OnEnable()
    {
        coreFXPlayer = FindObjectOfType<GameManager>().CoreFXPlayer;

        if (doOverrideFirePos) fireDis.localPosition = __fireDisPos;

         __fireDisPos = new Vector3(-8f, 1.1f, 72.8f);
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
        if (GameManager.hasWon) return; // Player will not be allowed to be controlled if they have already won.

        timeToFire = 1;

        // Spawn a bullet (because it's cooler seeing a real bullet object, instead of an invisible bullet)
        GameObject firedBullet = (GameObject)Instantiate(gunSettings.bulletToFire, firePoint.position, Quaternion.identity); // Spawn the bullet


        /* Problem:*/
        GameObject target = Instantiate(bulletTargetPrefab, fireDis.position, Quaternion.identity); // Spawn the despawn target of the bullet


        firedBullet.GetComponent<Bullet>().target = target.transform; // Assign the despawn target of the bullet

        // Instantiate a sound object in order to give it a custom pitch
        GameObject soundObject = Instantiate(FxObject, this.gameObject.transform);
        AudioSource audioSource = soundObject.GetComponent<AudioSource>();
        // audioSource.pitch = Random.Range(.9f, 1.1f); // no
        audioSource.volume = FindObjectOfType<UniversalPreferences>()._fxVolume;
        audioSource.clip = fireSound;
        audioSource.Play();
        Destroy(soundObject, fireSound.length);

    }
}
