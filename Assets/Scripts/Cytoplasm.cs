using UnityEngine;

public class Cytoplasm : CellOrganelle
{
    public float viscosity = 0.5f; // Affects movement speed
    private float _counter;
    public float ProductionRate = 1;
    private int productionAmt = 2;
    public int ProductionAmt
    {
        get { return productionAmt; }
        set { productionAmt = value; }
    }
    public delegate void UpdateGlucose(int amt);
    public delegate void UpdateATP(int amt);
    public UpdateGlucose onUpdateGlucose;
    public UpdateATP onUpdateATP;
    private void Start()
    {
        
    }
    public int MakeATPFromGlucose(int glucoseAmt)
    {

        return glucoseAmt * 2;
    }

    private void Update()
    {
        _counter += Time.deltaTime;
        if (_counter >= ProductionRate)
        {

            produceATP();
            _counter = 0;
        }
    }

    private void produceATP()
    {
        //check glucose
        if (GameData.Glucose >= 1)
        {
            GameData.ATP += productionAmt;
            GameData.Glucose -= 1;
            onUpdateATP?.Invoke(GameData.ATP);
            onUpdateGlucose?.Invoke(GameData.Glucose);
            //TODO: Update visual SFX
        }
        //if > 0, make productionAmt ATP
    }
}
