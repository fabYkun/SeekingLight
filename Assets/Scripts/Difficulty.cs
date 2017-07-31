using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class                        Difficulty : MonoBehaviour
{
	void                            Awake()
    {
        Time.timeScale = 0;
    }
	
	public void                     Easy()
    {
        GameOver.instance.difficultyMode = DifficultyEnum.Easy;
        Time.timeScale = 1;
        this.gameObject.SetActive(false);
    }

    public void                     Normal()
    {
        GameOver.instance.difficultyMode = DifficultyEnum.Normal;
        Time.timeScale = 1;
        this.gameObject.SetActive(false);
    }

    public void                     Difficult()
    {
        GameOver.instance.difficultyMode = DifficultyEnum.Hard;
        Time.timeScale = 1;
        this.gameObject.SetActive(false);
    }
}
