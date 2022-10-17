using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Tooltip("The enemy's start health.")] [SerializeField]  [Range(1, 2000)] int EnemyHealth;

    public void TakeDamage(int amount)
    {
        EnemyHealth -= amount;
    }

    private void FixedUpdate()
    {
        Transform playerPos = FindObjectOfType<PlayerManager>().transform;

        transform.LookAt(playerPos);

        
        
    }

    private void Update()
    {
        if (EnemyHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(this.gameObject);
    }

}
