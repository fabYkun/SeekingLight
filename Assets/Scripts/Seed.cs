using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class                Seed : MonoBehaviour
{
    Rigidbody2D             rb2d;
    public float            thrust;
    public float            rotationSpeed;
    public float            pollenReserve, pollenPerSec, pollenMax;
    public AnimationCurve   powerPerc;
    PollenManagement        pM;

    void                    Start()
    {
        pM = FindObjectOfType<PollenManagement>();
        rb2d = gameObject.GetComponent<Rigidbody2D>();
    }

    void                    Update()
    {
        if (pollenReserve > 0) Propulsion();
        Rotation();
    }

    void                    Propulsion()
    {
        bool                input = Input.GetButton("Propulse");

        if (input)
        {
            Vector2 adaptedForce = transform.up * powerPerc.Evaluate(pollenReserve/ pollenMax) * thrust;
            //print (adaptedForce);
            rb2d.AddForce(adaptedForce);
            pollenReserve -= Time.deltaTime * pollenPerSec;
        }
        pM.SetPollenGauge((int)pollenReserve);
    }

    void                    Rotation()
    {
        if (Input.GetButton("Left"))    rb2d.AddTorque(rotationSpeed);
        if (Input.GetButton("Right"))   rb2d.AddTorque(-rotationSpeed);
    }
}