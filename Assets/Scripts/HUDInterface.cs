using TMPro;
using UnityEngine;

public class HUDInterface : MonoBehaviour
{
    public TextMeshProUGUI ATPField;
    public TextMeshProUGUI AAField;
    public TextMeshProUGUI FAField;
    public TextMeshProUGUI NAField;
    public TextMeshProUGUI GField;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void UpdateATP(int amount)
    {
        ATPField.text = amount.ToString();
    }

    public void UpdateG(int amount) 
    {
        GField.text = amount.ToString();
    }

    public void UpdateFA(int amount)
    {
        FAField.text = amount.ToString();
    }

    public void UpdateNA(int amount)
    {
        NAField.text = amount.ToString();
    }

    public void UpdateAA(int amount)
    {
        AAField.text = amount.ToString();
    }
}
