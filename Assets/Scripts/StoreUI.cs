using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Data;
using System;
public class StoreUI : MonoBehaviour
{
    public static StoreUI Instance;
    public GameObject panel;
    public Button ribosomeButton;
    public Button SlicerButton;
    public Button closeButton;
    private CostPanel costPanel;
    public int[] RibosomeCosts;
    public int[] SlicerCosts;
    public const int FA_INDEX = 0;
    public const int AA_INDEX = 1;
    public const int NA_INDEX = 2;
    public const int ATP_INDEX = 3;

    public delegate void UpdateATP(int newAmount);
    public UpdateATP onUpdateATP;

    public delegate void UpdateGlucose(int newAmount);
    public UpdateGlucose onUpdateGlucose;

    public delegate void UpdateNA(int newAmount);
    public UpdateNA onUpdateNA;

    public delegate void UpdateFA(int newAmount);
    public UpdateNA onUpdateFA;
    
    public delegate void UpdateAA(int newAmount);
    public UpdateNA onUpdateAA;

    private void Awake()
    {
        if (Instance == null) Instance = this;
            else Destroy(gameObject);
    }

    void Start()
    {
       
        closeButton.onClick.AddListener(() => CloseStore());
        ribosomeButton.onClick.AddListener(() => AttemptPurchaseRibosome());
        SlicerButton.onClick.AddListener(() => AttemptPurchaseSlicer());
        costPanel = GetComponentInChildren<CostPanel>();
        EventTrigger trigger = ribosomeButton.gameObject.AddComponent<EventTrigger>();
        AddTrigger(trigger, EventTriggerType.PointerEnter, OnRibosomeHoverEnter);
        AddTrigger(trigger, EventTriggerType.PointerExit, OnRibosomeHoverExit);

        EventTrigger slicer_trigger = SlicerButton.gameObject.AddComponent<EventTrigger>();
        AddTrigger(slicer_trigger, EventTriggerType.PointerEnter, OnSlicerHoverEnter);
        AddTrigger(slicer_trigger, EventTriggerType.PointerExit, OnSlicerHoverExit);
        panel.SetActive(false);

    }

    private void OnSlicerHoverExit(BaseEventData arg0)
    {
        
    }

    private void OnSlicerHoverEnter(BaseEventData arg0)
    {
        
    }

    private bool AttemptPurchaseSlicer()
    {
        if (GameObject.FindObjectsByType<Ribosome>(FindObjectsSortMode.None).Length < 5)
            return false;
        
        if (GameData.ATP >= RibosomeCosts[ATP_INDEX] && GameData.NA >= RibosomeCosts[NA_INDEX] && GameData.AA >= RibosomeCosts[AA_INDEX])
        {
            GameData.ATP -= RibosomeCosts[ATP_INDEX];// 
            GameData.NA -= RibosomeCosts[NA_INDEX];// 
            GameData.AA -= RibosomeCosts[AA_INDEX];// 
            FindFirstObjectByType<Nucleus>()?.SignalSlicerProduction(5);
           
            onUpdateAA?.Invoke(GameData.AA);
            onUpdateNA?.Invoke(GameData.NA);
            onUpdateATP?.Invoke(GameData.ATP);          
            CloseStore();
            return true;
        }
        else
        {
            Debug.Log("Not enough resources to purchase Slicer");
        }
        return false;
    }

    private void AddTrigger(EventTrigger trigger, EventTriggerType type, UnityEngine.Events.UnityAction<BaseEventData> action)
    {
        var entry = new EventTrigger.Entry { eventID = type };
        entry.callback.AddListener(action);
        trigger.triggers.Add(entry);
    }
    private void OnRibosomeHoverEnter(BaseEventData data)
    {
        costPanel.ShowCosts(RibosomeCosts[FA_INDEX], RibosomeCosts[AA_INDEX], RibosomeCosts[NA_INDEX], RibosomeCosts[ATP_INDEX]);

       // tooltipBox.SetActive(true);
       // tooltipText.text = "Produces proteins over time. Cost: 10 ATP, 5 Nucleic Acids, 50 Amino Acids";
    }

    private void OnRibosomeHoverExit(BaseEventData data)
    {
        costPanel.HideCosts();
       // tooltipBox.SetActive(false);
    }

    public void OpenStore()
    {
        panel.SetActive(true);
    }

    public void CloseStore()
    {
        panel.SetActive(false);
    }

    private void AttemptPurchaseRibosome()
    {
        if (GameData.ATP >= RibosomeCosts[ATP_INDEX] && GameData.NA >= RibosomeCosts[NA_INDEX] && GameData.AA >= RibosomeCosts[AA_INDEX])
        {
            GameData.ATP -= RibosomeCosts[ATP_INDEX];// 10;
            GameData.NA -= RibosomeCosts[NA_INDEX];// 5;
            GameData.AA -= RibosomeCosts[AA_INDEX];// 50;
            FindFirstObjectByType<Nucleus>()?.SignalRibosomeProduction(5);
            Debug.Log("Purchased 1 Ribosome");
            onUpdateAA?.Invoke(GameData.AA);
            onUpdateNA?.Invoke(GameData.NA);
            onUpdateATP?.Invoke(GameData.ATP);
            CloseStore();
        }
        else
        {
            Debug.Log("Not enough resources to purchase Ribosome");
        }
    }
}