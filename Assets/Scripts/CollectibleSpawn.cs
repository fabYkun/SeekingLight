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
    [SerializeField]
    private float               rainAngle;
    [SerializeField]
    private float               rainForce;
    [SerializeField]
    private float               waterPercentage;
    [SerializeField]
    private float               pollenPercentage;
    public float timer;

    void                        OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(transform.position, size);
    }

    // it will be more deterministic in a fixedupdate
    void                        FixedUpdate()
    {
        List<GameObject>        collectibles = new List<GameObject>();
        Vector2                 topLeft = this.transform.position - this.size / 2;
        Vector2                 bottomRight = this.transform.position + this.size / 2;
        

        if (Random.Range(0, 100) < this.waterPercentage)
            collectibles.Add(Instantiate(this.waterPrefab, new Vector2(Random.Range(topLeft.x, bottomRight.x), Random.Range(topLeft.y, bottomRight.y)), Quaternion.identity));
        if (Random.Range(0, 100) < this.pollenPercentage)
            collectibles.Add(Instantiate(this.pollenPrefab, new Vector2(Random.Range(topLeft.x, bottomRight.x), Random.Range(topLeft.y, bottomRight.y)), Quaternion.identity));
        for (int i = 0; i < collectibles.Count; ++i)
        {
            collectibles[i].transform.Rotate(Vector3.forward * this.rainAngle);
            collectibles[i].GetComponent<Rigidbody2D>().AddForce(collectibles[i].gameObject.transform.up * rainForce * - 1, ForceMode2D.Impulse);
            Destroy(collectibles[i], timer);
        }
    }
}
