using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public enum ObjectType
{
    NotSpecified,
    Enemy,
    Turret,
    Player,
    Generator
}


[Tooltip("This class is to be put onto any object that has health.")]
public class Health : MonoBehaviour 
{
    [Tooltip("Very important reference that must be assigned. ObjectType: NOTSPECIFIED, ENEMY, TURRET, PLAYER, GENERATOR")]public ObjectType HealthType;

    [Tooltip("This will turn RED when this instance takes damage!")] [SerializeField] SkinnedMeshRenderer[] modelMaterial;
    [Tooltip("[EXPERIMENTAL]")] [SerializeField] MeshRenderer pModelMaterial; 


    [Space(10)]
    [Tooltip("The start player health. (Not used to change/read current health!)")] [SerializeField] public int startHealth = 100;
    [Tooltip("The current health of the player. (Read only! Overwritten on initialization!)")] public int currentHealth = 1;
    
    [Tooltip("This is a test of adding resistance to the player. This can be used later to make 'Armor-like' buffs. (health - (damage / resistance))")] public int resistance;

    // Generators
    [Tooltip("The on-screen health bar of the GENERATOR that corresponds to this health component. (Only for generators!)")] public Slider GeneratorHealthBar;
    [Tooltip("The on-screen health TEXT (e.g. 20%, 500%, 300%) of the GENERATOR that corresponds to this health component. (Only for generators!) ")] public TextMeshProUGUI GeneratorHealthText;


    [HideInInspector] public int _bounty; // This should be assigned via the Enemy script.


    private void Start()
    {
        if (HealthType == ObjectType.Turret) Debug.LogWarningFormat("Health Type is Turret, however, Turret health has no true functionality!");


        if (modelMaterial != null) foreach (SkinnedMeshRenderer renderer in modelMaterial) foreach (Material mat in renderer.materials) standardColor = mat.color; 
        else foreach(Material mat in pModelMaterial.materials) standardColor = mat.color;

        // Generators
        if (HealthType == ObjectType.Generator)
        {
            GeneratorHealthBar.maxValue = startHealth;
            GeneratorHealthBar.minValue = 0;
        }
       
        
        currentHealth = startHealth; // Initialize current health for things that are not Generators
    }

    private void Update()
    {
        if (HealthType == ObjectType.NotSpecified) { Debug.LogErrorFormat("Please specify the Health Type of this health object."); return; }

        // Generators
        if (HealthType == ObjectType.Generator)
        {
            GeneratorHealthBar.value = currentHealth;
            if (currentHealth > 1) GeneratorHealthText.text = $"{currentHealth}%";
            else GeneratorHealthText.text = $"<color=red>OFFLINE</color>";
        }
        if (currentHealth <= 0) // This will kill the health object. If it's an enemy, it should be attatched to the parent object.
        {
            if (HealthType == ObjectType.Player) GetComponent<PlayerManager>().Die();
            else Die();

            this.enabled = false;
        }


    }

    public void Damage(int amount) // This can be called to damage this instance. (TAKE DAMAGE)
    {
        StopCoroutine(DamageRenderer());
        StartCoroutine(DamageRenderer());

        if (resistance == 0) currentHealth -= amount;
        else
        {
            currentHealth = Mathf.RoundToInt(currentHealth -= amount / resistance); // This just divides the damage by the resistance. It's a simple feature, but it works.
        }
    }

    private Color standardColor;

    public IEnumerator DamageRenderer() // This simply makes the renderer appear red for a tenth of a second when it gets damaged. Sounds can be added later.
    {

        if (modelMaterial != null) foreach (SkinnedMeshRenderer renderer in modelMaterial) foreach (Material mat in renderer.materials) mat.color = Color.red;
        else if (pModelMaterial != null) foreach (Material mat in pModelMaterial.materials) mat.color = Color.red;

        yield return new WaitForSeconds(0.1f);


        if (modelMaterial != null) foreach (SkinnedMeshRenderer renderer in modelMaterial) foreach (Material mat in renderer.materials) mat.color = standardColor;
        else if (pModelMaterial != null) foreach (Material mat in pModelMaterial.materials) mat.color = standardColor;

    }



    private void Die() // This is called for enemies. Turret functionality is not going to be created.
    {
        if (HealthType != ObjectType.Enemy) return;

        PlayerManager.currentCurrency += _bounty; // Add player money according to the enemy's bounty.

        GameManager.enemiesAlive--; // Subtract from the index of enemies alive before proceeding.

        Destroy(this.gameObject);

    }
}
