using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class                        Trampoline : MonoBehaviour
{
    public Vector3                  destination = Vector3.zero;
    public float                    force;
    [Range(0, 1)]
    public float                    randomness;
    private Vector3                 calculatedDestination = Vector3.zero;

    void                            OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(this.transform.position + destination, 1f);
        Gizmos.DrawSphere(this.transform.position + calculatedDestination, 0.5f);
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(this.transform.position + (calculatedDestination * force), 0.2f);
    }

    void                            OnCollisionEnter2D(Collision2D collider)
    {
        Vector3                 random = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
        Vector3                 pushForce = (((this.transform.position + this.destination) - collider.transform.position).normalized + (random.normalized * randomness)).normalized;

        calculatedDestination = pushForce;
        collider.gameObject.GetComponent<Rigidbody2D>().AddForce(pushForce * force, ForceMode2D.Impulse);
    }
}
