using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildNode : MonoBehaviour
{
    [Tooltip("This will appear when the player hovers over the build node.")] [SerializeField] GameObject HoverObject;
    [Tooltip("This will appear when the player right clicks on an occupied build node.")] [SerializeField] GameObject DeleteOutline;

    [Tooltip("The sell menu that confirms whether or not the player really wants to delete this selected turret.")]  [SerializeField] GameObject SellMenu;

    [HideInInspector] [Tooltip("This helps the script identify what node it should build a turret on.")] public string uniqueNodeID;
    public bool isNodeOccupied;

    public GameObject MyPrefab; // Prefab of this build node. 
    public Turret MyTurretData; // Turret data of this node.

    [Tooltip("Parent object of the preview")] public GameObject TurretPreview; 
    [Tooltip("The range of the preview that is showing. The range is set an modified by script.")] public Transform RangePreview, constantRangePreview; 


    /// <summary>
    /// Shows a preview for any general turret.
    /// </summary>
    /// <param name="turretRange">The range of the turet, for the preview.</param>
    public void ShowPreview(float turretRange)
    {
        TurretPreview.SetActive(true);

        // Set the range scale of the preview circle.
        RangePreview.localScale = new(turretRange, RangePreview.localScale.y, turretRange);
    }

    public void HidePreview()
    {
        TurretPreview.SetActive(false); 
    }

    public void Awake()
    {
        if (SellMenu == null) SellMenu = FindObjectOfType<GameManager>().DeleteMenu;

        uniqueNodeID = $"{Random.Range(10000,99999)}";

        HidePreview();
    }

    public void OnMouseOver()
    {
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
                    Debug.Log("Hello");
                    SellMenu.SetActive(false);
                    UIManager.Instance.ShowBuildMenu();
                    UIManager.Instance.GetBuildMenuUI().nodeID = uniqueNodeID;
                }
            }
            else // Node is occupied
            {

                DeleteOutline.SetActive(true);

                if (Input.GetMouseButtonDown(0))
                {
                    UIManager.Instance.HideBuildMenu();
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
        if (SellMenu == null) SellMenu = FindObjectOfType<GameManager>().DeleteMenu;

        DeleteOutline.SetActive(false);
        HoverObject.SetActive(false);
    }

    private void Update()
    {
        if (GetComponentInChildren<TurretManager>()) isNodeOccupied = true; else isNodeOccupied = false; // Update node occupation status

        if (MyPrefab == null) MyTurretData = null;
    }
    public void Disable() // Turn off the nodes - Note: This is being called in an Update function!
    {
        if (SellMenu == null) SellMenu = FindObjectOfType<GameManager>().DeleteMenu;


        HoverObject.SetActive(false); // Hide the hover object

        UIManager.Instance.HideBuildMenu();
        UIManager.Instance.GetBuildMenuUI().nodeID = "";

        SellMenu.SetActive(false);
        SellMenu.GetComponent <SellMenu>().nodeID = ""; // Reset the Node ID

        // Hide the previews
        HidePreview();
        constantRangePreview.gameObject.SetActive(false);


    }


    public void Enable() // Turn on the nodes - Note: This is being called in an Update function!
    {
        if (SellMenu == null) SellMenu = FindObjectOfType<GameManager>().DeleteMenu;

        // Show the range of turret constantly
        if (isNodeOccupied)
        {
            constantRangePreview.gameObject.SetActive(true);
            constantRangePreview.localScale = new(MyTurretData.range, constantRangePreview.localScale.y, MyTurretData.range);
        }
        else
        {
            constantRangePreview.gameObject.SetActive(false);
        }
    }
}
