using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantRotate : MonoBehaviour
{
    // Update is called once per frame
    [SerializeField] private float rotationSpeed = 10f;

    void Update()
    {
        transform.Rotate(new Vector3 (0, 5, 0) * Time.deltaTime * rotationSpeed);
    }
}
