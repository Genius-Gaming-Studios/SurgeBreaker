using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu_Controller : MonoBehaviour
{
   public void NewGame() 
   {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
   }

   public void ExitGame()
   {
        Application.Quit();
   }
}
