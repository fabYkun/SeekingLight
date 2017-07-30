using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PollenManagement: MonoBehaviour {
    Image [] pollens;
    public Image pollen0, pollen1, pollen2, pollen3, pollen4, pollen5;

    void Awake ( ) {
        pollens = new Image [6];
        pollens [0] = pollen0;
        pollens [1] = pollen1;
        pollens [2] = pollen2;
        pollens [3] = pollen3;
        pollens [4] = pollen4;
        pollens [5] = pollen5;
        }
    public void SetPollenGauge (int nb) { // nb etant le nb de jauge a afficher
        for (int i = 0; i < 6; ++i) {
            if (i < nb)
            pollens[i].fillAmount = 1;
            else pollens[i].fillAmount = 0;

            }


        }
    }
