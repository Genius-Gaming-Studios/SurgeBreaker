/* Mark Gaskins
 * THIS IS A MULTI-INSTANCE SCRIPT!
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    //[Tooltip("This will modify the values of this gun.")] public GunSettings gunSettings;
    public Loadout loadout;
    [SerializeField] Transform firePoint;
    [SerializeField] Transform fireDis;
    //[SerializeField] GameObject FxObject;
    [SerializeField] GameObject bulletTargetPrefab;
    [HideInInspector] Vector3 __fireDisPos;
    [SerializeField] bool doOverrideFirePos = true;

    [SerializeField] AudioSource _audioSource;

    private float timeToFire = 1;

    // OVERCLOCK OVERRIDES
    [HideInInspector] public float overrideFireRate;
    [HideInInspector] public float overrideDamageBoost;

    private bool doFire; // True when the gun is firing.
    private AudioSource coreFXPlayer;

    private void OnEnable()
    {
        coreFXPlayer = FindObjectOfType<GameManager>().CoreFXPlayer;

        if (doOverrideFirePos) fireDis.localPosition = __fireDisPos;

        __fireDisPos = new Vector3(-8f, 1.1f, 72.8f);

        //Debug.Log($"[Gun] {name} using {gunSettings.name}, sound: {gunSettings.fireSound.name}");

    }

    private void Start()
    {
        // Loadout Initializion
        loadout = GameManager.Instance.loadout;

        // Audio Initialization
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = FindObjectOfType<UniversalPreferences>()._fxVolume;
        _audioSource.clip = GameManager.Instance.loadout.selectedWeapon.fireSound;
    }
        private void FixedUpdate()
    {

        if (timeToFire >= 0) timeToFire -= Time.deltaTime * (1 / (overrideFireRate == 0 ? GameManager.Instance.loadout.selectedWeapon.fireRate : overrideFireRate)); // Automatically reduce the time till the next bullet even though the gun is being spammed

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

        if (GameManager.Instance.currentMode == GameMode.Idle) return; // Do nothing while the game mode is "Idle"

        timeToFire = 1;

        // Spawn a bullet (because it's cooler seeing a real bullet object, instead of an invisible bullet)
        GameObject firedBullet = Instantiate(loadout.selectedWeapon.bulletToFire, firePoint.position, Quaternion.identity); // Spawn the bullet

        // OVERRIDE DAMAGE BOOST (OVC)
        if (overrideDamageBoost != 0) firedBullet.GetComponent<Bullet>().damage = Mathf.RoundToInt(GameManager.Instance.loadout.selectedWeapon.damage * overrideDamageBoost);

        GameObject target = Instantiate(bulletTargetPrefab, fireDis.position, Quaternion.identity); // Spawn the despawn target of the bullet


        firedBullet.GetComponent<Bullet>().target = target.transform; // Assign the despawn target of the bullet
        firedBullet.GetComponent<Bullet>().speed = GameManager.Instance.loadout.selectedWeapon.projectileSpeed; // Assign the despawn target of the bullet
        firedBullet.GetComponent<Bullet>().damage = Mathf.RoundToInt(GameManager.Instance.loadout.selectedWeapon.damage); // Assign the damage which the bullet does as the one from the loadout

        _audioSource.Play();
    }

}
