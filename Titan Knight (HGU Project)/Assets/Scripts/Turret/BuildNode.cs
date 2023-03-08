using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildNode : MonoBehaviour
{
    [Tooltip("This will appear when the player hovers over the build node.")] [SerializeField] GameObject HoverObject;
    [Tooltip("This will show a (prototype) list of turrets that you can build when you press E on the build node.")] [SerializeField] GameObject BuildMenu;


    [HideInInspector] [Tooltip("This helps the script identify what node it should build a turret on.")] public string uniqueNodeID;


    public void Awake()
    {
        
        uniqueNodeID = $"{Random.Range(10000,99999)}";
        BuildMenu.SetActive(false);
    }

    public void OnMouseOver()
    {
        //if (PlayerCasting.DistanceFromTarget < 4) // Is the player close enough to the build node?
        //{
            HoverObject.SetActive(true); // Show the hover object

            if (Input.GetKeyDown(KeyCode.E))
            {
            if (BuildMenu == null) BuildMenu = FindObjectOfType<GameManager>().BuildMenu;
          
                BuildMenu.SetActive(true);
                BuildMenu.GetComponent<BuildMenu>().nodeID = uniqueNodeID;
            }
        //}
        //else HoverObject.SetActive(false); // Player is not close enough to the build node
    }

    public void OnMouseExit()
    {
        HoverObject.SetActive(false);
    }


    public void Disable() // Turn off the nodes - Note: This is being called in an Update function!
    {
        this.GetComponent<MeshRenderer>().enabled = false;

        HoverObject.SetActive(false); // Hide the hover object

        BuildMenu.SetActive(false);
        BuildMenu.GetComponent<BuildMenu>().nodeID = ""; // Reset the Node ID


        GetComponent<BoxCollider>().enabled = false;
    }


    public void Enable() // Turn on the nodes - Note: This is being called in an Update function!
    {
        GetComponent<BoxCollider>().enabled = true;

        this.GetComponent<MeshRenderer>().enabled = true;

    }
}
