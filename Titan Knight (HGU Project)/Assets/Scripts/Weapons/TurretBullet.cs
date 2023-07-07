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
    [Tooltip("The damage that the bullet does.")] [SerializeField] [Range(2, 1000)] int power = 50;
    [Tooltip("The tag of the walls, or any object that the bullet can not go through.")] [SerializeField] string _solidObjectTag = "Can Stop Bullets";
    [Tooltip("The radius of the object collision checking.")] [SerializeField] float checkingRange = 1.0f;

    [Header("Power ups")]
    [Tooltip("Should this bullet do slowing on enemies?")] [SerializeField] public bool doSlowing;
    [Tooltip("How long should enemies be slowed for?")] [SerializeField] [Range(0.3f, 2.5f)] public float slowTime = 1.0f;
    [Tooltip("What is the speed that enemies should be slowed to?")] [SerializeField] [Range(1.0f, 5.0f)] public float slowTo = 2.0f;


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
            if (hitCollider.CompareTag(_solidObjectTag)) Destroy(this.gameObject);
        }

    }

    private void HitTarget()
    {
        Damage(target);
    }

    void Damage(Transform enemy)
    {
        Enemy e = enemy.GetComponent<Enemy>();

        if (e != null)
        {
            e.GetComponent<Health>().Damage(power);

            /// Register enemy slowing, if this is a slower bullet.
            if (doSlowing)
            {
                if (e.SlowingCoroutine != null) e.StopCoroutine(e.SlowingCoroutine);
                e.SlowingCoroutine = e.StartCoroutine(e.SlowEnemy(slowTo, slowTime));
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
 
}
