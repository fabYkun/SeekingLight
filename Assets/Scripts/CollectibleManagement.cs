using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class                    CollectibleManagement : MonoBehaviour
{
    public Image                gauge;

    public void                 SetGauge (float perc)
    {
        this.gauge.fillAmount = perc / 100;
    }

    public float                GetValue()
    {
        return this.gauge.fillAmount * 100;
    }
}