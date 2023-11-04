using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Mark Gaskins - Just automatically deletes the object after timeToDelete seconds.
/// </summary>
public class AutoDelete : MonoBehaviour
{
    [SerializeField] [Tooltip("Seconds until the object is deleted.")] float timeTillDeletion = 10.0f;


    private void Awake()
    {
        Invoke(nameof(DestroyObject), timeTillDeletion);
    }

    // Destroy the object.
    private void DestroyObject()
    {
        Destroy(this.gameObject);
    }
}
