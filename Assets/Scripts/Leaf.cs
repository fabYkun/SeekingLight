using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class                        Leaf : MonoBehaviour
{
    [SerializeField]
    private PlantPower              plant;
    public float                    waterAmount;
    public float                    pollenAmount;

    public void                     Initialize(PlantPower plant)
    {
        this.plant = plant;
    }

    void                            OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("collectiblePollen")) this.plant.pollenReserve += this.pollenAmount;
        if (collider.CompareTag("collectibleWater")) this.plant.waterReserve += this.waterAmount;
        Destroy(collider.gameObject);
    }
}
