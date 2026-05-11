using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpikeButton : MonoBehaviour, Resettable
{
    void Start()
    {
        //buttonTag = interactable.tag;
    }

    public void DisableSpikes()
    {
        gameObject.SetActive(false);
    }

    public void EnableSpikes()
    {
        gameObject.SetActive(true);
    }

    public void Death()
    {
        gameObject.SetActive(true);
    }
}