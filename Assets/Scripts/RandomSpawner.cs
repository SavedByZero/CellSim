using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    [Header("Goodies")]
    public GameObject[] spawnPrefabs;

    [Header("How Many")]
    public int spawnCount = 10;

    [Header("Spawn Area (world space)")]
    public Vector2 minPosition = new Vector2(-5f, -5f);
    public Vector2 maxPosition = new Vector2(5f, 5f);

    void Start()
    {
        SpawnObjects();
    }

    void SpawnObjects()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            if (spawnPrefabs.Length == 0) return;

            GameObject prefab = spawnPrefabs[Random.Range(0, spawnPrefabs.Length)];
            Vector2 randomPos = new Vector2(
                Random.Range(minPosition.x, maxPosition.x),
                Random.Range(minPosition.y, maxPosition.y)
            );

            Instantiate(prefab, randomPos, Quaternion.identity, transform);
        }
    }
}
