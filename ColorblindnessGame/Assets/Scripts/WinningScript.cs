using UnityEngine;

public class WinningScript : MonoBehaviour
{
    public GameObject winCanvas;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Check for collision with object with tag "Goal" and show the win canvas
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Goal"))
        {
            winCanvas.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
