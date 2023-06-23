/* MARK GASKINS
 * 
 * THIS SHOULD BE ATTACHED TO THE PARENT OBJECT OF AN ENEMY AT ALL TIMES
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyFollowMode 
{
    FollowPath, // Follows predetermined pathing. This requires the enemy to havebeen spawned in by an EnemySpawner.
    FollowPlayer // Follows player. This can be manually placed, or spawned by an EnemySpawner.
}

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [Header("Options")]
    [Tooltip("Should the enemy follow a path or follow the player?\n There should be two prefab isotopes of this enemy with either mode enabled.")] [SerializeField] public EnemyFollowMode FollowMode = EnemyFollowMode.FollowPath; // Very important stuff

    //[SerializeField] int walkSpeed;
    [Tooltip("The attack damage that this enemy does. \nNote, this is also the damage that it will do to the generator.")] [SerializeField] int attackDamage = 5;
    [Tooltip("This is the time between her attacks.")] [SerializeField] [Range(0.0f, 3.0f)] float attackRate = 1;
    [Tooltip("The sight range is how close a player must be for an enemy to see her.")] [SerializeField] [Range(0.0f, 100.0f)] float sightRange;
    [Tooltip("The attack range is how close the player must be for the enemy to attack her. \nEnsure that this isn't too low- so the Enemy doesn't go INTO the player when attacking.")] [SerializeField][Range(0.0f, 100.0f)] float attackRange;
    [Tooltip("The range of how far this enemy will patrol when no player is in range. \nEnsure this value isn't too high- So the enemy doesn't wander away.")] [SerializeField] [Range(2,100)] float patrolRange;
    [Space(10)] [SerializeField] [Tooltip("The amount of currency that the player will gain when this enemy is killed.\nCAUTION: ENSURE THAT THIS IS BALANCED!")] [Range(1, 375)] int bounty = 10;


    [Header("Important References")]
    [Tooltip("Ensure that these are assigned at all times.")] [SerializeField] LayerMask GroundLayer;
    [Tooltip("Ensure that these are assigned at all times.")] [SerializeField] LayerMask PlayerLayer;

    [SerializeField] NavMeshAgent agent;

    private GameManager gm;

    private Transform player;

    private Vector3 walkPoint; // Patrolling destination
    private bool walkPointSet; // (Is there a patrol destination currently set)

    private bool hasAttacked, pInAttackRange, pInSightRange;


    private void Awake() // Initialize all vaues
    {
        gm = FindObjectOfType<GameManager>();
        player = FindObjectOfType<PlayerManager>().transform;
        agent = GetComponent<NavMeshAgent>();
        // if (FollowMode == EnemyFollowMode.FollowPath) target = assignedPath.points[0];  // If the enemy prefab does not start with 'enabled' to false, a bug will come from this line of code.
    
    }


    #region Follow Player Mechanics
    private void FixedUpdate()
    {

        // The script will now determine whether or not this enemy should follow the player or follow a path's waypoints.
        if (FollowMode == EnemyFollowMode.FollowPlayer) 
        {

            if (gm.currentMode == GameMode.Build || PlayerManager.generatorsDestroyed) { agent.enabled = false; return;  } else agent.enabled = true; // Completely pause the enemy when the current mode is Build Mode.


            // This checks to see if the player is in sight and attack range.
            pInSightRange = Physics.CheckSphere(transform.position, sightRange, PlayerLayer); // She is in the general range of sight 
            pInAttackRange = Physics.CheckSphere(transform.position, attackRange, PlayerLayer); // She is in general range of being atkd  


            if (!pInSightRange && !pInAttackRange) Patrolling();
            if (pInSightRange && !pInAttackRange) ChasePlayer();
            if (pInSightRange && pInAttackRange) AttackPlayer();



            // Looks at the player 
            Transform playerPos = FindObjectOfType<PlayerManager>().transform;
        }

        GetComponentInChildren<Health>()._bounty = bounty; // Set the bounty

    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("Player")) other.GetComponent<Health>().Damage(attackDamage); // (Only hurt the player)
    //}


    private void Patrolling()
    {
        if (gm.currentMode == GameMode.Build) return;

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
        if (gm.currentMode == GameMode.Build) return;

        agent.SetDestination(player.position); // Simply follows the player
        transform.LookAt(player.position);
    }

    private void AttackPlayer()
    {
        if (gm.currentMode == GameMode.Build) return;

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
        player.GetComponent<Health>().Damage(attackDamage); // Basic Hit method that will attack the player, dealing attackDamage damage. 
    }
    #endregion

    #region Follow Predetermined Path (waypoints)
    
    // Speed is determined by the speed of the Navmesh Agent.

     

    [HideInInspector] public Transform target;
    public Waypoints assignedPath;

    private int waypointIndex = 0;

    private void Update() // This update method should only be active while the EnemyFollowMode is FollowPath
    {
        if (FollowMode == EnemyFollowMode.FollowPlayer) return; // Only allow for this region to function when the follow mode is FollowPlayer.
        
        if (PlayerManager.isDead) return; // Pause enemies if the player is dead

        if (PlayerManager.generatorsDestroyed) return; // Pause enemies if all generators destroyed

        if (gm.currentMode == GameMode.Build) return; // Pause enemies if the player is in build mode

        if (target == null) target = assignedPath.points[0];


        /// FOLLOW PATH ----- 
       
        agent.enabled = false; // Disable the Ai. This is for follwo path, not follow player.

        Vector3 nextWaypoint = target.position - transform.position; // Set the next waypoint that the enemy will walk to.

        transform.Translate(nextWaypoint.normalized * agent.speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.position) <= 2.5f) // The enemy will know to go to the next waypoint when it is 1.2 meters away from the current waypoint (target).
        {
            GetNextWaypoint();
        }

        if (nextWaypoint != Vector3.zero) transform.LookAt(target.transform.position); // Make the enemy look towards the next waypoint

    }

    private void GetNextWaypoint() // Update to tell the enemy where the next waypoint is, and set it to be the next "target".
    {
        if (waypointIndex >= assignedPath.points.Length - 1)
        {

            EndPath();
            return;
        }
        
       //  Debug.Log("<color=green>Next waypoint</color>");

        waypointIndex++;
        target = assignedPath.points[waypointIndex];
    }

    void EndPath() // If this is the final waypoint, the enemy will destroy itself, and cause damage to the generator.
    {
        Debug.Log($"<color=blue>[Enemy] </color> Enemy has successfully damaged a generator.");

        assignedPath.DamageGenerator(attackDamage);

        GameManager.enemiesAlive--; // Subtract from the list of enemies alive.

        Destroy(gameObject);
    }


    #endregion
}
