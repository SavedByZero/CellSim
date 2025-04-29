using UnityEngine;

public class Mitochondrion : CellOrganelle
{
    public int energyGenerationRate = 5;
    public delegate void ChangeEnergyGeneration(int newAmount);
    public ChangeEnergyGeneration onChangeEnergyGeneration;
    protected override void Start()
    {
        base.Start();
        onChangeEnergyGeneration?.Invoke(energyGenerationRate);
    }
    void Update()
    {
       // GenerateEnergy();
    }

    protected override void onDeath()
    {
        onChangeEnergyGeneration?.Invoke(2);
    }

    void GenerateEnergy()
    {
        if (currentHealth > 0)
        {
            // Increase cell energy (Assuming a CellManager class handles energy)
            //CellController.Instance.AddEnergy(energyGenerationRate * Time.deltaTime);
        }
    }
}
