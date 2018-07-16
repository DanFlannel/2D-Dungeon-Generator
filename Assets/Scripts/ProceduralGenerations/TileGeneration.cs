using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGeneration : MonoBehaviour {

    public static GameObject InstantiateFromArray(GameObject[] prefabs, Vector2 pos, Transform parent)
    {
        // Create a random index for the array.
        int randomIndex = UnityEngine.Random.Range(0, prefabs.Length);

        // The position to be instantiated at is based on the coordinates.
        Vector3 position = new Vector3(pos.x, pos.y, 0f);

        // Create an instance of the prefab from the random index of the array.
        GameObject tileInstance = Instantiate(prefabs[randomIndex], position, Quaternion.identity) as GameObject;

        // Set the tile's parent to the board holder.
        tileInstance.transform.parent = parent;
        return tileInstance;
    }

    public static GameObject InstantiateFromArrayLocal(GameObject[] prefabs, Vector2 pos, Transform parent)
    {
        // Create a random index for the array.
        int randomIndex = UnityEngine.Random.Range(0, prefabs.Length);

        // The position to be instantiated at is based on the coordinates.
        Vector3 position = new Vector3(pos.x, pos.y, 0f);

        // Create an instance of the prefab from the random index of the array.
        GameObject tileInstance = Instantiate(prefabs[randomIndex], Vector3.zero, Quaternion.identity) as GameObject;
        tileInstance.transform.localPosition = pos;
        // Set the tile's parent to the board holder.
        tileInstance.transform.parent = parent;
        return tileInstance;
    }

}
