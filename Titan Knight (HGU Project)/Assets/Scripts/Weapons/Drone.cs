/* MARK GASKINS
 * 
 * THIS SHOULD BE ATTACHED TO THE SCRIPT OBJECT OF A DRONE AT ALL TIMES
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Drone : MonoBehaviour
{
    [Header("Options")]

    [Tooltip("The duration of this drone. How long until it is automatically returned back to its post to recharge?")] [Range(10, 30)] public int startDuration = 15;
    [Space(10)]
    [Tooltip("The attack range is how close the enemy must be for the drone to attack her. \nEnsure that this isn't too low- so the Drone doesn't go INTO the enemy when attacking.")] [SerializeField] [Range(0.0f, 100.0f)] float attackRange;
    [Tooltip("The attack range is how close the enemy must be for the drone to see her. \nEnsure that this isn't too low- so the Drone can still see enemies.")] [SerializeField] [Range(0.0f, 100.0f)] float sightRange;

    [Tooltip("This is the time between her attacks.")] [SerializeField] [Range(0.0f, 3.0f)] float attackRate = 1.5f;

    [Header("Important References")]
    [Tooltip("Ensure that these are assigned at all times.")] [SerializeField] Transform ModelParent;
    
    private NavMeshAgent agent;
    private GameManager gm; 

    [Space(10)]
    [Header("DEBUG")]
    [SerializeField] Transform enemyTarget; // Visible for debug purposes only. Do not modify

    private bool eInAttackRange = false; // Is enemy in attack range?

    bool droneIsAlive = true, canPerish = true;
    float currentDuration = 0.0f;


    void Start()
    {
        // Initialize all that crap
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
         
        agent = GetComponent<NavMeshAgent>();
        gm = FindObjectOfType<GameManager>();
       
        currentDuration = startDuration;
    }


    // Update is called once per frame
    void Update()
    {
        if (gm.currentMode == GameMode.Build || PlayerManager.generatorsDestroyed) { agent.enabled = false; return; } else agent.enabled = true; // Completely pause the enemy when the current mode is Build Mode.


        if (enemyTarget != null)
        {

            eInAttackRange = Vector3.Distance(transform.position, enemyTarget.position) < attackRange; // She is in general range of attacking the target.  

            // Chase/Att
            if (!eInAttackRange) ChaseTarget();
            else AttackTarget();
        }
        
    }

     
    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        float distanceToPlayer = 0;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        distanceToPlayer = Vector3.Distance(transform.position, FindObjectOfType<PlayerManager>().transform.position);
        nearestEnemy = FindObjectOfType<PlayerManager>().gameObject;
        

        if (nearestEnemy != null && shortestDistance <=  && nearestEnemy.GetComponent<Enemy>().enabled) // This targets the nearest enemy, but can be modified later to target the enemy with the most power, HP, etc..
        {
            enemyTarget = nearestEnemy.transform;
        }
        else
            enemyTarget = null;

    }


    private void FixedUpdate()
    {
        if (gm.currentMode == GameMode.Build) return;

        if (currentDuration >= 0) droneIsAlive = true;
        else droneIsAlive = false;

        if (droneIsAlive)
            currentDuration -= Time.fixedDeltaTime;
        else
        {
            if (canPerish)
            {
                canPerish = false;

                DronePerish();
            }
        }

    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(this.transform.position, attackRange);
    }
    /// <summary>
    /// This automatically happens when she runs out of duration.
    /// </summary>
    public void DronePerish()
    {
        Debug.Log("<b>[Drone]</b> <color=red>A drone has perished.</color>");
    }



    /// <summary>
    /// Chase the enemy target
    /// </summary>
    private void ChaseTarget()
    {
        if (gm.currentMode == GameMode.Build) return;

        agent.SetDestination(enemyTarget.position); // Simply follows the player
        ModelParent.transform.LookAt(enemyTarget.position);
    }

    private bool hasAttacked = false;

    // This is automatically, and continuously called when the drone is in range to atk. (fun fact: this was typed in vr)
    private void AttackTarget()
    {
        if (gm.currentMode == GameMode.Build) return;

        agent.SetDestination(transform.position); // Stops the enemy from moving when it attacks in order to stop it from continuously running. This could be changed later to make different, faster types of enemies

        ModelParent.transform.LookAt(enemyTarget.position);


        // This initiates a delay between her attacks, dependent on attackRate.
        if (!hasAttacked)
        {
            hasAttacked = true;

            // eAnimator.SetTrigger("Attack"); <-- ( Here is where you'd run an animation. )

            Attack();

            Invoke(nameof(ResetAttack), attackRate);
        }
    }

    private void Attack()
    {
        if (gm.currentMode == GameMode.Build) return;

        Debug.Log("Attack!");
    }

    private void ResetAttack()
    {
        hasAttacked = false;
    }

}
