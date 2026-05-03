using UnityEngine;

public class PhsyicButton : MonoBehaviour
{
    public bool isPressed = false;
    public bool HoldDown = false;

    //Puzzle ting
    public bool isPuzzle = false;
    public int triggerType = 0;
    public CominationLock lockScript;

    public GameObject Door;

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
            Door.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject != this.gameObject)
        {
            if (HoldDown)
            {
                spriteRenderer.sprite = PressedUpSprite;
                isPressed = false;
                Door.SetActive(true);

                if (isPuzzle) //logik til puzzle
                {
                    switch (triggerType)
                    {
                        case 0:
                            lockScript.ChangeDigit1();
                            break;
                        case 1:
                            lockScript.ChangeDigit2();
                            break;
                        case 2:
                            lockScript.ChangeDigit3();
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
