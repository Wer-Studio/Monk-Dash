using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bone : Object
{
    public float Rotation;
    public int Length; // VectorN length
    public VectorN position;

    /*-$-$-$-$-$-$-$-$-CONSTRUCTORS & INDEXERS-$-$-$-$-$-$-$-$-*/

    public Bone(int size, float Rotation)
    {
        this.Length = size;
        this.Rotation = Rotation;
        this.position = new VectorN(Length);
    }
}
