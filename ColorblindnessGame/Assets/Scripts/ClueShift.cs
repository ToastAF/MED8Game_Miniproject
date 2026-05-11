using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ClueShift : MonoBehaviour, Resettable
{
    public Sprite[] sprites;
    public SpriteRenderer clueSpace;
    public int currentSpriteIndex = 0;

    public Sprite PressedDownSprite;
    public Sprite PressedUpSprite;
    public SpriteRenderer spriteRenderer;

    public bool isPressed = false;
    public bool HoldDown = false;
    int playersOnButton = 0;

    void Start()
    {
        //spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateSprite();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playersOnButton++;

        
        if (collision.CompareTag("Player 1") || collision.CompareTag("Player 2"))
        {
            if (playersOnButton == 1)
            {
                ShiftSprite();
                spriteRenderer.sprite = PressedDownSprite;
            }   
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {    
        if (collision.CompareTag("Player 1") || collision.CompareTag("Player 2"))
        {
            playersOnButton--;
            if (playersOnButton <= 0)
            {
                playersOnButton = 0;
                spriteRenderer.sprite = PressedUpSprite;
            }   
        }
    }

    void ShiftSprite()
    {
        currentSpriteIndex++;

        if (currentSpriteIndex >= sprites.Length)
        {
            currentSpriteIndex = 0;
        }

        UpdateSprite();

        CodeManager.instance.CheckCode();
    }

    void UpdateSprite()
    {
        clueSpace.sprite = sprites[currentSpriteIndex];
    }

    public void Death()
    {
        currentSpriteIndex = 0;
        playersOnButton = 0;
        UpdateSprite();
        spriteRenderer.sprite = PressedUpSprite;
    }
}
