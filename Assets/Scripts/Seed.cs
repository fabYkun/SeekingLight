using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed: MonoBehaviour {
    Rigidbody2D             rb2d;
    public float            thrust;
    public float            rotationSpeed, pollenPerSec, pollenMax=6;
    private float           pollenReserve;
    public AnimationCurve   powerPerc;
    PollenManagement        pM;
    PlantPower              pP;
    public GameObject       bebePlante;

    void Start ( ) {
        pM = FindObjectOfType<PollenManagement> ( );
        rb2d = gameObject.GetComponent<Rigidbody2D> ( );
        }

    void Update ( ) {
        if ( pollenReserve > 0 )
            Propulsion ( );
        Rotation ( );
        }

    void Propulsion ( ) {
        bool input = Input.GetButton ("Propulse");

        if ( input ) {
            Vector2 adaptedForce = transform.up * powerPerc.Evaluate (pollenReserve/ pollenMax) * thrust;
            //print (adaptedForce);
            rb2d.AddForce (adaptedForce);
            pollenReserve -= Time.deltaTime * pollenPerSec;
            }
        pM.SetPollenGauge ((int)pollenReserve);
        }

    void Rotation ( ) {
        if ( Input.GetButton ("Left") )
            rb2d.AddTorque (rotationSpeed);
        if ( Input.GetButton ("Right") )
            rb2d.AddTorque (-rotationSpeed);
        }

    public void Initialize (PlantPower plPo) {
        this.pP = plPo;
        this.pollenReserve = plPo.pollenReserve;
        }

    void OnCollisionEnter2D(Collision2D col) {
        if(col.gameObject.tag == "terre") {
            print("entered");
            Instantiate(bebePlante, transform.position, bebePlante.transform.rotation);
            DestroyObject(gameObject);
            }
        else print("GAMEOVER BECAUSE OF PAS FERTILE");
        }
    }