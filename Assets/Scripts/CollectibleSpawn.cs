using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class                    CollectibleSpawn : MonoBehaviour
{
    [SerializeField]
    private Vector3             size;
    [SerializeField]
    private GameObject          waterPrefab;
    [SerializeField]
    private GameObject          pollenPrefab;

    void                        OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(transform.position, size);
    }
}
