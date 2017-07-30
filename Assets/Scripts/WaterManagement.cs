using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterManagement: MonoBehaviour {
    public Image water;
    public void SetWaterGauge (float perc) {

        water.fillAmount = perc;
        }
    }
