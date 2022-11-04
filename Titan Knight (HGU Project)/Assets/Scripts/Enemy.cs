/* MARK GASKINS
 * 
 * THIS SHOULD BE ATTACHED TO THE PARENT OBJECT OF AN ENEMY AT ALL TIMES
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int walkSpeed;
    [Tooltip("The attack damage that this enemy does")] [SerializeField] int attackDamage = 5;
    [Tooltip("(speed = rate/1)")] [SerializeField] [Range(0.0f, 3.0f)] float attackRate = 1;


    public bool canDamage; // Should be changed to a more efficient method later

    //private void Start()
    //{
    //    InvokeRepeating("DoDamage", 5 /* Attack Cooldown */, attackRate); // This is just a quick way of making the attack follow the attack rate, but it should be simplified in the MVP
    //}
    private void FixedUpdate()
    {
        Transform playerPos = FindObjectOfType<PlayerManager>().transform;

        transform.LookAt(playerPos);        
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")) other.GetComponent<Health>().Damage(attackDamage);
        //    canDamage = true;
        //else canDamage = false;
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player")) other.GetComponent<Health>().Damage(attackDamage);
    //    //canDamage = false;
    //}




}
