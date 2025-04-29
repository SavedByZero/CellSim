using UnityEngine;

public class Cytoskeleton : CellOrganelle
{
    public float stabilityFactor = 0.1f;

    void Update()
    {
        ReinforceStructure();
    }

    void ReinforceStructure()
    {
        if (currentHealth > 0)
        {
           // CellManager.Instance.StabilizeCell(stabilityFactor);
        }
    }
}
