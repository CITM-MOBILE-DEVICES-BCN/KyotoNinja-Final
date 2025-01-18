using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlade : MonoBehaviour
{
    public float rotationSpeed = 360f;

    void Update()
    {
        // Rotar la cuchilla sobre el eje Z
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
