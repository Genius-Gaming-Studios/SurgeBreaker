using UnityEngine;

[Tooltip("This should always be attatched to the Build Menu")]
public class BuildMenu : MonoBehaviour
{
    public string nodeID;


    public void BuildButtonPressed(Turret turret)
    {
        foreach (BuildNode Node in FindObjectsOfType<BuildNode>())
        {
            if (Node.uniqueNodeID == nodeID) if (PlayerManager.currentCurrency >= turret.cost) BuildTurret(turret, Node); // Check to see if the node is the current selected node
                else Debug.LogErrorFormat("Not enough money.");
        }
    }

    private void BuildTurret(Turret turret, BuildNode Node)
    {
        Instantiate(turret.prefab, Node.transform);

        this.gameObject.SetActive(false); // Disable the build menu
    }
}
