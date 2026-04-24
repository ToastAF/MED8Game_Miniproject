using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SceneManagement : MonoBehaviour
{
    public GameObject menuCanvas;

    public List<GameObject> tutorialCanvases = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartGame()
    {
        SceneManager.LoadScene("Ronja Scene 2");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private int currentIndex = 0;

    public void ShowTutorial()
    {
        menuCanvas.SetActive(false);

        currentIndex = 0;

        for (int i = 0; i < tutorialCanvases.Count; i++)
        {
            tutorialCanvases[i].SetActive(i == currentIndex);
        }
    }

    public void NextTutorial()
    {
        tutorialCanvases[currentIndex].SetActive(false);

        currentIndex++;

        if (currentIndex < tutorialCanvases.Count)
        {
            tutorialCanvases[currentIndex].SetActive(true);
        }
        else
        {
            menuCanvas.SetActive(true);
            currentIndex = 0;
        }
    }

    public void PreviousTutorial()
    {
        if (currentIndex == 0) return;

        tutorialCanvases[currentIndex].SetActive(false);

        currentIndex--;

        tutorialCanvases[currentIndex].SetActive(true);
    }

    public void BackToMenu()
    {
        tutorialCanvases[currentIndex].SetActive(false);
        menuCanvas.SetActive(true);
    }

}
