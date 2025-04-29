using TMPro;
using UnityEngine;

public class Goodie : MonoBehaviour
{
    private TextMeshProUGUI field;
    public int amount = 1;
    [SerializeField]
    private string type = "g";
    public string Type
    {
        get { return type; }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        field = GetComponentInChildren<TextMeshProUGUI>();
        field.text = amount.ToString();
    }

    
}
