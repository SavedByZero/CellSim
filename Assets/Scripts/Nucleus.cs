using UnityEngine;

public class Nucleus : CellOrganelle
{
   // [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] healthSprites; // 3 sprites for different health states
    [SerializeField] private float health = 100f; // Full health
    
    private Nucleolus nucleolus;
    public Ribosome RibosomeTemplate;
    public SlicerEnzyme SlicerTemplate;
    public RNA RNATemplate;
    public Transform[] Pores;
    public Transform[] RNAPores;

    override protected void Start()
    {

       
        health = maxHealth;
        base.Start();
        nucleolus = new Nucleolus();

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        
        //nucleolus.produceRNA(RNATemplate, 5, RNAPores);
    }


    public void Hijack(InjectorVirus injector, int count = 1)
    {
        nucleolus.HijackAndProduceInjector(this, injector, count);
    }
   
   

    /// <summary>
    /// Sends a signal to the nucleolus to produce ribosomes.
    /// </summary>
    public void SignalRibosomeProduction(int amount)
    {
        if (nucleolus != null && currentHealth > 0)
        {
            nucleolus.ProduceRibosomes(amount, RibosomeTemplate,Pores, this);
        }
    }
    /// </summary>
    public void SignalSlicerProduction(int amount)
    {
        if (nucleolus != null && currentHealth > 0)
        {
            nucleolus.ProduceSlicers(amount, SlicerTemplate, RNATemplate, RNAPores, this);//, Pores);
        }
    }

    void OnMouseDown()
    {
        StoreUI.Instance.OpenStore();
    }

   
}
