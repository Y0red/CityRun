using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelerometerController : MonoBehaviour
{
    // The speed at which the object will move
    public float speed = 10.0f;

    // The object's Rigidbody component
    private Rigidbody rb;

    void Start()
    {
        // Get the object's Rigidbody component
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Get the accelerometer's x and y values
        float x = Input.acceleration.x;
        float y = Input.acceleration.y;

        // Create a vector using the x and y values
        Vector3 movement = new Vector3(x, y, 0.0f);

        // Move the object based on the accelerometer's x and y values
        rb.velocity = movement * speed;
    }
}