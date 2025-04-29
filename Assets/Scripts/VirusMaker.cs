using UnityEngine;

public class VirusMaker : MonoBehaviour
{
    public bool AutoStart;
    public int Spread = 3;
    public Vector2[] Locations;
    public InjectorVirus InjectorPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (AutoStart)
            MakeViruses(Spread);
    }

    public void MakeViruses(int count)
    {
        for(int i=0; i < count; i++)
        {
            InjectorVirus virus = Instantiate(InjectorPrefab);
            virus.transform.position = Locations[Random.Range(0, Locations.Length)];
            virus.transform.SetParent(this.transform.parent);
            virus.Attack();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
