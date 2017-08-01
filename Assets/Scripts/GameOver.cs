using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum DifficultyEnum
{
    Easy,
    Normal,
    Hard
}

public class                        GameOver: MonoBehaviour
{
    public static GameOver          instance;

    public DifficultyEnum           difficultyMode;
    public Image                    deathBackground;
    public Color                    initialDeathBGColor;
    public Color                    finalDeathBGColor;
    public Image                    deathScreen;
    public Color                    initialDeathColor;
    public Color                    finalDeathColor;
    public float                    fadeTime = 2;
    public float                    endWaitingTime = 1;
    public PlantPower               plant;

    public void                     Awake()
    {
        if (instance == null)
            instance = this;
        else
            GameObject.Destroy(this.gameObject);
    }

    IEnumerator                     fadeDeath()
    {
        float                       time = 0;

        this.plant.controlled = false;
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
        SceneManager.LoadScene(1);
    }

    public void                     LaunchGameover()
    {
        StartCoroutine(fadeDeath());
    }

    void                            OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "gameOver")
            this.LaunchGameover();
    }
}
