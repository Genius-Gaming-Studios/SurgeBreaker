using UnityEngine;

[Tooltip("This should always be attatched to the Build Menu")]
public class BuildMenu : MonoBehaviour
{
    public string nodeID;

    public AudioClip purchase_FX, notEnoughMoneyPress_FX;
    private void Start()
    {
        if (purchase_FX == null || notEnoughMoneyPress_FX == null) Debug.Log("<color=cyan>Notice! You have not assigned the audio clips for the build menu in this scene!</color> The build menu will not function properly. If you are unaware of how to add an audio clip to the Build Menu's script, please contact Mark in the Hidden Genius Slack.");
    }
    public void BuildButtonPressed(Turret turret)
    {
        foreach (BuildNode Node in FindObjectsOfType<BuildNode>())
        {

            if (Node.uniqueNodeID == nodeID) if (PlayerManager.currentCurrency >= turret.cost) BuildTurret(turret, Node); // Check to see if the node is the current selected node
                else { Debug.LogErrorFormat("Not enough money."); GameManager.GetCorePlayer().PlayOneShot(notEnoughMoneyPress_FX); }
        }
    }

    private void BuildTurret(Turret turret, BuildNode Node)
    {
        Instantiate(turret.prefab, Node.transform);

        PlayerManager.currentCurrency -= turret.cost;
        GameManager.GetCorePlayer().PlayOneShot(purchase_FX);


        this.gameObject.SetActive(false); // Disable the build menu
    }
}
