using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class                                        PlantPower : MonoBehaviour
{
    public GameObject                               nodePrefab;
    public List<HingeJoint2D>                       joints = new List<HingeJoint2D>();
    HingeJoint2D                                    head;
    public int                                      size;
    public float                                    distance = 0.4f;
    public float                                    elasticity = 1;

    void                                            Awake()
    {
        this.joints.Add(this.GetComponentInChildren<HingeJoint2D>());
        for (int i = 0; i < size; ++i)
            AddNode();
    }

    public void                                     AddNode()
    {
        GameObject                                  tmp;
        HingeJoint2D                                lastJoint = this.joints[this.joints.Count - 1];
        HingeJoint2D                                currentJoint;

        tmp = Instantiate(nodePrefab, Vector3.zero, Quaternion.identity);
        tmp.transform.SetParent(this.transform);
        tmp.transform.position = lastJoint.transform.position + lastJoint.transform.up * distance;
        tmp.transform.up = lastJoint.transform.up;
        currentJoint = tmp.GetComponentInChildren<HingeJoint2D>();
        currentJoint.connectedBody = lastJoint.GetComponent<Rigidbody2D>();
        this.joints.Add(currentJoint);
    }

    void                                            Update()
    {
        if (Input.GetButtonDown("Spawn"))
            this.AddNode();
        
    }
}