using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void exitGame()
    {
        Application.Quit();
    }
    public void startGame()
    {
        SceneManager.LoadScene(2);
    }
    public void savedGame()
    {
        SceneManager.LoadScene(1);
    }

    public void backToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void restart()
    {
        int i = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(i);
    }
    /*
    public void nextLevel()
    {
        int index = SceneManager.GetActiveScene().buildIndex + 1;
        if (index == 4) index = 0;
        SceneManager.LoadScene(index);
    }
    */
}
