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
    [Tooltip("The tag of the walls, or any object that the bullet can not go through.")] [SerializeField] string _solidObjectTag = "Can Stop Bullets";
    [Tooltip("The radius of the object collision checking.")] [SerializeField] float checkingRange = 1f;

    [HideInInspector] public Transform target;

    private void Update()
    {
        if (target == null) { Destroy(gameObject); return; } // Destroy bullet if no target

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame) // Destroy bullet if it has travelled as far as it can go
        {
            Destroy(target.gameObject); // Destroy the target to remove heirarchy clutter


            Destroy(this.gameObject); // Don't do this if different bullet types are added, (boomerangs, zig zags, etc..)
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);


        // Checks to see when it should delete the bullet after being too close to an object (after colliding with a wall)
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, checkingRange);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag(_solidObjectTag)) Destroy(this.gameObject);
        }
    }


    private void OnTriggerEnter(Collider col)
    {
        if (col.GetComponent<Bullet>()) return;
        if (col.GetComponent<PlayerManager>()) return;


        if (col.GetComponent<Enemy>())
        {
            col.GetComponent<Health>().Damage(damage);
            Destroy(this.gameObject);
        }
    }


}
