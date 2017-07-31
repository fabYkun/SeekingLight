using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour {
    public static Camera instance;

    public Vector3 adjust;
    public float step;


	void Awake () {
        if (instance == null)
            instance = this;
        else
            GameObject.Destroy(this.gameObject);
    }
	
    public void Follow(Transform cible){
        Vector3 ciblasse = new Vector3(cible.transform.position. x, cible.transform.position.y, transform.position.z) + adjust;
        transform.position = Vector3.MoveTowards(transform.position, ciblasse , step);
        }
}
