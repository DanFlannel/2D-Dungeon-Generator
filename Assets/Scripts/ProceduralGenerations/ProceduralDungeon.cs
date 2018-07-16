using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Delaunay;
using Delaunay.Geo;

namespace DFC
{
    public class ProceduralDungeon : MonoBehaviour
    {
        private static ProceduralDungeon instance;
        public static ProceduralDungeon Instance { get { return instance; } }

        public static Dictionary<Vector2, GameObject> tileDict = new Dictionary<Vector2, GameObject>();
        public static Dictionary<Vector2, GameObject> wallDict = new Dictionary<Vector2, GameObject>();

        [Header("Holder")]
        public GameObject boardHolder;

        [Header("Base Size")]
        public float radius = 25f;
        public Vector2 center;
        public Vector2 size;

        [Header("Rooms")]
        public List<Room> primaryRooms = new List<Room>();
        public List<Room> secondaryRooms = new List<Room>();

        public Vector2 numOfRooms = new Vector2(35, 40);
        public Vector2 roomSize = new Vector2(25, 25);
        public int roomCount;

        [Header("Hallways")]
        public int hallwayWidth = 4;
        public List<Hallway> hallways = new List<Hallway>();
        public int hallwayCount;

        [Header("Sprites")]
        public GameObject[] floorTiles;
        public GameObject[] wallTiles;

        [Header("Tree")]
        public List<LineSegment> edges = null;
        public List<LineSegment> spanningTree;
        public List<LineSegment> delaunayTriangulation;


        private List<Room> allRooms = new List<Room>();
        private GameObject hallwayHolder;
        private GameObject roomHolder;

        private void Awake()
        {
            if(instance != null && instance != this) { Destroy(instance); }
            instance = this;
        }

        // Use this for initialization
        void Start()
        {
            hallwayHolder = new GameObject("Hallway Parent");
            roomHolder = new GameObject("Room Parent");
            GenerateDungeon();
        }

        public void GenerateDungeon()
        {
            StartCoroutine(GenerationCoroutine());
        }

        private Vector2 getRandomPointInCircle(float radius)
        {
            
            float t = 2f * Mathf.PI * Random.Range(0f, 1f);
            float u = Random.Range(0f, 1f) + Random.Range(0f, 1f);
            float r = (u > 1f) ? 2f - u : u;
            return new Vector2(radius*r * Mathf.Cos(t), radius*r * Mathf.Sin(t));
        }

        private void GenerateRooms()
        {
            int primaryRoomSize = (int)(roomSize.x * 1f) * (int)(roomSize.y * 1f);
            for (int i = 0; i < numOfRooms.RandomInt(); i++)
            {
                Room r = CreateRoom(getRandomPointInCircle(radius));
                int size = r.width * r.height;

                if(size >= primaryRoomSize) { primaryRooms.Add(r); }
                else {
                    secondaryRooms.Add(r);
                }
                allRooms.Add(r);
            }
        }

        private void DisableSecondaryRooms()
        {
            for(int i = 0; i < secondaryRooms.Count; i++)
            {
                secondaryRooms[i].gameObject.SetActive(false);
            }
        }

        private Room CreateRoom(Vector2 center)
        {
            GameObject go = new GameObject(string.Format("Room {0}", roomCount++));
            go.transform.SetParent(roomHolder.transform);
            Room r = go.AddComponent<Room>();

            int width = (int)(MathHelpers.GetBellCurvePoint(Random.Range(.25f, 1.5f), 1f) * roomSize.x);
            int height = (int)(MathHelpers.GetBellCurvePoint(Random.Range(.25f, 1.5f), 1f) * roomSize.y);

            width = (width < 4) ? 4 : width;
            height = (height < 4) ? 4 : height;

            //Debug.LogFormat("Width: {0} Height: {1}", width, height);
            r.SetupRoom(center, width, height);
            return r;
        }

        private Hallway CreateHallway(LineSegment line)
        {
            GameObject go = new GameObject(string.Format("Hallway {0}", hallwayCount++));
            go.transform.SetParent(hallwayHolder.transform);
            Hallway h = go.AddComponent<Hallway>();
            h.Generate(line, hallwayWidth);
            return h;
        }

        private void EnablePhysics(bool b)
        {
            for(int i = 0; i < primaryRooms.Count; i++)
            {
                primaryRooms[i].rigid.isKinematic = b;
            }

            for(int i = 0; i < secondaryRooms.Count; i++)
            {
                secondaryRooms[i].rigid.isKinematic = b;
            }
        }

        private void RoundPositions()
        {
            for(int i = 0; i < allRooms.Count; i++)
            {
                Vector2 center = allRooms[i].transform.position.RoundToInt();
                allRooms[i].transform.position = center;
                allRooms[i].center = center;
                allRooms[i].CalculateBounds();
            }
        }

        private void GetBounds()
        {
            int minX = 0;
            int maxX = 0;
            int minY = 0;
            int maxY = 0;

            for(int i = 0; i < allRooms.Count; i++)
            {
                minX = (minX < (int)allRooms[i].lowerLeft.x) ? minX :(int)allRooms[i].lowerLeft.x;
                maxX = (maxX > (int)allRooms[i].upperRight.x) ? maxX : (int)allRooms[i].upperRight.x;

                minY = (minY < (int)allRooms[i].lowerLeft.y) ? minY : (int)allRooms[i].lowerLeft.y;
                maxY = (maxY > (int)allRooms[i].upperRight.y) ? maxY : (int)allRooms[i].upperRight.y;
            }

            size = new Vector2((maxX - minX), (maxY - minY));
            center = new Vector2((Mathf.Abs(maxX) - Mathf.Abs(minX)) / 2f, (Mathf.Abs(maxY) - Mathf.Abs(minY)) / 2f);
        }

        private void VoronoiGeneration()
        {
            List<Vector2> points = new List<Vector2>();
            List<uint> colors = new List<uint>();

            for(int i = 0; i < primaryRooms.Count; i++)
            {
                points.Add(primaryRooms[i].center);
                colors.Add(0);
            }

            Delaunay.Voronoi v = new Delaunay.Voronoi(points, colors, new Rect(center,size));

            edges = v.VoronoiDiagram();
            spanningTree = v.SpanningTree(KruskalType.MINIMUM);
            delaunayTriangulation = v.DelaunayTriangulation();
        }

        private void GenerateHallways()
        {
            for(int i = 0; i < spanningTree.Count; i++)
            {
                CreateHallway(spanningTree[i]);
            }
        } //I need to connect the edges of the rooms, not the centers

        private IEnumerator GenerationCoroutine()
        {
            //.. Inital Generation

            GenerateRooms();

            //.. Spread Via Physics

            Time.timeScale = 10f;
            EnablePhysics(false);

            yield return new WaitForSeconds(15f);

            Time.timeScale = 1f;
            EnablePhysics(true);

            //.. Voroni Setup and Generation

            RoundPositions();
            GetBounds();
            VoronoiGeneration();

            //.. Hallways

            DisableSecondaryRooms();
            GenerateHallways();

        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            if (delaunayTriangulation != null)
            {
                for (int i = 0; i < delaunayTriangulation.Count; i++)
                {
                    Vector2 left = (Vector2)delaunayTriangulation[i].p0;
                    Vector2 right = (Vector2)delaunayTriangulation[i].p1;
                    Gizmos.DrawLine((Vector3)left, (Vector3)right);
                }
            }

            if (spanningTree != null)
            {
                Gizmos.color = Color.green;
                for (int i = 0; i < spanningTree.Count; i++)
                {
                    LineSegment seg = spanningTree[i];
                    Vector2 left = (Vector2)seg.p0;
                    Vector2 right = (Vector2)seg.p1;
                    Gizmos.DrawLine((Vector3)left, (Vector3)right);
                }
            }
        }
    }
}
