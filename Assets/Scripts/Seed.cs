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
    WaterManagement         wM;

    PlantPower              pP;
    Camera                  cam;
    public GameObject       bebePlante;

    void Start ( ) {
        pM = FindObjectOfType<PollenManagement> ( );
        wM = FindObjectOfType<WaterManagement> ( );

        rb2d = gameObject.GetComponent<Rigidbody2D> ( );
        cam = FindObjectOfType<Camera>();
        }

    void Update ( ) {
        if ( pollenReserve > 0 )
            Propulsion ( );
        Rotation ( );
        cam.Follow(gameObject.transform);
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
            GameObject tmp = Instantiate(bebePlante, transform.position, bebePlante.transform.rotation);
            tmp.GetComponent<PlantPower>().Initialize(wM, pM);
            DestroyObject(gameObject);
            }
        else if(col.gameObject.tag == "pierre") { }
        else print("GAMEOVER BECAUSE OF PAS FERTILE");
        }
    }