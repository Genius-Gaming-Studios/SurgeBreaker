using UnityEngine;
using TMPro;

[Tooltip("This should always be attatched to the Sell Menu")]
public class SellMenu : MonoBehaviour
{
    public string nodeID;
    [SerializeField] TextMeshProUGUI sellText, nameText;

    public AudioClip sell_FX;
    private void Start()
    {
        if (sell_FX == null) Debug.Log("<color=cyan>Notice! You have not assigned the audio clips for the build menu in this scene!</color> The build menu will not function properly. If you are unaware of how to add an audio clip to the Build Menu's script, please contact Mark in the Hidden Genius Slack.");
    }

    private void Update()
    {
        foreach (BuildNode Node in FindObjectsOfType<BuildNode>())
        {
            // Check to see if the node is the current selected node
            if (Node.uniqueNodeID == nodeID)
            {
                TurretManager turret = Node.MyPrefab.GetComponent<TurretManager>();

                nameText.text = turret.turretSettings.turretName;
                sellText.text = $"Sell for ${turret.turretSettings.cost - (turret.turretSettings.cost /4)}";
            }
        }
    }
    public void SellButton()
    {
        foreach (BuildNode Node in FindObjectsOfType<BuildNode>())
        {

            if (Node.uniqueNodeID == nodeID)
            {
                TurretManager turret = Node.MyPrefab.GetComponent<TurretManager>();
                SellTurret(turret.turretSettings, Node); // Check to see if the node is the current selected node
            }
        }
    }


    private void SellTurret(Turret turret, BuildNode Node)
    {
        PlayerManager.currentCurrency += (turret.cost - Mathf.RoundToInt(turret.cost/4));

        Destroy(Node.MyPrefab);
        Node.MyPrefab = null;
        
        GameManager.GetCorePlayer().PlayOneShot(sell_FX);

        this.gameObject.SetActive(false); // Disable the sell menu
    }

    public void CancelButton()
    {
        this.gameObject.SetActive(false);
    }
}