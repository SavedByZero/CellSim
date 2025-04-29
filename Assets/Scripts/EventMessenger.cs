using System;
using UnityEngine;

public class EventMessenger : MonoBehaviour
{
    CellController[] cellControllers; 
    public HUDInterface hudInterface;
    public GameOverHandler GameOver;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cellControllers = GetComponentsInChildren<CellController>();
        for (int i = 0; i < cellControllers.Length; i++) 
        {
            listenToNewCell(cellControllers[i]);
        }
        StoreUI store = GetComponentInChildren<StoreUI>();
        if (store != null)
        {
            store.onUpdateATP += delegate (int atp) { passToHUD(atp, "atp"); };
            store.onUpdateFA += delegate (int fa) { passToHUD(fa, "fa"); };
            store.onUpdateNA += delegate (int na) { passToHUD(na, "na"); };
        }
    }

    void listenToNewCell(CellController cellController)
    {
        cellController.onUpdateATP += delegate (int atp) { passToHUD(atp, "atp"); };
        cellController.onUpdateGlucose += delegate (int g) { passToHUD(g, "g"); };
        cellController.onUpdateGoodie += delegate (Goodie g) { passToHUD(g.amount,g.Type); };
        cellController.onSignalGameOver += receiveGameOverEvent;
    }

    private void receiveGameOverEvent(string reason)
    {
        GameOver.ShowScreen(reason);
    }

    private void passToHUD(int newAmount, string type)
    {
        switch (type) 
        {
            case "atp":
                hudInterface.UpdateATP(newAmount); break;
            case "fa":
                hudInterface.UpdateFA(newAmount); break;
            case "aa":
                hudInterface.UpdateAA(newAmount); break;
            case "g":
                hudInterface.UpdateG(newAmount);break;
            case "na":
                hudInterface.UpdateNA(newAmount);break;
                
        }
    }

    
}
