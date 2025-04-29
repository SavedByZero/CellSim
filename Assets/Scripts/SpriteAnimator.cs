using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    public Sprite[] Sprites;
    public float frameRate = 0.1f; // Time per frame

    private int currentFrame = 0;
    private float timer = 0f;
    private bool _active = true;
    public bool Loop = true;

    void Start()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (Sprites.Length == 0)
        {
            Debug.LogWarning("No sprites assigned to the SpriteAnimator on " + gameObject.name);
            //enabled = false;
        }
    }

    public void ResetSprite()
    {
        currentFrame = 0;
        _active = true;
    }

    void Update()
    {
        if (!_active || Sprites.Length == 0 || spriteRenderer == null) 
            return;

        timer += Time.deltaTime;
        if (timer >= frameRate)
        {
            timer = 0f;
            currentFrame = (currentFrame + 1) % Sprites.Length;
            if (currentFrame == Sprites.Length - 1)
            {
                if (!Loop)
                    _active = false;
            }
            spriteRenderer.sprite = Sprites[currentFrame];
        }
    }
}
