using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Delaunay.Geo;

namespace DFC
{
    public class Hallway : MonoBehaviour
    {
        public HallwayPoints points;

        private BoxCollider2D col1;
        private BoxCollider2D col2;

        private int width;

        public void Generate(HallwayPoints points, int width)
        {
            this.points = points;
            col1 = this.gameObject.AddComponent<BoxCollider2D>();

            this.width = width;


            //GenerateHallwayTiles();

            this.transform.position = points.midPoint;
        }

        private void GenerateHallwayTiles()
        {
            if (points.startingPoint.x - points.endPoint.x > width)
            {
                CreateXTiles(points.startingPoint, points.endPoint);
                CreateYTiles(points.midPoint, points.endPoint);
            }
            else if (points.startingPoint.y - points.endPoint.y > width)
            {
                CreateYTiles(points.startingPoint, points.midPoint);
                CreateXTiles(points.midPoint, points.startingPoint);
            }
            else
            {
                if (Mathf.Abs(points.startingPoint.x - points.endPoint.x) <= Mathf.Abs(points.startingPoint.y - points.endPoint.y))
                {
                    CreateXTiles(points.startingPoint, points.endPoint);
                }
                else if (Mathf.Abs(points.startingPoint.y - points.endPoint.y) > Mathf.Abs(points.startingPoint.y - points.endPoint.y))
                {
                    CreateYTiles(points.startingPoint, points.endPoint);
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
