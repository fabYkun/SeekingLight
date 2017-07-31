using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightScroll : MonoBehaviour {
    public AnimationCurve scrollSpeed;
    float timer = 0; // correspond au x de la courbe precedante
	public float maxtimer, unitPerSec;

    float               difficultyMode()
    {
        if (GameOver.instance.difficultyMode == DifficultyEnum.Easy) return 0;
        if (GameOver.instance.difficultyMode == DifficultyEnum.Normal) return 0.5f;
        return 1;
    }

	void Update () {
		timer += Time.deltaTime;
        transform.position = new Vector3(transform.position.x + Time.deltaTime * unitPerSec * this.difficultyMode(), transform.position.y, transform.position.z);
	}
}
