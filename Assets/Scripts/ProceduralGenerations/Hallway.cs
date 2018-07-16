using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Delaunay.Geo;

public class Hallway : MonoBehaviour {

    [Header("Info")]
    public Vector2 startingPoint;
    public Vector2 endpoint;
    public Vector2 cornerPoint;

    private BoxCollider col1;
    private BoxCollider col2;

    private int width;

    public void Generate(LineSegment line, int width)
    {
        startingPoint = line.p0.Value;
        endpoint = line.p1.Value;

        this.width = width;

        if (startingPoint.x - endpoint.x > width || startingPoint.y - endpoint.y > width)
        {
            //create corner point
        }
    }
}
