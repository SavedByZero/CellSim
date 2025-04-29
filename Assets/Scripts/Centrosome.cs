using UnityEngine;

public class Centrosome : CellOrganelle
{
    public Transform[] centrioles;

    override protected void Start()
    {
        base.Start();
       // OrganizeMicrotubules();
    }

    void OrganizeMicrotubules()
    {
        foreach (Transform centriole in centrioles)
        {
            centriole.Rotate(Vector3.forward * 10f);
        }
    }

    public void AssistCellDivision()
    {
        //CellManager.Instance.InitiateCellDivision();
    }
}
