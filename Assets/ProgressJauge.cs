using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressJauge: MonoBehaviour {

    public Image soleilIcon, plantIcon;
    public Transform cam, sol;
    public float startJauge, endJauge, startReal, endReal;

    // Update is called once per frame
    void Update ( ) {
        float tmp = endReal - startReal; // longueur en unit du nivau depuis le depart de la lumiere

        float pozCam = cam.position.x - startReal; // position de la camera en unit ramené au depart
        float percCam = pozCam/tmp; // pourcentage de la position de la camera
        //print ("percCam " + percCam);
        float jaugePercCam = ( endJauge - startJauge ) * percCam; //nombre de pixel de la jauge a parcourir
        print(jaugePercCam);
        print(startJauge);
        Vector3 plantPoz = new Vector3 (startJauge + jaugePercCam, plantIcon.transform.localPosition.y, plantIcon.transform.localPosition.z);
        plantIcon.transform.localPosition = plantPoz;

        float pozSol = sol.position.x - startReal;
        float percSol = pozSol/tmp;
                //print ("percsol " + percSol);

        float jaugePercSol = ( endJauge - startJauge ) * percSol;
        Vector3 SoleilPoz = new Vector3 (startJauge + jaugePercSol, soleilIcon.transform.localPosition.y, soleilIcon.transform.localPosition.z);
        soleilIcon.transform.localPosition = SoleilPoz;


        }
    }
