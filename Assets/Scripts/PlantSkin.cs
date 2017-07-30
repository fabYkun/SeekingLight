using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlantSkin")]
public class                    PlantSkin : ScriptableObject
{
    public Sprite               head;
    public Sprite               sprout;
    public Sprite               stem;
    public List<Sprite>         leftLeafs = new List<Sprite>();
    public List<Sprite>         rightLeafs = new List<Sprite>();
}
