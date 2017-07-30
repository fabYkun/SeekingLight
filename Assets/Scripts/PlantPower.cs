﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class                                        PlantPower : MonoBehaviour
{
    public bool                                     controlled = true; // current plant under control
    public GameObject                               nodePrefab;
    public GameObject                               detachedHeadPrefab;
    public PlantSkin                                skin;

    private List<HingeJoint2D>                      joints = new List<HingeJoint2D>();
    private HingeJoint2D                            head;
    public int                                      size;
    public float                                    distance = 0.4f;
    public float                                    elasticity = 60;
    public AnimationCurve                           rigidDistribution;
    public int                                      controlledNodes = 1; // number of nodes you actually control
    public AnimationCurve                           powerDistribution; // force given to manual nodes in descending order
    public float                                    maxPower; // maximal power

    [Header("Force attributes")]
    public float                                    swapDelay = 0.1f;
    public Vector3                                  currentForce = Vector3.zero;
    public Vector3                                  targetForce = Vector3.zero;
    public Vector3                                  forceVelocity = Vector3.zero;

    //hugo
    PollenManagement                                pM;
    WaterManagement                                 wM;
    public float pollenReserve;
    public float waterReserve;

    void                                            Awake()
    {
        pM = FindObjectOfType<PollenManagement>();
        pM.SetPollenGauge((int)pollenReserve);
        wM = FindObjectOfType<WaterManagement>();
        wM.SetWaterGauge(waterReserve); 

        // fin hugo

        int                                         controlledNodes = this.controlledNodes;

        this.joints.Add((this.head = this.GetComponentInChildren<HingeJoint2D>()));
        for (int i = 0; i < size; ++i)
            AddNode();
        this.controlledNodes = controlledNodes; // to modify
        this.targetForce = new Vector3(this.powerDistribution.Evaluate(0.33f), this.powerDistribution.Evaluate(0.66f), this.powerDistribution.Evaluate(1));
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

    IEnumerator                                     scaleUp(Transform transform, Vector3 from, Vector3 to, float duration)
    {
        float                                       t = 0;

        while (t < duration)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(from, to, t / duration);
            yield return null;
        }
    }

    public void                                     DetachHead()
    {
        HingeJoint2D                                lastJoint = this.joints[this.joints.Count - 1];
        HingeJoint2D                                newLastJoint;
        GameObject                                  detachedHead;

        this.joints.RemoveAt(this.joints.Count - 1);
        Destroy(lastJoint.gameObject);
        newLastJoint = this.joints[this.joints.Count - 1];
        newLastJoint.GetComponent<SpriteRenderer>().sprite = this.skin.head;
        StartCoroutine(scaleUp(newLastJoint.transform, new Vector3(0.2f, 0.2f), Vector3.one, 2));
        detachedHead = Instantiate(detachedHeadPrefab, this.head.transform.position, Quaternion.identity);
        detachedHead.transform.up = this.head.transform.up;
        this.controlled = false;
    }
    
    void                                            Inputs()
    {
        if (Input.GetButtonDown("Spawn"))
            this.AddNode();
        else if (Input.GetButtonDown("Propulse"))
            this.DetachHead();

        int                                     j = 0;
        int                                     firstTier = controlledNodes / 3;
        int                                     secondTier = firstTier * 2;
        int                                     thirdTier = controlledNodes;

        if (Input.GetButton("Left"))
            this.currentForce = Vector3.SmoothDamp(this.currentForce, -this.targetForce, ref this.forceVelocity, this.swapDelay);
        else if (Input.GetButton("Right"))
            this.currentForce = Vector3.SmoothDamp(this.currentForce, this.targetForce, ref this.forceVelocity, this.swapDelay);
        else
            this.currentForce = Vector3.SmoothDamp(this.currentForce, Vector3.zero, ref this.forceVelocity, this.swapDelay);
        for (int i = this.joints.Count - controlledNodes; i < this.joints.Count; i = ++j)
        {
            HingeJoint2D joint = this.joints[i];
            JointMotor2D motor = joint.motor;
            motor.motorSpeed = ((j < firstTier) ? this.currentForce.x : ((j < secondTier) ? this.currentForce.y : this.currentForce.z)) * this.maxPower;
            joint.motor = motor;
        }
    }

    void                                            Update()
    {
        if (this.controlled) this.Inputs();
    }

    void                                            FixedUpdate()
    {
        for (int i = 0; i < this.joints.Count; ++i)
        {
            HingeJoint2D                            joint = this.joints[i];
            JointMotor2D                            motor = joint.motor;
            JointAngleLimits2D                      angles = joint.limits;

            angles.min = -this.elasticity * this.rigidDistribution.Evaluate((float) i / this.joints.Count);
            angles.max = this.elasticity * this.rigidDistribution.Evaluate((float) i / this.joints.Count);
            joint.limits = angles;
            joint.motor = motor;
        }
        if (this.controlled) this.Inputs();
        wM.SetWaterGauge(waterReserve); 
    }
}