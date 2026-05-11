using UnityEngine;
using UnityEngine.Rendering;

public class CodeManager : MonoBehaviour
{
    public static CodeManager instance;
    public ClueShift[] buttons;
    public int[] correctCode;
    public GameObject obstacle;

    private void Awake()
    {
        instance = this;
    }

    public void CheckCode()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].currentSpriteIndex != correctCode[i])
            {
                return;
            }
        }

        CorrectCodeEntered();
    }

    void CorrectCodeEntered()
    {
        obstacle.SetActive(false);
    }
}
