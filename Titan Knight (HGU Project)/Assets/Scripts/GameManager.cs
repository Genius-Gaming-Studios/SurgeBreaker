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
