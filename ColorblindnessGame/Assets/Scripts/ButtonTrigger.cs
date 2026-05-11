using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ButtonTrigger : MonoBehaviour, Resettable
{
    public GameObject interactable;
    public List<GameObject> actionObjects;

    public BoxCollider2D solidCollider;

    public Sprite PressedDownSprite;
    public Sprite PressedUpSprite;
    SpriteRenderer spriteRenderer;

    public bool isPressed = false;
    public bool HoldDown = false;

    public bool returnable;

    string actionObjectTag;
    string playerTag;
    string buttonTag;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buttonTag = interactable.tag;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(interactable.tag))
        {
            foreach(GameObject obj in actionObjects)
            {
                var platform = obj.GetComponent<MovingPlatform>();
                if (platform !=null)
                {
                    platform.MovePlatform();
                    continue;
                }

                var clue = obj.GetComponent<ClueSpace>();
                if (clue != null)
                {
                    clue.RevealClue();
                }

                var spikes = obj.GetComponent<SpikeButton>();
                if (spikes != null)
                {
                    spikes.DisableSpikes();
                }
            }
            spriteRenderer.sprite = PressedDownSprite;
            isPressed = true;
        }
        else
        {
            Debug.Log("wrong player");

            solidCollider.enabled = false;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (!collider.CompareTag(interactable.tag))return;

        if (returnable == true)
        {
            foreach(GameObject obj in actionObjects)
            {
                var platform = obj.GetComponent<MovingPlatform>();
                if (platform !=null)
                {
                    platform.ResetPlatform();
                    continue;
                }

                var clue = obj.GetComponent<ClueSpace>();
                if (clue != null)
                {
                    clue.HideClue();
                }

                var door = obj.GetComponent<Door>();
                if (door != null)         {
                    door.MovePlatform();
                }

                var spikes = obj.GetComponent<SpikeButton>();
                if (spikes != null)
                {
                    spikes.EnableSpikes();
                }
            }
        }
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

    public void Death()
    {
        spriteRenderer.sprite = PressedUpSprite;
        isPressed = false;
    }
}
