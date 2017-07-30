using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightScroll : MonoBehaviour {
    public AnimationCurve scrollSpeed;
    float timer = 0; // correspond au x de la courbe precedante
	public float maxtimer, unitPerSec;

	void Update () {
		timer += Time.deltaTime;
        transform.position = new Vector3(transform.position.x + Time.deltaTime * unitPerSec, transform.position.y, transform.position.z);
	}
}
