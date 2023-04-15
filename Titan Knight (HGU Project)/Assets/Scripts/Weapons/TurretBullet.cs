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
    [Tooltip("The speed that the bullet travels.")] [SerializeField] [Range(10, 150)] float speed = 70f;
    [Tooltip("The damage that the bullet does.")] [SerializeField] [Range(2, 1000)] int power = 50;
    [Tooltip("The tag of the walls, or any object that the bullet can not go through.")] [SerializeField] string _solidObjectTag = "Can Stop Bullets";
    [Tooltip("The radius of the object collision checking.")] [SerializeField] float checkingRange = 1f;
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

        Destroy(gameObject);
    }

    void Damage(Transform enemy)
    {
        Enemy e = enemy.GetComponent<Enemy>();

        if (e != null)
        {
            e.GetComponent<Health>().Damage(power);
        }
        else
        {
            Debug.Log("This isn't an enemy.");
        }
        Destroy(gameObject);
    }
 
}
