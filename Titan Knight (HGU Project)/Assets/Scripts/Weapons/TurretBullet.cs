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
