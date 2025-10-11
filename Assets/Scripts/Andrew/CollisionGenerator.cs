using UnityEngine;
using UnityEngine.Tilemaps;

public class ColliderGenerator : MonoBehaviour
{
    public Tilemap tilemap; // Assign your Tilemap here
    public GameObject collisionPrefab; // Prefab with Collider2D

    void Start()
    {
        GenerateCollisions();
    }

    void GenerateCollisions()
    {
        if (tilemap == null)
        {
            Debug.LogError("Tilemap is not assigned!");
            return;
        }

        if (collisionPrefab == null)
        {
            Debug.LogError("Collision prefab is not assigned!");
            return;
        }

        // Create a new empty GameObject to act as the parent for all spawned colliders.
        GameObject allCollidersParent = new GameObject("Tilemap Colliders");
        allCollidersParent.transform.position = Vector3.zero;

        BoundsInt bounds = tilemap.cellBounds;

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                TileBase tile = tilemap.GetTile(tilePosition);

                if (tile != null) // Only place colliders where there are tiles
                {
                    Vector3 worldPosition = tilemap.GetCellCenterWorld(tilePosition);

                    // Instantiate the prefab.
                    GameObject newCollider = Instantiate(collisionPrefab, worldPosition, Quaternion.identity);

                    // Set the newly instantiated prefab's parent to the single parent object.
                    newCollider.transform.SetParent(allCollidersParent.transform);
                }
            }
        }

        Debug.Log("Tilemap collision generation complete.");
    }
}
