using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    [Tooltip("This is the start health of the generator at the end of this path. Make sure that this is balanced.")] [SerializeField] [Range(100, 15000)] int generatorStartHealth = 500;

    [Tooltip("This is the health of the generator.")] public int generatorCurrentHealth;

    [Tooltip("Nothing should be assigned to this. Waypoints are automatically assigned! It is visible for debug purposes only.")] [SerializeField] public Transform[] points;

    [Tooltip("Is the generator alive?")] [SerializeField] bool isGeneratorAlive;

    private void Awake()
    {
        // Starts by setting all of the waypoints automatically into the waypoints array.
        points = new Transform[transform.childCount];

        for (int i = 0; i < points.Length; i++)
        {
            points[i] = transform.GetChild(i);
        }

        // Set the current health to the specified start health
        generatorCurrentHealth = generatorStartHealth;

        isGeneratorAlive = true;
    }

    public void DamageGenerator(int damage) // This will damage the generator. If the health is 0, the generator dies.
    {
        generatorCurrentHealth -= damage;

        // Handle generator destruction
        if (generatorCurrentHealth <= 0)
        {
            Debug.Log("<color=red>A generator is destroyed.</color>");
            isGeneratorAlive = false;
        }
        else isGeneratorAlive = true;
    }
        
}
