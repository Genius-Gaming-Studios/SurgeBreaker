using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode
{
    Idle,
    Build,
    Combat,
}

public class GameManager : MonoBehaviour
{
    [Tooltip("This is the current GameMode that majorly effects how the game acts. Idle = 0, Build = 1, Combat = 2")]public GameMode currentMode = GameMode.Build;

    [SerializeField] GameObject CombatCanvas, BuildCanvas;
    [Tooltip("This will be turned off when the player is in Build Mode!")][SerializeField] GameObject WeaponsParent;


    /// <summary>
    /// The only audio player that the FX come out of.
    /// </summary>
    public AudioSource CoreFXPlayer;
    
    /// <summary>
    /// The only audio player that the Music should come out of.
    /// </summary>
    public AudioSource CoreMusicPlayer;

    public static AudioSource GetCorePlayer() { return FindObjectOfType<GameManager>().CoreFXPlayer; }

[SerializeField] public GameObject BuildMenu;


    private void Update()
    {



        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchGamemode();
        }

        switch (currentMode)
        {
            case GameMode.Build:
                CombatCanvas.SetActive(false);
                BuildCanvas.SetActive(true);
                WeaponsParent.SetActive(false);

                foreach (BuildNode node in FindObjectsOfType<BuildNode>())
                {
                    node.Enable(); // Show all node mesh renderes
                } 


                break;

            case GameMode.Combat:
                CombatCanvas.SetActive(true);
                BuildCanvas.SetActive(false);
                WeaponsParent.SetActive(true);

                foreach (BuildNode node in FindObjectsOfType<BuildNode>()) node.Disable(); // Hide all node mesh renderers

                break;

            default: 
                CombatCanvas.SetActive(false);
                BuildCanvas.SetActive(false);
                WeaponsParent.SetActive(false);


                foreach (BuildNode node in FindObjectsOfType<BuildNode>()) node.Disable(); // Hide all node mesh renderers

                break;
        }

        // Surge Breaker (1.5p)
        // This will stop all coroutines, and pause the game. This means, animations will need to be animators and mustn't rely on TimeScale if an animation is to be seen within Build Mode. 
        // This is primarily used for pausing the spawning of waves while in build mode.
        // This method may be deprecated in the future due to all of the problems that may come from pausing time itself while in build mode. Try not to take this off of your radar.

        // if (currentMode == GameMode.Build) Time.timeScale = 0;
        // else Time.timeScale = 1; // Normalize Time Scale. This may need more TLC if there is a pause menu that is implemented.
    }


    public void SwitchGamemode(GameMode switchTo) // This manually alternates the gamemode
    {
        currentMode = switchTo;

        Debug.Log($"Switched gamemode to <color=\"yellow\">{currentMode}</color>.");
    }


    public void SwitchGamemode() // This automatically alternates the gamemode when you press tab
    {
        if (currentMode == GameMode.Build)
        {
            currentMode = GameMode.Combat;
        }
        else if (currentMode == GameMode.Combat)
        {
            currentMode = GameMode.Build;
        }
        else if (currentMode.GetHashCode() == 0)
        {
            currentMode++;
        }

        Debug.Log($"Switched gamemode to <color=\"yellow\">{currentMode}</color>.");
    }

}
