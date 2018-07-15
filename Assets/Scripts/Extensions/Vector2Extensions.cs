using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Vector2Extensions
{
    public static float RandomFloat (this Vector2 vector2)
    {
        return UnityEngine.Random.Range(vector2.x, vector2.y);
    }

    public static int RandomInt(this Vector2 vector2)
    {
        return (int)RandomFloat(vector2);
    }

    public static Vector2 mult(this Vector2 v1, Vector2 v2)
    {
        Vector2 final;
        final.x = v1.x * v2.x;
        final.y = v1.y * v2.y;
        return final;
    }
}