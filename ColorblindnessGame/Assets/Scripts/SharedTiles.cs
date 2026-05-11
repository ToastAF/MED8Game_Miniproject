using UnityEngine;

public class SharedTiles : MonoBehaviour, Resettable
{
    public Sprite[] sprites;
    public int currentSpriteIndex;
    public BoxCollider2D solidCollider;
    public SpriteRenderer tile;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateSprite();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player 2"))
        {
            if (currentSpriteIndex != 2)
            {
                currentSpriteIndex++;

                UpdateSprite();

                solidCollider.enabled = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player 2"))
        {
            if (currentSpriteIndex != 2)
            {
                currentSpriteIndex++;

                UpdateSprite();
            }
        }
    }

    void UpdateSprite()
    {
        tile.sprite = sprites[currentSpriteIndex];
    }

    public void Death()
    {
        currentSpriteIndex = 0;
        UpdateSprite();
        solidCollider.enabled = false;
    }
}
