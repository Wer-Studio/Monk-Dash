using System.Collections;
using UnityEngine;

public class Interpolation : Object
{
    /**
     * CUBIC INTERPOLATION. http://archive.gamedev.net/archive/reference/articles/article1497.html
     * k(u) = k(0)×(2u3-3u2+1) + k(1)×(3u2-2u3)
     * A:= 2u3
     * B:= 3u2
     * k(u) = k(0)×(A-B+1) + k(1)×(B-A)
     * Where u is the step to interpolate between 0 and 1.
     * 
     * VectorN start is the value of the funcion at step 1 k(0).
     * VectorN end is the value of the funcion at step 1 k(1).
     * float step : is the step to interpolate between start and end (u).
     */
    public VectorN cubicInterpolation(VectorN start, VectorN end, float step)
    {
        if (start.Length == end.Length)
        {
            float A = 2 * Mathf.Pow(step, 3);
            float B = 3 * Mathf.Pow(step, 2);

            return (start*(A-B+1))+(end*(B-A));
        }
        else
        {
            throw new System.Exception("VECTORS DIMENSIONS MUST AGREE");
        }

    }
    /**
     * LINEAR INTERPOLATION. http://archive.gamedev.net/archive/reference/articles/article1497.html
     * k(0)×(1-u) + k(1)×(u)
     * 
     * Where u is the step to interpolate between 0 and 1.
     * 
     * VectorN start is the value of the funcion at step 1 k(0).
     * VectorN end is the value of the funcion at step 1 k(1).
     * float step : is the step to interpolate between start and end (u).
     */
    public VectorN linearInterpolation(VectorN start, VectorN end, float step)
    {
        return (start*(1-step))+(end*(step));
    }
