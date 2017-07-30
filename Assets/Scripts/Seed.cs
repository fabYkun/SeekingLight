using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class                Seed : MonoBehaviour
{
    Rigidbody2D             rb2d;
    public float            thrust;
    public float            rotationSpeed;

    void                    Start() {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
    }

    void                    Update() {
        Propulsion();
        Rotation();
    }

    void                    Propulsion()
    {
        bool                input = Input.GetButton("Propulse");

        if (input)
            rb2d.AddForce(transform.up * thrust);
    }

    void                    Rotation()
    {
        if (Input.GetButton("Left"))    rb2d.AddTorque(rotationSpeed);
        if (Input.GetButton("Right"))   rb2d.AddTorque(-rotationSpeed);
    }
}