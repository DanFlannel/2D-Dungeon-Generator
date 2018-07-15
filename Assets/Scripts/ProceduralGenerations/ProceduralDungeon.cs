using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        public Vector2 baseCenter = new Vector2(50, 50);
        public float radius = 25f;
        public int rows = 100;
        public int columns = 100;

        [Header("Rooms")]

        public List<Room> primaryRooms = new List<Room>();
        public List<Room> secondaryRooms = new List<Room>();

        public Vector2 numOfRooms;
        public Vector2 roomSize = new Vector2(6,10);
        public int roomCount;

        [Header("Sprites")]
        public GameObject[] floorTiles;
        public GameObject[] wallTiles;

        private void Awake()
        {
            if(instance != null && instance != this) { Destroy(instance); }
            instance = this;
        }

        // Use this for initialization
        void Start()
        {
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
            int primaryRoomSize = (int)(roomSize.x * 1.15f) * (int)(roomSize.y * 1.15f);
            for (int i = 0; i < numOfRooms.RandomInt(); i++)
            {
                Room r = CreateRoom(getRandomPointInCircle(radius));
                int size = r.width * r.height;

                if(size >= primaryRoomSize) { primaryRooms.Add(r); }
                else { secondaryRooms.Add(r); }
            }
        }

        private Room CreateRoom(Vector2 center)
        {
            GameObject go = new GameObject(string.Format("Room {0}", roomCount++));
            go.transform.SetParent(boardHolder.transform);
            Room r = go.AddComponent<Room>();

            int width = (int)(MathHelpers.GetBellCurvePoint(Random.Range(.25f, 1.5f), 1f) * roomSize.x);
            int height = (int)(MathHelpers.GetBellCurvePoint(Random.Range(.25f, 1.5f), 1f) * roomSize.y);

            width = (width < 4) ? 4 : width;
            height = (height < 4) ? 4 : height;

            //Debug.LogFormat("Width: {0} Height: {1}", width, height);
            r.SetupRoom(center, width, height);
            return r;
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

        private IEnumerator GenerationCoroutine()
        {
            GenerateRooms();

            Time.timeScale = 10f;
            EnablePhysics(false);

            yield return new WaitForSeconds(15f);

            Time.timeScale = 1f;
            EnablePhysics(true);



        }

    }
}
