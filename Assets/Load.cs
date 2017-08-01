using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Load: MonoBehaviour {

    public string name;

    void OnTriggerEnter2D (Collider2D col) {
        if ( col.gameObject.tag == "player" )
            SceneManager.LoadScene (name, LoadSceneMode.Single);
        }
        }
