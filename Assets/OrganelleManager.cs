using UnityEngine;

public class OrganelleManager : MonoBehaviour
{
    public GameObject nucleusPrefab;
    public GameObject mitochondrionPrefab;
    public int mitochondriaCount = 5;

    void Start()
    {
        SpawnNucleus();
        SpawnMitochondria();
    }

    void SpawnNucleus()
    {
        Instantiate(nucleusPrefab, transform.position, Quaternion.identity, transform);
    }

    void SpawnMitochondria()
    {
        for (int i = 0; i < mitochondriaCount; i++)
        {
            Vector2 randomPos = Random.insideUnitCircle * 1.5f;
            Instantiate(mitochondrionPrefab, randomPos, Quaternion.identity, transform);
        }
    }
}
