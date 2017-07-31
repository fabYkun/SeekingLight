using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class                            LaunchScene : MonoBehaviour
{
    public float                        delay;
    public int                          sceneNb;
    private bool                        launched = false;


    void                                Start()
    {
        this.delay = Time.time + this.delay;
    }
	
	void                                Update()
    {
        if (!launched && Time.time >= delay)
        {
            SceneManager.LoadScene(this.sceneNb);
            this.launched = true;
        }
	}
}
