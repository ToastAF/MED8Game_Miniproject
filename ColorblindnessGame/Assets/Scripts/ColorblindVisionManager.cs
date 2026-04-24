using UnityEngine;

public class ColorblindVisionManager : MonoBehaviour
{
    public static ColorblindVisionManager Instance { get; private set; }

    [Header("Player References")]
    public Transform player1; // Protanopia character
    public Transform player2; // Tritanopia character

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
}
