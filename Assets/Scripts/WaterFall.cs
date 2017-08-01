using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class                        WaterFall : MonoBehaviour
{
    private MeshRenderer            mr;
    public float                    fallSpeed;

    void                            Start()
    {
        this.mr = this.GetComponent<MeshRenderer>();
    }

    void                            Update()
    {
        this.mr.material.mainTextureOffset = this.mr.material.mainTextureOffset + new Vector2(0, fallSpeed * Time.deltaTime);
    }
}
