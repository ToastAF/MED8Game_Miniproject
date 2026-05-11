using UnityEngine;

public class Death : MonoBehaviour
{
    public bool killsPlayer1 = true;
    public bool killsPlayer2 = true;

    public GameObject player1;
    public GameObject player2;
    public GameObject respawn1;
    public GameObject respawn2;

    public LevelReset levelReset;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((killsPlayer1 && collision.CompareTag("Player 1")) || (killsPlayer2 && collision.CompareTag("Player 2")))
        {
            KillsPlayers();
            levelReset.ResetEverything();
        }

    }

    void KillsPlayers()
    {
        player1.transform.position = respawn1.transform.position;
        player2.transform.position = respawn2.transform.position;
    }
}