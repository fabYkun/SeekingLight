using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class                        Leaf : MonoBehaviour
{
    [SerializeField]
    private PlantPower              plant;
    public float                    waterAmount = 1;
    public float                    pollenAmount = 1;

    public void                     Initialize(PlantPower plant)
    {
        this.plant = plant;
    }

    public void                     Initialize(PlantPower plant, float newWaterAmount, float newPollenAmount)
    {
        this.plant = plant;
        this.waterAmount = newWaterAmount;
        this.pollenAmount = newPollenAmount;
    }

    void                            OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.CompareTag("collectiblePollen")) {
            Debug.Log(this.plant.pollenReserve + " + " + this.pollenAmount);
            this.plant.pollenReserve += this.pollenAmount;
            Debug.Log("=" + this.plant.pollenReserve);
            if (this.plant.pollenReserve >100) this.plant.pollenReserve = 100;
        }
        if (collider.gameObject.CompareTag("collectibleWater"))
        {
            this.plant.AddNode();
            this.plant.waterReserve += this.waterAmount;
        }
        if (collider.gameObject.CompareTag("collectiblePollen") || collider.gameObject.CompareTag("collectibleWater")) Destroy(collider.gameObject);
    }
}
