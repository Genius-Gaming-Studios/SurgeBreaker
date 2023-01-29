using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour 
{

    [Header("Options")]
    [SerializeField] int enemiesToSpawn = 16;
    [Range(0.1f, 3f)] [SerializeField] float spawnRate = 2; 

    [Space(10)]
    [Header("References")]
    [Tooltip("The waypoint that the enemies spawn at.")] [SerializeField] Transform SpawnWaypoint;
    [Tooltip("This is REQUIRED. It must be attached to its corresponding Waypoints script AT ALL TIMES.")] [SerializeField] Waypoints CorrespondingPathway;
    [Tooltip("Enemy prefabs to spawn in. Uses RNG to choose which one to spawn, but that can/will be modified later.")] [SerializeField] List<GameObject> EnemyPrefabs; // RNG determines which one of the enemies in this List will spawn in..


    private void OnEnable()
    {
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            // Assign the required values to the enemy prefab to let it navigate properly
            GameObject spawnedEnemyObj = Instantiate(EnemyPrefabs[Random.Range(0, EnemyPrefabs.Count)], SpawnWaypoint.position, Quaternion.identity);
            Enemy thisEnemy = spawnedEnemyObj.GetComponent<Enemy>();

            thisEnemy.assignedPath = CorrespondingPathway;
            thisEnemy.target = thisEnemy.assignedPath.points[0];

            // thisEnemy.FollowMode = EnemyFollowMode.FollowPath; // Discontinued, to allow for enemies that follow the player to also be spawned.

            thisEnemy.GetComponent<Enemy>().enabled = true;

            yield return new WaitForSeconds(1 / spawnRate); // Waits before spawning in another enemy
        }
    }
}
