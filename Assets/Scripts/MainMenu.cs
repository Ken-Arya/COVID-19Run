using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Text HighscoreText;
    // Start is called before the first frame update
    void Start()
    {
       HighscoreText.text = ((int)PlayerPrefs.GetFloat("Highscore")).ToString(); 
    }

    public void ToGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void HowToPlayMenu()
    {
        SceneManager.LoadScene("HowToPlay");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("EXIT");
    }
}
