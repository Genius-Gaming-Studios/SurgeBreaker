using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildNode : MonoBehaviour
{
    [Tooltip("This will appear when the player hovers over the build node.")] [SerializeField] GameObject HoverObject;

    public void OnMouseOver()
    {
        //if (PlayerCasting.DistanceFromTarget < 4) // Is the player close enough to the build node?
        //{
            HoverObject.SetActive(true); // Show the hover object

            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("open menu");
            }
        //}
        //else HoverObject.SetActive(false); // Player is not close enough to the build node
    }

    public void OnMouseExit()
    {
        HoverObject.SetActive(false);
    }
}
