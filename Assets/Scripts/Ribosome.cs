using UnityEngine;
using DG.Tweening;

public class Ribosome : CellOrganelle
{
    [SerializeField] private float synthesisCooldown = 5f;
    [SerializeField] private GameObject proteinPrefab;
    private float timer;

    protected override void Start()
    {
        base.Start();
        timer = synthesisCooldown * Random.value;
        this.transform.DOShakeScale(1);
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            SynthesizeProtein();
            timer = synthesisCooldown;
        }
    }

    void SynthesizeProtein()
    {
        if (proteinPrefab != null)
        {
            Instantiate(proteinPrefab, transform.position, Quaternion.identity, transform.parent);
            Debug.Log("Protein synthesized by ribosome.");
        }
    }
}
