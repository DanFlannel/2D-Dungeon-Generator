using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Delaunay.Geo;

namespace DFC
{
    public class Hallway : MonoBehaviour
    {

        [Header("Info")]
        public Vector2 startingPoint;
        public Vector2 endpoint;
        public Vector2 cornerPoint;

        private BoxCollider2D col1;
        private BoxCollider2D col2;

        private int width;

        public void Generate(LineSegment line, int width)
        {
            startingPoint = line.p0.Value;
            endpoint = line.p1.Value;

            col1 = this.gameObject.AddComponent<BoxCollider2D>();

            this.width = width;


            GenerateHallwayTiles();

            this.transform.position = startingPoint;
        }

        private void GenerateHallwayTiles()
        {
            if (startingPoint.x - endpoint.x > width)
            {
                cornerPoint = new Vector2(endpoint.x, startingPoint.y);
                CreateXTiles(startingPoint, cornerPoint);
                CreateYTiles(cornerPoint, endpoint);
            }
            else if (startingPoint.y - endpoint.y > width)
            {
                cornerPoint = new Vector2(startingPoint.x, endpoint.y);
                CreateYTiles(startingPoint, cornerPoint);
                CreateXTiles(cornerPoint, endpoint);
            }
            else
            {
                cornerPoint = Vector2.zero;
                if (Mathf.Abs(startingPoint.x - endpoint.x) <= Mathf.Abs(startingPoint.y - endpoint.y))
                {
                    CreateXTiles(startingPoint, endpoint);
                }
                else if (Mathf.Abs(startingPoint.y - endpoint.y) > Mathf.Abs(startingPoint.y - endpoint.y))
                {
                    CreateYTiles(startingPoint, endpoint);
                }
            }
        }

        private void CreateXTiles(Vector2 p1, Vector2 p2)
        {
            for (int x = 0; x < Mathf.Abs(p1.x - p2.x); x++)
            {
                for (int y = 0; y < width; y++)
                {
                    Vector2 pos = new Vector2(x, y);
                    TileGeneration.InstantiateFromArray(ProceduralDungeon.Instance.floorTiles, pos, this.transform);
                }
            }
        }

        private void CreateYTiles(Vector2 p1, Vector2 p2)
        {
            for (int y = 0; y < Mathf.Abs(p1.y - p2.y); y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Vector2 pos = new Vector2(x, y);
                    TileGeneration.InstantiateFromArray(ProceduralDungeon.Instance.floorTiles, pos, this.transform);
                }
            }
        }
    }
}
