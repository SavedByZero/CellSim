using System.Collections;
using UnityEngine;

public class RNA : CellOrganelle
{
    public GameObject templateProtein;
    public float decayTime = 10f;
    public Sprite[] DescentSprites;
    private float timer;

    protected override void Start()
    {
        base.Start();
        timer = decayTime;
    }

    public void Descend(float delay = 2.5f)
    {
        StartCoroutine(descendInternal(delay));
    }

    IEnumerator descendInternal(float delay)
    {
        yield return new WaitForSeconds(delay);
        Healthy = DescentSprites;
        UpdateSprite();
        _currentAnimator.ResetSprite();
        StartCoroutine(expire());
    }

    IEnumerator expire()
    {
        yield return new WaitForSeconds(1);
        TakeDamage(maxHealth);
        //TODO: recycle ATP?
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
           
            Debug.Log("RNA decayed");
            TakeDamage(maxHealth);
        }
    }
}