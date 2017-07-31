using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class                            Scenary : MonoBehaviour
{
    public float                        delay;
    public float                        fadeInTime;
    public float                        stayVisibleTime;
    public float                        fadeOutTime;
    public Color                        fadedOutColor;
    public Color                        fadedInColor;
    [SerializeField]
    private SpriteRenderer[]            sprites;
    private bool                        launched = false;

	void                                Awake ()
    {
        this.sprites = this.GetComponentsInChildren<SpriteRenderer>();
	}

    void                                Start()
    {
        this.delay = Time.time + this.delay;
    }

    IEnumerator                         drawThings()
    {
        float                           time = 0;

        while (time < fadeInTime)
        {
            time += Time.deltaTime;
            for (int i = 0; i < this.sprites.Length; ++i)
                this.sprites[i].color = Color.Lerp(this.fadedOutColor, this.fadedInColor, time / fadeInTime);
            yield return null;
        }
        time = 0;
        while (time < stayVisibleTime)
        {
            time += Time.deltaTime;
            yield return null;
        }
        time = 0;
        while (time < fadeOutTime)
        {
            time += Time.deltaTime;
            for (int i = 0; i < this.sprites.Length; ++i)
                this.sprites[i].color = Color.Lerp(this.fadedInColor, this.fadedOutColor, time / fadeOutTime);
            yield return null;
        }
    }
	
	void                                Update()
    {
        if (!launched && Time.time >= delay)
        {
            StartCoroutine(drawThings());
            this.launched = true;
        }
	}
}
