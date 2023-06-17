using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Waypoints : MonoBehaviour
{
    [Tooltip("This is the start health of the generator at the end of this path. Make sure that this is balanced.")] [SerializeField] [Range(100, 15000)] int generatorStartHealth = 500;

    [Tooltip("Nothing should be assigned to this. Waypoints are automatically assigned! It is visible for debug purposes only.")] [SerializeField] public Transform[] points;

    [Tooltip("Is the generator alive?")] [SerializeField] bool isGeneratorAlive;

    [Tooltip("Generator health bar that corresponds to this path's generator")]

    public Health generatorHealth;
    [Tooltip("Automatically assigns the health if true.")]public bool autoAssignHealth;

    private void Awake()
    {
        // Assign corresponding generator
        if (autoAssignHealth) generatorHealth = GetComponent<Health>();
        generatorHealth.HealthType = ObjectType.Generator;

        // Starts by setting all of the waypoints automatically into the waypoints array.
        points = new Transform[transform.childCount];

        for (int i = 0; i < points.Length; i++)
        {
            points[i] = transform.GetChild(i);
        }

        // Set the start health in the manager
        generatorHealth.startHealth = generatorStartHealth;

        isGeneratorAlive = true;
    }

    public void DamageGenerator(int damage) // This will damage the generator. If the health is 0, the generator dies.
    {
        generatorHealth.currentHealth -= damage;
        generatorHealth.StartCoroutine(generatorHealth.DamageRenderer());

        // Handle generator destruction
        if (generatorHealth.currentHealth <= 0)
        {
            Debug.Log("<color=red>A generator is destroyed.</color>");
            isGeneratorAlive = false;
        }
        else isGeneratorAlive = true;
    }
        
}
