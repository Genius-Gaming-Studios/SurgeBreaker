/* MARK GASKINS
 * Genius Studios 2023
 * 
 */

using UnityEngine;

public enum TurretTypes
{
    Bullet,
    Laser
}


public class TurretManager : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("(Important) The type of this turret.")] [SerializeField] public TurretTypes TurretType; // Very important, decides how the entire turret will behave.

    [Header("General")]
    [Tooltip("The range in which the turret can reach enemies from.")] [SerializeField] [Range(75, 9999)] public int turretCost = 150;

    [Tooltip("The range in which the turret can reach enemies from.")] [SerializeField] [Range(3, 100)] public float range = 15f;
    [Tooltip("The tip of the turret that its bullets will fire from.")] [SerializeField] Transform firePoint;

    [Tooltip("The turn speed of the hinge. Doesn't affect how fast the turret fires.")] [SerializeField] [Range(0, 30)] float turnSpeed = 10f;
    [Tooltip("The tag of the enemy. Ensure that all enemies have this tag name assigned to them.")] [SerializeField] string enemyTag = "Enemy";
    [Tooltip("The part that contains the fire animation.")] [SerializeField] Animation fireAnimation;

    public Turret turretSettings; 

    [Header("Bullet Preferences")]

    [Tooltip("The head part of the turret. This will turn to face its target, and should be the parent of the head of the turret.")][SerializeField] Transform Hinge;
    [Tooltip("The 'TurretBullet' prefab that spawns each time an enemy is in range of the Turret.")][SerializeField] GameObject BulletPrefab;

    [Tooltip("The speed in which the Turret fires bullets.")] [SerializeField] [Range(0, 15)] float fireRate = 1f;

    [SerializeField] GameObject FxObject;
    [Tooltip("The sound that can be heard when the gun is fired.")] [SerializeField] AudioClip fireSound;
    [Tooltip("The muzzle flash for the gun.")] [SerializeField] GameObject muzzleFlash;


    [Header("Laser Preferences")]
    [SerializeField] LineRenderer laserRenderer;
    [Tooltip("The speed in which the enemy will take damage at.")] [SerializeField] [Range(0, 15)] float damageRate = 1f;
    [Tooltip("The amount of damage the enemy will take every damageRate.")] [SerializeField] [Range(3, 150)] int laserPower = 1;


    private float fireCountdown;
    private float mzwaitTime = 1f;
    private float mzTimer = 0.0f;
 
    private Transform target;
    private Enemy targetEnemy;
    private AudioSource coreFXPlayer;
    
    private void Start()
    {

        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        coreFXPlayer = FindObjectOfType<GameManager>().CoreFXPlayer;
        
        if (turretSettings == null) Debug.LogWarningFormat("Please attatch the turret settings to a Turret in this scene in order to utilize the Selling features.");

        mzwaitTime = fireRate;
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range) // This targets the nearest enemy, but can be modified later to target the enemy with the most power, HP, etc..
        {
            target = nearestEnemy.transform;
            targetEnemy = nearestEnemy.GetComponent<Enemy>();
        }
        else
        {
            target = null;
        }

    }

    private void FixedUpdate()
    {
        if (target == null)
        {
            if (TurretType == TurretTypes.Laser)
            {
                laserRenderer.enabled = false; // Hides the laser if there is no target in range of the turret.
                laserRenderer.GetComponent<AudioSource>().enabled = false;
            }

            return;
        }
        else if (TurretType == TurretTypes.Laser)
        { // Re-enable graphic and audio
            laserRenderer.GetComponent<AudioSource>().enabled = true;
            laserRenderer.enabled = true;
        }

        if (FindObjectOfType<GameManager>().currentMode != GameMode.Combat) return; // Ensure that it's in combat mode fire

        LockOnTarget();

        if (TurretType == TurretTypes.Bullet)
        {
            if (fireCountdown <= 0f)
            {

                Fire();

                fireCountdown = 1f / fireRate;
            }

            fireCountdown -= Time.deltaTime;
        }
        else if (TurretType == TurretTypes.Laser)
        {
            LaserFire();

            fireCountdown -= Time.deltaTime;
        }
    }


    void LockOnTarget()
    {
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(Hinge.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        Hinge.rotation = Quaternion.Euler(0f, rotation.y, 0f);

    }

    private void LaserFire()
    {
        /// Locks the laser onto the target. 
        laserRenderer.SetPosition(0, firePoint.position);
        laserRenderer.SetPosition(1, target.position);

        if (fireCountdown <= 0f)
        {
            target.gameObject.GetComponent<Health>().Damage(laserPower);

            fireCountdown = 1f / damageRate;
        }
    }

    private void Fire()
    {
        // Reset the fire animation.
        fireAnimation.Stop();

        GameObject bulletObject = (GameObject)Instantiate(BulletPrefab, firePoint.position, firePoint.rotation);
        TurretBullet bullet = bulletObject.GetComponent<TurretBullet>();

        if (bullet != null)
        {
            bullet.Seek(target);
        }

       //  muzzleFlash.SetActive(true);
        // mzTimer = 0f;

        // muzzle flash
        GameObject flashObject = Instantiate(muzzleFlash, firePoint.position, firePoint.rotation);

        // Instantiate a sound object in order to give it a custom pitch
        GameObject soundObject = Instantiate(FxObject, this.gameObject.transform);
        AudioSource audioSource = soundObject.GetComponent<AudioSource>();
        // audioSource.pitch = Random.Range(1.1f, 1.3f); // sounds so bad lol
        audioSource.volume = FindObjectOfType<UniversalPreferences>()._fxVolume;
        audioSource.clip = fireSound;
        audioSource.Play();
        Destroy(soundObject, fireSound.length);
        Destroy(flashObject, fireSound.length);

        // Play the fire animation.
        fireAnimation.Play();
    }

    private void OnDrawGizmosSelected() // Shows the range of the turret's bullets with a red gizmo. (Ensure Gizmos are enabled in the editor)
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
