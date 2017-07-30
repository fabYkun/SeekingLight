using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class                                        PlantPower : MonoBehaviour
{
    public bool                                     controlled = true; // current plant under control
    public GameObject                               nodePrefab;
    public GameObject                               detachedHeadPrefab;
    public PlantSkin                                skin;

    private List<HingeJoint2D>                      joints = new List<HingeJoint2D>();
    HingeJoint2D                                    head;
    public int                                      size;
    public float                                    distance = 0.4f;
    public float                                    elasticity = 1;
    public int                                      controlledNodes = 1; // number of nodes you actually control
    public AnimationCurve                           powerDistribution; // force given to manual nodes in descending order
    public float                                    maxPower; // maximal power

    void                                            Awake()
    {
        int                                         controlledNodes = this.controlledNodes;

        this.joints.Add((this.head = this.GetComponentInChildren<HingeJoint2D>()));
        for (int i = 0; i < size; ++i)
            AddNode();
        this.controlledNodes = controlledNodes; // to modify
    }

    public void                                     AddNode()
    {
        GameObject                                  tmp;
        HingeJoint2D                                lastJoint = this.joints[this.joints.Count - 1];
        HingeJoint2D                                currentJoint;

        tmp = Instantiate(nodePrefab, Vector3.zero, Quaternion.identity);
        tmp.GetComponent<SpriteRenderer>().sprite = this.skin.head;
        tmp.transform.SetParent(this.transform);
        tmp.transform.position = lastJoint.transform.position;
        if (this.joints.Count > 1) tmp.transform.position += lastJoint.transform.up * distance;
        tmp.transform.up = lastJoint.transform.up;
        currentJoint = tmp.GetComponentInChildren<HingeJoint2D>();
        currentJoint.connectedBody = lastJoint.GetComponent<Rigidbody2D>();
        ++controlledNodes; // maybe not
        lastJoint.GetComponent<SpriteRenderer>().sprite = (this.joints.Count == 1 ? this.skin.sprout : this.skin.stem);
        this.head = currentJoint;
        this.joints.Add(currentJoint);
    }

    public void                                     DetachHead()
    {
        HingeJoint2D                                lastJoint = this.joints[this.joints.Count - 1];
        HingeJoint2D                                newLastJoint;
        GameObject                                  detachedHead;

        this.joints.RemoveAt(this.joints.Count - 1);
        Destroy(lastJoint.gameObject);
        newLastJoint = this.joints[this.joints.Count - 1];
        newLastJoint.connectedBody = null;
        detachedHead = Instantiate(detachedHeadPrefab, this.head.transform.position, Quaternion.identity);
        detachedHead.transform.up = this.head.transform.up;
        this.controlled = false;
    }

    void                                            Inputs()
    {
        if (Input.GetButtonDown("Spawn"))
            this.AddNode();
        if (Input.GetButtonDown("Left") || Input.GetButtonDown("Right"))
        {
            for (int i = 0; i < controlledNodes; ++i)
            {
                HingeJoint2D joint = this.joints[this.joints.Count - (i + 1)];
                JointMotor2D motor = joint.motor;
                motor.motorSpeed = this.powerDistribution.Evaluate((float) i / controlledNodes) * this.maxPower;
                if (Input.GetButtonDown("Left")) motor.motorSpeed = -motor.motorSpeed;
                joint.motor = motor;
            }
        }
        if (Input.GetButtonDown("Propulse"))
            this.DetachHead();
    }

    void                                            Update()
    {
        if (this.controlled) this.Inputs();
    }
}