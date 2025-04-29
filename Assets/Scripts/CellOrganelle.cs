using System.Collections;
using UnityEngine;

[RequireComponent (typeof(SpriteAnimator))]
public class CellOrganelle : MonoBehaviour
{
    public float maxHealth = 100f;
    [HideInInspector]
    public float currentHealth;
    public Sprite[] Healthy; // 0 = Healthy, 1 = Damaged, 2 = Critical
    public Sprite[] Damaged; // 0 = Healthy, 1 = Damaged, 2 = Critical
    public Sprite[] Critical; // 0 = Healthy, 1 = Damaged, 2 = Critical
    public Sprite[] Recycling;
    protected SpriteAnimator _currentAnimator;
    protected SpriteRenderer spriteRenderer;
    private Rigidbody2D _rb2D;
    public Rigidbody2D Rb2D
    {
        get { return _rb2D; }
    }
    protected virtual void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        _currentAnimator = GetComponentInChildren<SpriteAnimator>();
        UpdateSprite();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        if (currentHealth <= 0)
        {
            StartCoroutine(die());    
        }
        UpdateSprite();
    }

    IEnumerator die()
    {
        yield return new WaitForSeconds(1f);
        onDeath();
        this.gameObject.SetActive(false);
    }

    protected virtual void onDeath()
    {

    }

    protected void UpdateSprite()
    {
        if (_currentAnimator == null)
            return;
        if (currentHealth > maxHealth * 0.66f)
            _currentAnimator.Sprites = Healthy;
        else if (currentHealth > maxHealth * 0.33f)
            _currentAnimator.Sprites = Damaged;
        else if (currentHealth > 0)
            _currentAnimator.Sprites = Critical;
        else
        {
            _currentAnimator.Sprites = Recycling;
            _currentAnimator.Loop = false;
        }
    }
}
