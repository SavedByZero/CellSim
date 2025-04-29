using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System;
using System.Linq;

public class CostPanel : MonoBehaviour
{
    public GameObject FA;
    public GameObject AA;
    public GameObject NA;
  
    //public GameObject G;
    public GameObject ATP;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HideCosts();
    }

    public void HideCosts()
    {
        FA.SetActive(false);
        AA.SetActive(false);
        NA.SetActive(false);
        ATP.SetActive(false);
    }

    public void ShowCosts(int FA, int AA, int NA, int ATP)
    {

        this.FA.SetActive(FA > 0);
        this.AA.SetActive(AA > 0);
        this.NA.SetActive(NA > 0);
        this.ATP.SetActive(ATP > 0);

        if (FA > 0)
        {
            
            this.FA.GetComponentInChildren<TextMeshProUGUI>().text = FA.ToString();
        }
            
        if (AA > 0)
        {
            this.AA.GetComponentInChildren<TextMeshProUGUI>().text = AA.ToString();
        }
           
        if (NA > 0)
        {
            this.NA.GetComponentInChildren<TextMeshProUGUI>().text = NA.ToString();
        }
            
        if (ATP > 0)
        {
            this.ATP.GetComponentInChildren<TextMeshProUGUI>().text = ATP.ToString();
        }
           

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
