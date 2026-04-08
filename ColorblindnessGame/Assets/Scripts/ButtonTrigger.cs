using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    public GameObject interactable;
    public List<GameObject> actionObjects;

    string actionObjectTag;
    string playerTag;
    string buttonTag;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buttonTag = interactable.tag;
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
            }
        }
        else
        {
            Debug.Log("wrong player");
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (!collider.CompareTag(interactable.tag))return;

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
        }
    }
}
