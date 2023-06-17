using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour 
{

    [Header("Options")]
    [SerializeField] public int enemiesToSpawn = 16;
    [Range(0.1f, 3f)] [SerializeField] float spawnRate = 2; 

    [Space(10)]
    [Header("References")]
    [Tooltip("The waypoint that the enemies spawn at.")] [SerializeField] Transform SpawnWaypoint;
    [Tooltip("This is REQUIRED. It must be attached to its corresponding Waypoints script AT ALL TIMES.")] [SerializeField] Waypoints CorrespondingPathway;
    [Tooltip("Enemy prefabs to spawn in. Uses RNG to choose which one to spawn, but that can/will be modified later.")] [SerializeField] List<GameObject> EnemyPrefabs; // RNG determines which one of the enemies in this List will spawn in..
    [Tooltip("The sound that plays when a enemy spawns here.")] [SerializeField] AudioClip spawnSound;

    GameManager gm;

    private void OnEnable()
    {
        gm = FindObjectOfType<GameManager>(); // Assign the build mode

        if (spawnSound == null) Debug.Log("<color=cyan>Notice! You have not assigned the audio clip for the spawn sound for an Enemy Spawner in this scene!</color> The enemy spawner will not function properly. If you are unaware of how to add an audio clip to the Build Menu's script, please contact Mark in the Hidden Genius Slack.");
    }

    public IEnumerator SpawnWave()
    {
        
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            // Assign the required values to the enemy prefab to let it navigate properly
            GameObject spawnedEnemyObj = Instantiate(EnemyPrefabs[Random.Range(0, EnemyPrefabs.Count)], SpawnWaypoint.position, Quaternion.identity);
            Enemy thisEnemy = spawnedEnemyObj.GetComponent<Enemy>();

            if (spawnSound != null)FindObjectOfType<GameManager>().CoreFXPlayer.PlayOneShot(spawnSound);

            thisEnemy.assignedPath = CorrespondingPathway;
            thisEnemy.target = thisEnemy.assignedPath.points[0];

            // thisEnemy.FollowMode = EnemyFollowMode.FollowPath; // Discontinued, to allow for enemies that follow the player to also be spawned.

            thisEnemy.GetComponent<Enemy>().enabled = true;

            // WaitForSeconds() is prohibited in the spawning of waves. WaitForSeconds() disregards the necessity of pausing the spawning of waves while in build mode. 
            float timer = 0f; 
            while (timer < 1f)
            {
                while (gm.currentMode == GameMode.Build) // Pause the spawning of waves while in build mode
                {
                    yield return null;
                }

                timer += Time.deltaTime;
                yield return null;
            }
        }
    }
}
