using UnityEngine;

public class Endosome : CellOrganelle
{
    public Transform golgiApparatus;
    public Transform lysosome;
    public Transform plasmaMembrane;

    public void SortVesicle(Vesicle vesicle)
    {
        switch (vesicle.cargoType)
        {
            case Vesicle.CargoType.Protein:
                vesicle.target = golgiApparatus;
                break;
            case Vesicle.CargoType.Lipid:
                vesicle.target = plasmaMembrane;
                break;
            case Vesicle.CargoType.Waste:
                vesicle.target = lysosome;
                break;
        }
    }
}
