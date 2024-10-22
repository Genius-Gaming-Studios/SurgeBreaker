using UnityEngine;

[Tooltip("This should always be attatched to the Build Menu")]
public class BuildMenu : MonoBehaviour
{
    public static BuildMenu Instance {get; private set;}

    public string nodeID;

    public AudioClip purchase_FX, notEnoughMoneyPress_FX;

    private void Awake()
    {
        // Check if there is already an Instance of this in the scene
        if (Instance != null)
        {   
            // Destroy this extra copy if this is
            Destroy(gameObject);
            Debug.LogError("Cannot Have More Than One Instance of [BuildMenu] In The Scene!");
            return;
        } 

        Instance = this;
    }

    private void Start()
    {
        if (purchase_FX == null || notEnoughMoneyPress_FX == null) Debug.Log("<color=cyan>Notice! You have not assigned the audio clips for the build menu in this scene!</color> The build menu will not function properly. If you are unaware of how to add an audio clip to the Build Menu's script, please contact Mark in the Hidden Genius Slack.");
    }
    public void BuildButtonPressed(Turret turret)
    {
        foreach (BuildNode Node in FindObjectsOfType<BuildNode>())
        {
            Node.HidePreview(); // Disable the build preview.

            if (Node.uniqueNodeID == nodeID) if (PlayerManager.currentCurrency >= turret.cost) BuildTurret(turret, Node); // Check to see if the node is the current selected node
                else { Debug.LogErrorFormat("Not enough money."); GameManager.GetCorePlayer().PlayOneShot(notEnoughMoneyPress_FX); }
        }
    }

    private void BuildTurret(Turret turret, BuildNode Node)
    {
        PlayerManager.currentCurrency -= turret.cost;
        Node.MyPrefab = (GameObject)Instantiate(turret.prefab, Node.transform);


        Debug.Log("Assigned " + Node.MyPrefab);
        Node.MyPrefab.GetComponent<TurretManager>().turretSettings = turret; // Assigns turret settings
        Node.MyTurretData = turret; // Assign turret settings to node

        GameManager.GetCorePlayer().PlayOneShot(purchase_FX);

        // Handle Voice Line Randomization
        int displayLine = 0;
        displayLine = Random.Range(0, 2); //50% chance
        if (displayLine == 0)
        {
            // Trigger a voice line. This is a player spoken voice line so 'true' is enabled.
            FindObjectOfType<VoicesManager>().TriggerVoiceLine(TriggerCode.TurretBoughtCode, true);
        }


        this.gameObject.SetActive(false); // Disable the build menu


    }


    /// <summary>
    /// Called when a turret build icon is hovered over (in the build menu)
    /// </summary>
    public void TurretPreviewStart(Turret turret)
    {
        foreach (BuildNode Node in FindObjectsOfType<BuildNode>())
        {
            if (Node.uniqueNodeID == nodeID) // Filtering through to get the selected node
            {
                Node.ShowPreview(turret.range);
            }
        }
    }

    /// <summary>
    /// Called when the turret build icon is unhovered (in the build menu)
    /// </summary>
    public void TurretPreviewEnd()
    {
        foreach (BuildNode Node in FindObjectsOfType<BuildNode>())
        {
            if (Node.uniqueNodeID == nodeID) // Filtering through to get the selected node
            {
                Node.HidePreview();
            }
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse1)) gameObject.SetActive(false);
    }
}
