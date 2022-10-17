/* Mark Gaskins
 * THIS IS A MULTI-INSTANCE SCRIPT!
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Attributes")]
    [Tooltip("Damage that the bullet deals.")] [SerializeField] [Range(1, 99)] public int damage = 5;
    [SerializeField] [Range(10, 150)] float speed = 70f;


    [HideInInspector] public Transform target;

    private void Update()
    {
        if (target == null) { Destroy(gameObject); return; } // Destroy bullet if no target

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame) // Destroy bullet if it has travelled as far as it can go
        {
            Destroy(target.gameObject); // Destroy the target to remove heirarchy clutter


            Destroy(this.gameObject);
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }


    private void OnTriggerEnter(Collider col)
    {
        if (col.GetComponent<Bullet>()) return;
        if (col.GetComponent<PlayerManager>()) return;


        if (col.GetComponent<Enemy>())
        {
            col.GetComponent<Enemy>().TakeDamage(damage);
        }
    }
}
