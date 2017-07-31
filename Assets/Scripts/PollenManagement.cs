using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PollenManagement: MonoBehaviour {
    [SerializeField]
    Image [] pollens;

    public void SetPollenGauge (int nb) { // nb etant le nb de jauge a afficher
        for (int i = 0; i < 6; ++i) {
            if (i < nb)
            pollens[i].fillAmount = 1;
            else pollens[i].fillAmount = 0;

            }


        }
    }
