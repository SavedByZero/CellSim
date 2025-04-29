using UnityEngine;

public class GolgiApparatus : CellOrganelle
{
    public float proteinProcessingRate = 3f;

    void Update()
    {
        ProcessProteins();
    }

    void ProcessProteins()
    {
        if (currentHealth > 0)
        {
            //CellManager.Instance.AddProteins(proteinProcessingRate * Time.deltaTime);
        }
    }
}
