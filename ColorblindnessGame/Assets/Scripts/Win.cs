using UnityEngine;

public class Win : MonoBehaviour
{
    public GameObject winScreen;
    public GameObject divider;

    bool playerOneInGoal = false;
    bool playerTwoInGoal = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        winScreen.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player 1"))
        {
            playerOneInGoal = true;
        }
        
        if (collision.CompareTag("Player 2"))
        {
            playerTwoInGoal = true;
        }

        if (playerOneInGoal && playerTwoInGoal)
        {
            divider.SetActive(false);
            winScreen.SetActive(true);
        }
    }
}
