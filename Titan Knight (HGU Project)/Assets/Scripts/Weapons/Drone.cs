/* MARK GASKINS
 * 
 * THIS SHOULD BE ATTACHED TO THE SCRIPT OBJECT OF A DRONE AT ALL TIMES
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

enum AttackState
{
    Recharge,
    Attack
}
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
    [Tooltip("The type of bullet that the drone shoots.")] [SerializeField] GameObject DroneBullet;
    [Space(10)]
    [Tooltip("Location of the drone bullet's spawning.")] [SerializeField] GameObject FirePos;
    [Tooltip("Effect that the drone display swhen bullet is fired.")] [SerializeField] GameObject FireVFX;

    private NavMeshAgent agent;
    private GameManager gm; 

    [Space(10)]
    [Header("DEBUG")]
    public Transform enemyTarget; // Visible for debug purposes only. Do not modify
    public Transform MyDroneNode;
    public GameObject DroneNodeCharging, DroneNodeActive;

    private AttackState attackState;
    private bool eInAttackRange = false; // Is enemy in attack range?

    bool droneIsAlive = true, canPerish = true;
    float currentDuration = 0.0f;


    void Start()
    {
        // Initialize all that crap
        agent = GetComponent<NavMeshAgent>();
        gm = FindObjectOfType<GameManager>();
       
        currentDuration = startDuration;
    }


    // Update is called once per frame
    void Update()
    {
        if (PlayerManager.generatorsDestroyed) // Player lost
        { agent.enabled = false; return; }

        UpdateTarget();

        if (gm.currentMode == GameMode.Build)
        { attackState = AttackState.Recharge; DroneNodeCharging.SetActive(true); DroneNodeActive.SetActive(false); }
        else
        { attackState = AttackState.Attack; DroneNodeCharging.SetActive(false); DroneNodeActive.SetActive(true); }

        if (enemyTarget != null && attackState == AttackState.Attack)
        {

            eInAttackRange = Vector3.Distance(transform.position, enemyTarget.position) < attackRange; // She is in general range of attacking the target.  


            if (gm.currentMode == GameMode.Combat)
            {
                // Chase/Att
                if (!eInAttackRange) ChaseTarget();
                else AttackTarget();


            }
        }
        else if (attackState == AttackState.Recharge)
        {
            /// automatically goes back to the drone node 

            agent.SetDestination(MyDroneNode.position); // Simply follows the player
            ModelParent.transform.LookAt(MyDroneNode.position);
        }
        
    }


    void UpdateTarget()
    {
        if (attackState == AttackState.Attack)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            float shortestDistance = Mathf.Infinity;
            GameObject nearestEnemy = null;

            foreach (GameObject enemy in enemies)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = enemy;
                }
            }

            if (nearestEnemy != null && shortestDistance <= sightRange && nearestEnemy.GetComponent<Enemy>().enabled) // This targets the nearest enemy, but can be modified later to target the enemy with the most power, HP, etc..
                enemyTarget = nearestEnemy.transform;
            else
                enemyTarget = null;

        }
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
        Gizmos.DrawWireSphere(this.transform.position, sightRange);

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
        Debug.Log("Attack");


        // This initiates a delay between her attacks, dependent on attackRate.
        if (!hasAttacked)
        {
            hasAttacked = true;

            // droneAnimator.SetTrigger("Attack"); <-- ( Here is where you'd run an animation. )

            Attack();

            Invoke(nameof(ResetAttack), attackRate);
        }
    }

    private void Attack()
    {
        if (gm.currentMode == GameMode.Build) return;

        /// Shoot a bullet at the enemy target
        Instantiate(FireVFX, FirePos.transform);
        GameObject bulletObj = (GameObject)Instantiate(DroneBullet);
        TurretBullet bullet = bulletObj.GetComponent<TurretBullet>();

        Debug.Log("Attack");
        try
        {
            bullet.Seek(enemyTarget);
            bullet.transform.position = FirePos.transform.position;
        }
        catch (System.Exception Ex)
        {
            Debug.Log(Ex.ToString());
        }
    }

    private void ResetAttack()
    {
        hasAttacked = false;
    }

}
