using UnityEngine;

public class ClueSpace : MonoBehaviour
{
    public GameObject clue;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RevealClue()
    {
        clue.SetActive(true);
    }

    public void HideClue()
    {
        clue.SetActive(false);
    }
}
