using UnityEngine;

public class Vesicle : CellOrganelle
{
    public enum CargoType { Protein, Lipid, Waste }
    public CargoType cargoType;
    public Transform target;

    public float speed = 2f;

    void Update()
    {
        if (target != null)
        {
            MoveToTarget();
        }
    }

    void MoveToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            DeliverCargo();
        }
    }

    void DeliverCargo()
    {
        switch (cargoType)
        {
            case CargoType.Protein:
                //CellManager.Instance.AddProteins(5);
                break;
            case CargoType.Lipid:
               // CellManager.Instance.AddLipids(5);
                break;
            case CargoType.Waste:
                //CellManager.Instance.RemoveToxins(5);
                break;
        }
        Destroy(gameObject); // Vesicle is used up after delivery
    }
}
