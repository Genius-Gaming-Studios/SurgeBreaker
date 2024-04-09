/* Mark Gaskins
 * THIS IS A MULTI-INSTANCE SCRIPT!
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBullet : MonoBehaviour
{

    private Transform target;


    [Header("Attributes")]
    [Tooltip("The speed that the bullet travels.")] [SerializeField] [Range(10, 150)] float speed = 70.0f;
    [Tooltip("The damage that the bullet does.")] [SerializeField] [Range(2, 1000)] public int power = 50;
    [Tooltip("The tag of the walls, or any object that the bullet can not go through.")] [SerializeField] string _solidObjectTag = "Can Stop Bullets";
    [Tooltip("The radius of the object collision checking.")] [SerializeField] float checkingRange = 1.0f;
    [Tooltip("The SFX that will play when the bullet hits.")] [SerializeField] GameObject hitSFX; // Yes, Super lazy way of doing it but whatever

    [Header("Power ups")]
    [Tooltip("Should this bullet do slowing on enemies?")] [SerializeField] public bool doSlowing;
    [Tooltip("How long should enemies be slowed for?")] [SerializeField] [Range(0.3f, 2.5f)] public float slowTime = 1.0f;
    [Tooltip("What is the speed that enemies should be slowed to?")] [SerializeField] [Range(1.0f, 5.0f)] public float slowTo = 2.0f;

    [Space(10)]
    [Tooltip("Should this bullet explode when it hits the target?")] public bool doExplosion = false;
    [Tooltip("[GIZMO] Explosion radius of the bullet when it hits the target.")] [Range(3.5f, 47.5f)] [SerializeField] float explosionRange = 6f;
    [Tooltip("The FX that will appear when the bullet reaches the target.")] [SerializeField] GameObject ExplosionFX;
    [Tooltip("The SFX that will play when the bullet explodes.")] [SerializeField] AudioClip ExplosionSFX;

    [Tooltip("The multiplier of the damage given to the ones in the center [Ring A] of the explosion. (DEFAULT: 1.3)")] [SerializeField] [Range(1.0f, 1.9f)] float centerDamageMultiplier = 1.3f;
    [Tooltip("[Read Only] This is a preview. This is the maximum amount of damage that anyone in the radius will experience.")] [SerializeField] [Range(0, 475)] private int maxDamagePreview;

    public void Seek(Transform _target)
    {
        target = _target;
    }

    private void Update()
    {
        if (target == null) { Destroy(gameObject); return; } // Destroy bullet if no target

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target);

        // Checks to see when it should delete the bullet after being too close to an object (after colliding with a wall)
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, checkingRange);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag(_solidObjectTag))
            {
                // BULLET JUICE ~~
                GameObject FX = Instantiate(hitSFX);
                FX.transform.position = this.transform.position;
                Destroy(FX, .9f);

                Destroy(this.gameObject);
            }
        }

    }

    private void HitTarget()
    {
        // BULLET JUICE ~~
        GameObject FX = Instantiate(hitSFX);
        FX.transform.position = this.transform.position;
        Destroy(FX, .9f);

        Damage(target);
        Destroy(target.gameObject, 0.3f);
    }

    void Damage(Transform enemy)
    {

        Enemy e = enemy.GetComponent<Enemy>();


        if (e != null)
        {
            if (!doExplosion) /// Singular Enemy Hit
            {
                /// Register singular enemy slowing, if this is a slower bullet.
                if (doSlowing)
                {
                    if (e.SlowingCoroutine != null) e.StopCoroutine(e.SlowingCoroutine);
                    e.SlowingCoroutine = e.StartCoroutine(e.SlowEnemy(slowTo, slowTime));
                }

                e.GetComponent<Health>().Damage(power);
            }
            else /// Explosion bullet AOE mechanics [1.9.5a]
            {
                GameObject explosionFX = Instantiate(ExplosionFX, transform.position, Quaternion.identity); // Instantiates the explosion FX where the bullet currently is.
                FindObjectOfType<GameManager>().CoreFXPlayer.PlayOneShot(ExplosionSFX);

                // First damage the enemy in the center of the AOE in order to raise the stakes of being the one in the center of the attack.
                e.GetComponent<Health>().Damage(power / 3);

                // Find all enemies within the range of the blast 
                GameObject[] enemiesInRange = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (GameObject _enemy in enemiesInRange)
                {
                    // Fill values and identify the rings of damage
                    float distanceFromCenter = Vector3.Distance(transform.position, _enemy.transform.position);
                    float perimeterA = explosionRange / 4; /// Closest - Most Damaged
                    float perimeterB = (explosionRange / 4) * 2; /// Second Closest
                    float perimeterC = (explosionRange / 4) * 3; /// Third Closest
                    float perimeterD = explosionRange; /// Furthest - Least Damged

                    /// Damage all enemies in range, depending on how close they were to the target.
                    if (distanceFromCenter <= perimeterA) // First ring of damage [A]
                        _enemy.GetComponent<Health>().Damage(Mathf.RoundToInt(power * centerDamageMultiplier)); // This damage is multiplied by the centerDamageMultiplier to ensure that the ones closest to the center are hit harder than the rest.
                    else if (distanceFromCenter <= perimeterB && distanceFromCenter >= perimeterA) // Second ring of damage [B]
                        _enemy.GetComponent<Health>().Damage(power);
                    else if (distanceFromCenter <= perimeterC && distanceFromCenter >= perimeterB) // Third ring of damage [C]
                        _enemy.GetComponent<Health>().Damage(Mathf.RoundToInt(power - power / 4));
                    else if (distanceFromCenter <= perimeterD && distanceFromCenter >= perimeterC) // Fourth ring of damage [D]
                        _enemy.GetComponent<Health>().Damage(Mathf.RoundToInt(power - power / 2));

                    // Do slowing on enemies in A and B ring ------------
                    /// Register enemy slowing, if this is a slower bullet.
                    if (doSlowing)
                    {
                        if (distanceFromCenter <= perimeterB) // Only For: Perimeters [A] and [B]
                        {
                            if (_enemy.GetComponent<Enemy>().SlowingCoroutine != null) _enemy.GetComponent<Enemy>().StopCoroutine(_enemy.GetComponent<Enemy>().SlowingCoroutine);
                            _enemy.GetComponent<Enemy>().SlowingCoroutine = _enemy.GetComponent<Enemy>().StartCoroutine(_enemy.GetComponent<Enemy>().SlowEnemy(slowTo, slowTime));
                        }
                    }
                }


                Destroy(explosionFX, 3);  // Destroys the explosion FX after 3 seconds of delay.
                Destroy(this.gameObject, 1); // Destroy the bullet when the explosion is complete.
            }


        }
        else
        {
            if (enemy.GetComponent<PlayerManager>() != null) // Heal
                enemy.GetComponent<Health>().Damage(-power);
            else
                Debug.Log("This isn't an enemy or a player.");
        }



        Destroy(gameObject);
    }

    public void OnDrawGizmosSelected()
    {
        /// Shows the explosion radius when gizmos are selected.
        if (doExplosion)
        {
            Gizmos.DrawWireSphere(transform.position, explosionRange);
        }

        /// <summary>
        /// Editor function to update the preview according to the damage values.
        /// </summary>
        maxDamagePreview = Mathf.RoundToInt(power * centerDamageMultiplier);
    }
}
