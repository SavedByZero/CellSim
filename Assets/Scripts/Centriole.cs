using UnityEngine;

public class Centriole : CellOrganelle
{
    public float divisionCooldown = 30f; // Time before cell can divide again
    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= divisionCooldown)
        {
            //CellManager.Instance.InitiateCellDivision();
            timer = 0f;
        }
    }
}
