using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Extensions
{

    public static Vector3 RoundToInt(this Vector3 vector3)
    {
        vector3.x = Mathf.RoundToInt(vector3.x);
        vector3.y = Mathf.RoundToInt(vector3.y);
        vector3.z = Mathf.RoundToInt(vector3.z);
        return vector3;
    }

}
