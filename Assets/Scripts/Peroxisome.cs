using UnityEngine;

public class Peroxisome : CellOrganelle
{
    public float detoxRate = 5f;
    public float lipidBreakdownRate = 3f;

    void Update()
    {
        Detoxify();
        BreakDownLipids();
    }

    void Detoxify()
    {
        if (currentHealth > 0)
        {
            //CellManager.Instance.RemoveToxins(detoxRate * Time.deltaTime);
        }
    }

    void BreakDownLipids()
    {
        if (currentHealth > 0)
        {
            //CellManager.Instance.AddEnergy(lipidBreakdownRate * Time.deltaTime);
        }
    }
}
