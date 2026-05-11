using UnityEngine;
using UnityEngine.Tilemaps;

public class LayerShift : MonoBehaviour, Resettable
{
    public TilemapRenderer leftSpikes;
    public SpriteRenderer leftButton;
    public TilemapRenderer redSpikes;
    public TilemapRenderer blueSpikes;

    bool playerOneWasHere = false;
    bool playerTwoWasHere = false;

    void Start()
    {
        leftSpikes = leftSpikes.GetComponent<TilemapRenderer>();
        leftButton = leftButton.GetComponent<SpriteRenderer>();
        redSpikes = redSpikes.GetComponent<TilemapRenderer>();
        blueSpikes = blueSpikes.GetComponent<TilemapRenderer>();
    }

    void Update()
    {
        if (playerOneWasHere && playerTwoWasHere)
        {
            Shift();
        }
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player 1"))
        {
            playerOneWasHere = true;
        }
        
        if (collision.CompareTag("Player 2"))
        {
            playerTwoWasHere = true;
        }
    }

    void Shift()
    {
        leftSpikes.sortingOrder = 1;
        leftButton.sortingOrder = 2;
        redSpikes.sortingOrder = 1;
        blueSpikes.sortingOrder = 2;
    }

    public void Death()
    {
        playerOneWasHere = false;
        playerTwoWasHere = false;
        leftSpikes.sortingOrder = 2;
        leftButton.sortingOrder = 1;
        redSpikes.sortingOrder = 2;
        blueSpikes.sortingOrder = 1;
    }
}
