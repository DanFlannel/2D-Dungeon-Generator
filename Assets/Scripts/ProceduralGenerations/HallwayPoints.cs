using Delaunay.Geo;
using System.Collections.Generic;
using UnityEngine;

namespace DFC
{
    [System.Serializable]
    public class HallwayPoints
    {
        public LineSegment line;
        public Room r0;
        public Room r1;

        public Vector3 startingPoint;
        public Vector3 midPoint;
        public Vector3 endPoint;

        private int width;

        public HallwayPoints(LineSegment line, ref List<Room> rooms, int width)
        {
            this.line = line;
            this.width = width;

            for (int i = 0; i < rooms.Count; i++)
            {
                if (r0 != null && r1 != null) { break; }

                if (line.p0 == rooms[i].center)
                {
                    r0 = rooms[i];
                }

                if (line.p1 == rooms[i].center)
                {
                    r1 = rooms[i];
                }
            }

            startingPoint = line.p0.Value;
            endPoint = line.p1.Value;
            midPoint = new Vector2(((startingPoint.x - endPoint.x) / 2) + endPoint.x, ((startingPoint.y - endPoint.y) / 2) + endPoint.y);

            AdjustStartEndPoints();
            GenerateCorner();
        }

        private void AdjustStartEndPoints()
        {
            Vector2 adjStartingPoint = startingPoint;
            Vector2 adjEndPoint = endPoint;

            if (Mathf.Abs(startingPoint.x - midPoint.x) > Mathf.Abs(startingPoint.y - midPoint.x))
            {
                adjStartingPoint.x = (startingPoint.x > endPoint.x) ? adjStartingPoint.x - (r0.width / 2) : adjStartingPoint.x + (r0.width / 2);
            }
            else
            {
                adjStartingPoint.y = (startingPoint.y > endPoint.y) ? adjStartingPoint.y - (r0.height / 2) : adjStartingPoint.y + (r0.height / 2);
            }

            if (Mathf.Abs(endPoint.x - midPoint.x) > Mathf.Abs(endPoint.y - midPoint.y))
            {
                adjEndPoint.x = (endPoint.x > startingPoint.x) ? adjEndPoint.x - (r1.width / 2) : adjEndPoint.x + (r1.width / 2);
            }
            else
            {
                adjEndPoint.y = (endPoint.y > startingPoint.y) ? adjEndPoint.y - (r1.height / 2) : adjEndPoint.y + (r1.height / 2);
            }

            startingPoint = adjStartingPoint;
            endPoint = adjEndPoint;
        }

        private void GenerateCorner()
        {
            if (startingPoint.x - endPoint.x > width)
            {
                midPoint = new Vector2(endPoint.x, startingPoint.y);
            }
            else if (startingPoint.y - endPoint.y > width)
            {
                midPoint = new Vector2(startingPoint.x, endPoint.y);
            }
            else
            {
                midPoint = new Vector2(((startingPoint.x - endPoint.x) / 2) + startingPoint.x, ((startingPoint.y - endPoint.y) / 2) + startingPoint.y);
            }
        }
    }
}