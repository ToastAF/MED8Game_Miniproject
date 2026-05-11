using UnityEngine;

public class PhsyicButton : MonoBehaviour
{
    public bool isPressed = false;
    public bool HoldDown = false;

    //Puzzle ting
    //public bool isPuzzle = false;
    //public int triggerType = 0;
    //public CominationLock lockScript;

    //public GameObject Door;

    public Sprite PressedDownSprite;
    public Sprite PressedUpSprite;
    SpriteRenderer spriteRenderer;

    public void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != this.gameObject)
        {
            spriteRenderer.sprite = PressedDownSprite;
            isPressed = true;
            //Door.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject != this.gameObject)
        {
            if (HoldDown)
            {
                spriteRenderer.sprite = PressedDownSprite;
                isPressed = false;
            }
            else
            {
                spriteRenderer.sprite = PressedUpSprite;
                isPressed = false;
            }
        }
    }
}
