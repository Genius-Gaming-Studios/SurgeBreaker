using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildNode : MonoBehaviour
{
    [Tooltip("This will appear when the player hovers over the build node.")] [SerializeField] GameObject HoverObject;
    [Tooltip("This will appear when the player right clicks on an occupied build node.")] [SerializeField] GameObject DeleteOutline;

    [Tooltip("This will show a (prototype) list of turrets that you can build when you press mouse 0 on the build node.")] [SerializeField] GameObject BuildMenu;
    [Tooltip("The sell menu that confirms whether or not the player really wants to delete this selected turret.")]  [SerializeField] GameObject SellMenu;

    [HideInInspector] [Tooltip("This helps the script identify what node it should build a turret on.")] public string uniqueNodeID;
    public bool isNodeOccupied;

    public GameObject MyPrefab; // Prefab of this build node. 

    public void Awake()
    {
        if (BuildMenu == null) BuildMenu = FindObjectOfType<GameManager>().BuildMenu;
        if (SellMenu == null) SellMenu = FindObjectOfType<GameManager>().DeleteMenu;

        uniqueNodeID = $"{Random.Range(10000,99999)}";
    }

    public void OnMouseOver()
    {
        if (BuildMenu == null) BuildMenu = FindObjectOfType<GameManager>().BuildMenu;
        if (SellMenu == null) SellMenu = FindObjectOfType<GameManager>().DeleteMenu;

        //if (PlayerCasting.DistanceFromTarget < 4) // Is the player close enough to the build node? [DEPRECATED]
        //{

        if (FindObjectOfType<GameManager>().currentMode == GameMode.Build)
        {
            if (!isNodeOccupied)
            {
                HoverObject.SetActive(true); // Show the hover object

                if (Input.GetMouseButtonDown(0))
                {
                    SellMenu.SetActive(false);
                    BuildMenu.SetActive(true);
                    BuildMenu.GetComponent<BuildMenu>().nodeID = uniqueNodeID;
                }
            }
            else // Node is occupied
            {
                DeleteOutline.SetActive(true);

                if (Input.GetMouseButtonDown(0))
                {
                    BuildMenu.SetActive(false);
                    SellMenu.SetActive(true);
                    SellMenu.GetComponent<SellMenu>().nodeID = uniqueNodeID;
                }
            }
        }
        //}
        //else HoverObject.SetActive(false); // Player is not close enough to the build node
    }

    public void OnMouseExit()
    {
        if (BuildMenu == null) BuildMenu = FindObjectOfType<GameManager>().BuildMenu;
        if (SellMenu == null) SellMenu = FindObjectOfType<GameManager>().DeleteMenu;

        DeleteOutline.SetActive(false);
        HoverObject.SetActive(false);
    }

    private void Update()
    {
        if (GetComponentInChildren<TurretManager>()) isNodeOccupied = true; else isNodeOccupied = false; // Update node occupation status
    }
    public void Disable() // Turn off the nodes - Note: This is being called in an Update function!
    {
        if (BuildMenu == null) BuildMenu = FindObjectOfType<GameManager>().BuildMenu;
        if (SellMenu == null) SellMenu = FindObjectOfType<GameManager>().DeleteMenu;


        HoverObject.SetActive(false); // Hide the hover object

        BuildMenu.SetActive(false);
        BuildMenu.GetComponent<BuildMenu>().nodeID = ""; // Reset the Node ID

        SellMenu.SetActive(false);
        SellMenu.GetComponent<BuildMenu>().nodeID = ""; // Reset the Node ID


    }


    public void Enable() // Turn on the nodes - Note: This is being called in an Update function!
    {
        if (BuildMenu == null) BuildMenu = FindObjectOfType<GameManager>().BuildMenu;
        if (SellMenu == null) SellMenu = FindObjectOfType<GameManager>().DeleteMenu;
    }
}
