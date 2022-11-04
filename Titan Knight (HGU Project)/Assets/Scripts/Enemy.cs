/* MARK GASKINS
 * 
 * THIS SHOULD BE ATTACHED TO THE PARENT OBJECT OF AN ENEMY AT ALL TIMES
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [Header("Options")]
    //[SerializeField] int walkSpeed;
    [Tooltip("The attack damage that this enemy does")] [SerializeField] int attackDamage = 5;
    [Tooltip("This is the time between her attacks")] [SerializeField] [Range(0.0f, 3.0f)] float attackRate = 1;
    [Tooltip("The sight range is how close a player must be for an enemy to see her. The attack range is how close the player must be for the enemy to attack her.")] [SerializeField] [Range(0.0f, 100.0f)] float sightRange, attackRange;
    [Tooltip("The range of how far this enemy will patrol when no player is in range.")] [SerializeField] [Range(2,15)] float patrolRange;



    [Header("Important References")]
    [Tooltip("Ensure that these are assigned at all times.")] [SerializeField] LayerMask GroundLayer;
    [Tooltip("Ensure that these are assigned at all times.")] [SerializeField] LayerMask PlayerLayer;

    [SerializeField] NavMeshAgent agent;

    private Transform player;

    private Vector3 walkPoint; // Patrolling destination
    private bool walkPointSet; // (Is there a patrol destination currently set)

    private bool hasAttacked, pInAttackRange, pInSightRange;

    private void Awake()
    {
        player = FindObjectOfType<PlayerManager>().transform;
        agent = GetComponent<NavMeshAgent>();
    }


    private void FixedUpdate()
    {
        // This checks to see if the player is in sight and attack range.
        pInSightRange = Physics.CheckSphere(transform.position, sightRange, PlayerLayer); // She is in the general range of sight 
        pInAttackRange = Physics.CheckSphere(transform.position, attackRange, PlayerLayer); // She is in general range of being atkd  

        if (!pInSightRange && !pInAttackRange) Patrolling();
        if (pInSightRange && !pInAttackRange) ChasePlayer();
        if (pInSightRange && pInAttackRange) AttackPlayer();



        // Looks at the player 
        Transform playerPos = FindObjectOfType<PlayerManager>().transform;

    }


    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("Player")) other.GetComponent<Health>().Damage(attackDamage); // (Only hurt the player)
    //}


    private void Patrolling()
    {
        if (!walkPointSet) SearchWalkPoint(); // Find a walkpoint if there's no patrolling destination

        if (walkPointSet)  // If there is a walkpoint, just make the navmesh travel to it
        {
            agent.SetDestination(walkPoint);
            transform.LookAt(walkPoint);
        }

        Vector3 distanceToWalPoint = transform.position - walkPoint;


        // Arrived at walkpoint
        if (distanceToWalPoint.magnitude < 1f) walkPointSet = false; // Reached destination, now repeat patrol
        
    }

    private void SearchWalkPoint() // Find a random position within the patrolling range to patrol to, and ensure that it IS ON THE GROUND so the enemy doesn't walk off of the map 
    {
        float randomZ = Random.Range(-patrolRange, patrolRange); 
        float randomX = Random.Range(-patrolRange, patrolRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);



        if (Physics.Raycast(walkPoint, -transform.up, 2f, GroundLayer)) walkPointSet = true; // Latch the lockpoint variable
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position); // Simply follows the player
        transform.LookAt(player.position);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position); // Stops the enemy from moving when it attacks in order to stop it from continuously running. This could be changed later to make different, faster types of enemies

        transform.LookAt(player);


        // This initiates a delay between her attacks, dependent on attackRate.
        if (!hasAttacked) 
        {
            hasAttacked = true;

            Attack();

            Invoke(nameof(ResetAttack), attackRate); 
        }
    }

    private void ResetAttack()
    {
        hasAttacked = false; 
    }



    private void Attack()   /* This is the attack function. Currently, it just hits the player, but later, it can be more complex-- (projectiles, self-destruction, etc..) */
    {
        player.GetComponent<Health>().Damage(attackDamage); // (Only hurt the player)
    }
}
