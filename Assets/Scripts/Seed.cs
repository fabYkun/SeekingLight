using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class                Seed: MonoBehaviour
{
    Rigidbody2D             rb2d;
    public float            thrust;
    public float            rotationSpeed, pollenPerSec;
    private float           pollenReserve;
    private float           waterReserve;
    CollectibleManagement   pollenGauge;
    CollectibleManagement   waterGauge;
    private bool            hasGerminated = false;

    PlantPower              pP;
    Camera                  cam;
    public GameObject       bebePlante;

    void                    Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D> ( );
        cam = Camera.instance;
        cam.adjust = new Vector3(3, 0, 0);
    }

    void                    Update()
    {
        if (pollenReserve > 0)
            Propulsion ( );
        Rotation ( );
        cam.Follow(gameObject.transform);
    }

    void                    Propulsion()
    {
        bool                input = Input.GetButton ("Propulse");

        if (input)
        {
            Vector2         adaptedForce = transform.up * thrust;
            //print (adaptedForce);
            rb2d.AddForce (adaptedForce);
            pollenReserve -= Time.deltaTime * pollenPerSec;
        }
        pollenGauge.SetGauge ((int)pollenReserve);
    }

    void                    Rotation()
    {
        if ( Input.GetButton ("Left") )
            rb2d.AddTorque (rotationSpeed);
        if ( Input.GetButton ("Right") )
            rb2d.AddTorque (-rotationSpeed);
    }

    public void             Initialize(PlantPower plPo)
    {
        this.pP = plPo;
        this.pollenGauge = plPo.pollenGauge;
        this.waterGauge = plPo.waterGauge;
        this.pollenReserve = plPo.pollenReserve;
        this.waterReserve = plPo.waterReserve;
    }

    void                    OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "terre" && !this.hasGerminated)
        {
            GameObject tmp = Instantiate(bebePlante, transform.position, Quaternion.identity);
            tmp.GetComponent<PlantPower>().Initialize(this.waterGauge, this.pollenGauge);
            Destroy(this.gameObject);
            this.hasGerminated = true;
        }
        else if(col.gameObject.tag == "pierre" && !this.hasGerminated) { }
        else if (this.pollenReserve <= 0)
        {
            this.pP.controlled = true;
            Destroy(this.gameObject);
        }
    }
}