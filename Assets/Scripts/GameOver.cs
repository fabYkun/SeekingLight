using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class                        GameOver: MonoBehaviour
{
    public Image                    deathBackground;
    public Color                    initialDeathBGColor;
    public Color                    finalDeathBGColor;
    public Image                    deathScreen;
    public Color                    initialDeathColor;
    public Color                    finalDeathColor;
    public float                    fadeTime = 2;
    public float                    endWaitingTime = 1;

    IEnumerator                     fadeDeath()
    {
        float                       time = 0;

        while (time < this.fadeTime)
        {
            time += Time.deltaTime;
            deathBackground.color = Color.Lerp(this.initialDeathBGColor, this.finalDeathBGColor, time / fadeTime);
            deathScreen.color = Color.Lerp(this.initialDeathColor, this.finalDeathColor, time / fadeTime);
            yield return null;
        }
        time = 0;
        while (time < this.endWaitingTime)
        {
            time += Time.deltaTime;
            yield return null;
        }
        SceneManager.LoadScene(0);
    }

    void                            OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "gameOver")
            StartCoroutine(fadeDeath());
    }
}
