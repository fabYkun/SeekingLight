using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class                        ConsommableDeath : MonoBehaviour
{
    void                            OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.CompareTag("terre") || collider.gameObject.CompareTag("eau") || collider.gameObject.CompareTag("pierre"))
            Destroy(this.gameObject);
    }
}
