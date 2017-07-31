using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlantSkin")]
public class                    PlantSkin : ScriptableObject
{
    [Tooltip("When the plant is bigger than minLength it can actually use that skin")]
    public int                  minLength = 0;

    public Sprite               head;
    public Sprite               sprout;
    public Sprite               stem;
    public Sprite               leftLeaf;
    public Sprite               rightLeaf;
}
