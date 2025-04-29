using UnityEngine;

public class EndoplasmicReticulum : CellOrganelle
{
    //Makes more membrane using the RNA - Ribosome procedure.  Increases maximum cell health 
    public enum Type { Rough, Smooth }
    public Type erType;
    public float productionRate = 2f;

    void Update()
    {
        if (erType == Type.Rough)
            ProduceProteins();
        else
            ProduceLipids();
    }

    void ProduceProteins()
    {
        if (currentHealth > 0)
        {
            //CellManager.Instance.AddProteins(productionRate * Time.deltaTime);
        }
    }

    void ProduceLipids()
    {
        if (currentHealth > 0)
        {
            //CellManager.Instance.AddLipids(productionRate * Time.deltaTime);
        }
    }
}
