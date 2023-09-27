using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    public static float DistanceCheck2D(Transform transform1, Transform transform2)
    {
        float outcome;

        Vector2 pos = new Vector2(transform1.position.x, transform1.position.z);
        Vector2 player = new Vector2(transform2.position.x, transform2.position.z);

        outcome = Vector2.Distance(pos, player);

        return outcome;
    }

    public static bool IsBetween<T>(T value, T lowerBound, T upperBound) where T : IComparable<T>
    {
        return value.CompareTo(lowerBound) > 0 && value.CompareTo(upperBound) < 0;
    }

    public static bool IsBetween<T>(T value, T lowerBound, T upperBound, bool L_Equal, bool G_Equal) where T : IComparable<T>
    {
        if(L_Equal && G_Equal)
            return value.CompareTo(lowerBound) >= 0 && value.CompareTo(upperBound) <= 0;
        else if(L_Equal)
            return value.CompareTo(lowerBound) >= 0 && value.CompareTo(upperBound) < 0;
        else if(G_Equal)
            return value.CompareTo(lowerBound) > 0 && value.CompareTo(upperBound) <= 0;
        else
            return value.CompareTo(lowerBound) > 0 && value.CompareTo(upperBound) < 0;
    }

}
