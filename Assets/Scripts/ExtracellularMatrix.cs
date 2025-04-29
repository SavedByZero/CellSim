using UnityEngine;

public class ExtracellularMatrix : CellOrganelle
{
    public void ReceiveSignal(string signalType)
    {
        Debug.Log("Cell received signal: " + signalType);
        //CellManager.Instance.ReactToEnvironment(signalType);
    }
}
