using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu_Controller : MonoBehaviour
{
    public GameObject MainScreen;
    public GameObject LevelScreen;
    public GameObject OptionsScreen;

    private void Awake()
    {
        AudioListener.pause = false;
    }

    public void OptionsButton()
    {
        MainScreen.SetActive(false);
        OptionsScreen.SetActive(true);
    }

    public void ReturnButton()
    {
        MainScreen.SetActive(true);
        OptionsScreen.SetActive(false);
        LevelScreen.SetActive(false);
    }

    public void LevelsButton()
    {
        MainScreen.SetActive(false);
        LevelScreen.SetActive(true);
    }

   public void LoadLevel(string levelName) 
   {
        SceneManager.LoadScene(levelName) ;
   }

   public void ExitGame()
   {
        Application.Quit();
   }
}
