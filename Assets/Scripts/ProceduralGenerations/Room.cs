using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DFC
{
    [System.Serializable]
    public class Room : MonoBehaviour
    {
        public int height;
        public int width;
        public Vector2 center;

        [Header("Bounds")]
        public Vector2 lowerLeft;
        public Vector2 upperRight;
        public Transform parent;
        public BoxCollider2D coll;
        public Rigidbody2D rigid;

        public void SetupRoom(Vector2 center, int height, int width)
        {
            this.height = (height % 2 != 0) ? height + 1 : height;
            this.width = (width % 2 != 0) ? width + 1 : width;
            this.center = new Vector2(Mathf.Floor(center.x), Mathf.Floor(center.y));

            SetupPhysicsCollisions();
            SetCollider();
            CalculateBounds();
            EnableTiles();
        }

        public void CalculateBounds()
        {

            int halfHeight = height / 2;
            int halfWidth = width / 2;

            lowerLeft = center;
            lowerLeft.x -= (width % 2 != 0) ? halfWidth + 1 : halfWidth;
            lowerLeft.y -= (height % 2 != 0) ? halfHeight + 1 : halfHeight;

            upperRight = center;
            upperRight.x += halfWidth;
            upperRight.y += halfHeight;
        }

        private void SetCollider()
        {
            coll.transform.position = center;
            coll.offset = new Vector2(.5f, .5f);
            coll.size = new Vector2(width, height);
        }

        private void SetupPhysicsCollisions()
        {
            coll = this.gameObject.AddComponent<BoxCollider2D>();
            rigid = this.gameObject.AddComponent<Rigidbody2D>();
            rigid.isKinematic = true;
            rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
            rigid.gravityScale = 0f;
        }

        public void EnableTiles()
        {
            for(int row = 0; row < width; row++)
            {
                for(int col = 0; col < height; col++)
                {
                    Vector2 pos = upperRight;
                    pos.x -= row;
                    pos.y -= col;

                    InstantiateFromArray(ProceduralDungeon.Instance.floorTiles, pos);
                }
            }
        }

        private GameObject InstantiateFromArray(GameObject[] prefabs, Vector2 pos)
        {
            // Create a random index for the array.
            int randomIndex = UnityEngine.Random.Range(0, prefabs.Length);

            // The position to be instantiated at is based on the coordinates.
            Vector3 position = new Vector3(pos.x, pos.y, 0f);

            // Create an instance of the prefab from the random index of the array.
            GameObject tileInstance = Instantiate(prefabs[randomIndex], position, Quaternion.identity) as GameObject;

            // Set the tile's parent to the board holder.
            tileInstance.transform.parent = this.transform;
            return tileInstance;
        }
    }
}
