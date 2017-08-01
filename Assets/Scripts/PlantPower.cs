using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class                                        PlantPower : MonoBehaviour
{
    public bool                                     controlled = true;      // current plant under control
    public GameObject                               nodePrefab;
    public GameObject                               detachedHeadPrefab;
    public GameObject                               leafPrefab;
    public PlantSkin                                defaultSkin;
    public PlantSkin[]                              upgradeSkins;
    public PlantSkin                                skin;

    private List<HingeJoint2D>                      joints = new List<HingeJoint2D>();
    private HingeJoint2D                            head;

    public int                                      currentSize;
    public int                                      maxSize;
    public int                                      detachHeadSacrifice = 6;

    [Tooltip("water consumption factor")]
    public float                                    waterConsumptionPerSec = 0.5f;
    [Tooltip("size-related water consumption, x = 0 = size 0 and x = 1 = maxSize")]
    public AnimationCurve                           maxSizeRatioWaterConsumtion;

    [Range(0, 100), Tooltip("Bonus leafs rate")]
    public float                                    leafProbability = 25;
    private int                                     noLeafSince = 0;
    public int                                      maximalLeafGap = 5;
    [Tooltip("distance between each node")]
    public float                                    distance = 0.4f;
    [Tooltip("widest angle used on the nodes")]
    public float                                    elasticity = 60;
    public AnimationCurve                           rigidDistribution;
    [Tooltip("number of nodes you actually control")]
    public int                                      controlledNodes = 1;
    [Tooltip("force given to manual nodes in descending order")]
    public AnimationCurve                           powerDistribution;
    [Tooltip("power corresponding to Y(x) = 1 on the powerDistribution")]
    public float                                    maxPower;

    [Header("Force attributes")]
    public float                                    swapDelay = 0.1f;
    [SerializeField]
    private Vector3                                 currentForce = Vector3.zero;
    [SerializeField]
    private Vector3                                 targetForce = Vector3.zero;
    [SerializeField]
    private Vector3                                 forceVelocity = Vector3.zero;
    [SerializeField]
    private Vector3                                 momentum;
    [Tooltip("when detached the head will keep the momentum of the plant, this factor exagerates it")]
    public float                                    momentumFactor;
    private Vector3                                 oldPosition = Vector3.zero;

    [Header("Canvas")]
    public CollectibleManagement                    pollenGauge;
    public CollectibleManagement                    waterGauge;
    public float                                    pollenReserve;
    public float                                    waterReserve;
    public float                                    waterAmount = 1;
    public float                                    pollenAmount = 1;

    public SimpleAudioEvent                         GrowthAudio;
    public SimpleAudioEvent                         PollenAudio;
    public SimpleAudioEvent                         EjectAudio;
    public AudioSource                              audioSource;

    void                                            Awake()
    {
        int                                         controlledNodes = this.controlledNodes;
        int                                         oldCurrentSize = this.currentSize;

        this.audioSource = this.GetComponent<AudioSource>();
        this.skin = this.defaultSkin;
        this.currentSize = 0;
        this.joints.Add((this.head = this.GetComponentInChildren<HingeJoint2D>()));
        for (int i = 0; i < oldCurrentSize; ++i)
            AddNode();
        this.controlledNodes = controlledNodes; // to modify
        this.targetForce = new Vector3(this.powerDistribution.Evaluate(0.33f), this.powerDistribution.Evaluate(0.66f), this.powerDistribution.Evaluate(1));
    }

    void                                            Start()
    {
        GameOver.instance.plant = this;
        this.pollenGauge.SetGauge((int)pollenReserve);
        this.waterGauge.SetGauge(waterReserve); 
    }

    public void                                     AddNode()
    {
        GameObject                                  tmp;
        HingeJoint2D                                lastJoint = this.joints[this.joints.Count - 1];
        HingeJoint2D                                currentJoint;

        if (this.currentSize + 1 > this.maxSize) return;
        this.GrowthAudio.Play(this.audioSource);
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
        Destroy(lastJoint.GetComponent<PolygonCollider2D>());
        Destroy(lastJoint.GetComponent<Leaf>());
        this.head = currentJoint;
        this.head.gameObject.AddComponent<PolygonCollider2D>();
        this.head.gameObject.AddComponent<Leaf>().Initialize(this, this.waterAmount, this.pollenAmount);
        this.joints.Add(currentJoint);
        if (Random.Range(0, 100) < this.leafProbability || ++this.noLeafSince > this.maximalLeafGap)
        {
            GameObject leaf = Instantiate(leafPrefab, Vector3.zero, Quaternion.identity);
            leaf.GetComponent<SpriteRenderer>().sprite = (Random.Range(0, 2) % 2 == 0 ? this.skin.leftLeaf : this.skin.rightLeaf);
            leaf.GetComponent<Leaf>().Initialize(this);
            leaf.transform.parent = currentJoint.transform;
            leaf.transform.localPosition = Vector3.zero;
            this.noLeafSince = 0;
        }
        ++this.currentSize;
        this.SkinMutation();
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
        HingeJoint2D                                lastJoint;
        GameObject                                  detachedHead;

        if (this.currentSize < detachHeadSacrifice + 2) return;
        this.EjectAudio.Play(this.audioSource);
        detachedHead = Instantiate(detachedHeadPrefab, this.head.transform.position, Quaternion.identity);
        detachedHead.transform.up = this.head.transform.up;
        detachedHead.GetComponent<Rigidbody2D>().AddForce(this.momentum * 4, ForceMode2D.Impulse);
        for (int i = 0; i < detachHeadSacrifice; ++i)
        {
            lastJoint = this.joints[this.joints.Count - 1];
            this.joints.RemoveAt(this.joints.Count - 1);
            --this.controlledNodes;
            --this.currentSize;
            Destroy(lastJoint.gameObject);
        }
        this.head = this.joints[this.joints.Count - 1];
        this.head.GetComponent<SpriteRenderer>().sprite = this.skin.head;
        StartCoroutine(scaleUp(this.head.transform, this.head.transform.localScale, Vector3.one, 2));
        detachedHead.GetComponent<Seed>().Initialize(this);
        this.pollenReserve = 0;
        this.controlled = false;
        this.SkinMutation();
    }
    
    void                                            Inputs()
    {
        if (Input.GetButton("Left"))
            this.currentForce = Vector3.SmoothDamp(this.currentForce, -this.targetForce, ref this.forceVelocity, this.swapDelay);
        else if (Input.GetButton("Right"))
            this.currentForce = Vector3.SmoothDamp(this.currentForce, this.targetForce, ref this.forceVelocity, this.swapDelay);
        else
            this.currentForce = Vector3.SmoothDamp(this.currentForce, Vector3.zero, ref this.forceVelocity, this.swapDelay);

        if (Input.GetButtonDown("Spawn"))
            this.AddNode();
        else if (Input.GetButtonDown("Propulse"))
            this.DetachHead();
    }
    
    void                                            SkinMutation()
    {
        PlantSkin                                   bestSkin = this.defaultSkin;

        for (int i = 0; i < this.upgradeSkins.Length; ++i)
            if (this.upgradeSkins[i].minLength < this.currentSize && this.upgradeSkins[i].minLength > bestSkin.minLength)
                bestSkin = this.upgradeSkins[i];
        this.skin = bestSkin;
    }

    void                                            Update()
    {
        this.waterReserve -= this.waterConsumptionPerSec * Time.deltaTime * this.maxSizeRatioWaterConsumtion.Evaluate(this.currentSize / this.maxSize);
        if (this.controlled == true)
        {
            this.Inputs();
            Camera.instance.Follow(this.transform);
            Camera.instance.adjust = new Vector3(0, 3, 0);
        }
    }

    void                                            Danse()
    {
        int                                         j = 0;
        int                                         firstTier = controlledNodes / 3;
        int                                         secondTier = firstTier * 2;
        for (int i = this.joints.Count - controlledNodes; i < this.joints.Count; ++i)
        {
            ++j;
            HingeJoint2D joint = this.joints[i];
            JointMotor2D motor = joint.motor;
            motor.motorSpeed = ((j < firstTier) ? this.currentForce.x : ((j < secondTier) ? this.currentForce.y : this.currentForce.z)) * this.maxPower;
            joint.motor = motor;
        }
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
        Danse();
    }

    void                                            LateUpdate()
    {
        if (!this.controlled) return;
        this.waterGauge.SetGauge(waterReserve);
        this.pollenGauge.SetGauge(pollenReserve);
        this.momentum = this.head.transform.position - this.oldPosition;
        this.oldPosition = this.head.transform.position;
        if (this.waterReserve <= 0) GameOver.instance.LaunchGameover();
    }

    public void                                     Initialize(CollectibleManagement waterGauge, CollectibleManagement pollenGauge)
    {
        this.waterGauge = waterGauge;
        this.pollenGauge = pollenGauge;
        this.waterReserve = waterGauge.GetValue();
        this.pollenReserve = pollenGauge.GetValue();
    }
}