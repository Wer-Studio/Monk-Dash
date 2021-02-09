using System.Collections;
using UnityEngine;

public class VectorN : Object
{
    public int Length; //Size
    public float[] values;

    /*-$-$-$-$-$-$-$-$-CONSTRUCTORS & INDEXERS-$-$-$-$-$-$-$-$-*/

    public VectorN(int size)
    {
        this.Length = size;
        this.values = new float[Length];
    }

    public float this[int i] //Be careful this does not look for errors
    {
        get
        {
            return values[i];
        }
        set
        {
            values[i] = value;
        }
    }

    /*-$-$-$-$-$-$-$-$-METHODS-$-$-$-$-$-$-$-$-*/

    public void print()
    {
        for (int i = 0; i < Length; i++)
        {
            Debug.Log(i + " -> " + values[i]);
        }
    }

    public float norme()
    {
        return ScalarProduct(this, this);
    }

    public static float ScalarProduct(VectorN v1, VectorN v2)
    {
        if (v1.Length == v2.Length)
        {
            float sum = 0;
            for (int i = 0; i < v1.Length; i++)
            {
                sum += v1[i] * v2[i];
            }
            Debug.Log(sum);
            return sum;
        }
        else
        {
            throw new System.Exception("VECTORS DIMENSIONS MUST AGREE");
        }
    }

    public static Vector3 CrossProduct(Vector3 u, Vector3 v)
    { //Dimension 3
        return new Vector3(u.y * v.z - u.z * v.y, u.z * v.x - u.x * v.z, u.x * v.y - u.y * v.x);
    }

    public static bool Orthogonal(VectorN v1, VectorN v2)
    {
        return VectorN.ScalarProduct(v1, v2) == 0;
    }

    /*-$-$-$-$-$-$-$-$-OPERATORS-$-$-$-$-$-$-$-$-*/

    //Elementwise sum
    public static VectorN operator +(VectorN v1, VectorN v2)
    {
        if (v1.Length == v2.Length)
        {
            VectorN v = new VectorN(v1.Length);
            for (int i = 0; i < v1.Length; i++)
            {
                v[i] = v1[i] + v2[i];
            }
            return v;
        }
        else
        {
            throw new System.Exception("VECTORS DIMENSIONS MUST AGREE");
        }
    }

    //Elementwise difference
    public static VectorN operator -(VectorN v1, VectorN v2)
    {
        if (v1.Length == v2.Length)
        {
            VectorN v = new VectorN(v1.Length);
            for (int i = 0; i < v1.Length; i++)
            {
                v[i] = v1[i] - v2[i];
            }
            return v;
        }
        else
        {
            throw new System.Exception("VECTORS DIMENSIONS MUST AGREE");
        }
    }
    public static VectorN operator *(VectorN v1, float f)
    {
        VectorN v = new VectorN(v1.Length);
        for (int i = 0; i < v1.Length; i++)
        {
            v[i] = v1[i] * f;
        }
        return v;
    }
    public static VectorN operator *(float f, VectorN v1)
    {
        VectorN v = new VectorN(v1.Length);
        for (int i = 0; i < v1.Length; i++)
        {
            v[i] = v1[i] * f;
        }
        return v;
    }

    public static VectorN operator /(VectorN v1, float f)
    {
        if (f != 0)
        {
            VectorN v = new VectorN(v1.Length);
            for (int i = 0; i < v1.Length; i++)
            {
                v[i] = v1[i] / f;
            }
            return v;
        }
        else
        {
            throw new System.Exception("ARRGH, DIVISION BY ZERO");
        }
    }

    public static bool operator ==(VectorN v1, VectorN v2)
    {
        if (v1.Length == v2.Length)
        {
            for (int i = 0; i < v1.Length; i++)
            {
                if (v1[i] != v2[i])
                {
                    return false;
                }
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool operator !=(VectorN v1, VectorN v2)
    {
        if (v1.Length == v2.Length)
        {
            for (int i = 0; i < v1.Length; i++)
            {
                if (v1[i] != v2[i])
                {
                    return true;
                }
            }
            return false;
        }
        else
        {
            return true;
        }
    }

}

