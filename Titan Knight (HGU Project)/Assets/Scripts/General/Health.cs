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
    [Tooltip("Maximum amount of health that the player or generator can have. Recommended amount: startHealth")] public int maximumHealth = 100;
    [SerializeField] bool autoAssignMaxHealth = true;

    [Tooltip("This is a test of adding resistance to the player. This can be used later to make 'Armor-like' buffs. (health - (damage / resistance))")] public int resistance;

    // Generators
    [Tooltip("The on-screen health bar of the GENERATOR that corresponds to this health component. (Only for generators!)")] public Slider GeneratorHealthBar;
    [Tooltip("The on-screen health TEXT (e.g. 20%, 500%, 300%) of the GENERATOR that corresponds to this health component. (Only for generators!) ")] public TextMeshProUGUI GeneratorHealthText;


    [HideInInspector] public int _bounty; // This should be assigned via the Enemy script.
    [HideInInspector] public bool beingSlowed; // This should be assigned via the Enemy script.


    [Tooltip("The color material that the enemy will glow as when they're being slowed")] [SerializeField] Material SlowMaterial;
    private Material StandardMaterial;

    private void Start()
    {
        if (HealthType == ObjectType.Turret) Debug.LogWarningFormat("Health Type is Turret, however, Turret health has no true functionality!");
        if (HealthType == ObjectType.Enemy && SlowMaterial == null) Debug.Log("Enemy that was spawned has no slow material. This means they are not compatible with the 1.9.2a update, and will encounter errors when slowed.");
         

        if (autoAssignMaxHealth) maximumHealth = startHealth;

        if (modelMaterial != null) foreach (SkinnedMeshRenderer renderer in modelMaterial) foreach (Material mat in renderer.materials) standardColor = mat.color; 
        else foreach(Material mat in pModelMaterial.materials) standardColor = mat.color;


        if (modelMaterial != null) foreach (SkinnedMeshRenderer renderer in modelMaterial) foreach (Material mat in renderer.materials) StandardMaterial = mat;
        else foreach (Material mat in pModelMaterial.materials) StandardMaterial = mat;

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
            if (HealthType == ObjectType.Player) GetComponent<PlayerManager>().Die(); // The player manager needs to handle it herself.
            else Die(); // We can handle it ourselves.

            this.enabled = false;
        }

        if (HealthType == ObjectType.Enemy)
        {
            if (beingSlowed)
            {
                canTick = true;
                if (modelMaterial != null) foreach (SkinnedMeshRenderer mat in modelMaterial) mat.material = SlowMaterial;
                else pModelMaterial.material = SlowMaterial;
            }
            else
            {
                if (canTick)
                {
                    canTick = false;
                    if (modelMaterial != null) foreach (SkinnedMeshRenderer mat in modelMaterial) mat.material = StandardMaterial;
                    else pModelMaterial.material = SlowMaterial;
                }
            }
        }
    }
    public Coroutine DamageCoroutine;

    [HideInInspector] bool canTick = true;


    public void Damage(int amount) // This can be called to damage this instance. (TAKE DAMAGE)
    {
        if (DamageCoroutine != null) StopCoroutine(DamageCoroutine);
        if (amount > 0) // Damage
        {
            DamageCoroutine = StartCoroutine(DamageRenderer(Color.red));

            if (resistance == 0) currentHealth -= amount;
            else
            {
                currentHealth = Mathf.RoundToInt(currentHealth -= amount / resistance); // This just divides the damage by the resistance. It's a simple feature, but it works.
            }

        }
        else if (amount < 0) // Heal
        {
            DamageCoroutine = StartCoroutine(DamageRenderer(Color.green));

            if (amount + currentHealth > maximumHealth) currentHealth = maximumHealth;

            if (currentHealth < maximumHealth) currentHealth -= amount;
        }
    }

    private Color standardColor;

    public IEnumerator DamageRenderer(Color colorUpdate) // This simply makes the renderer appear red for a tenth of a second when it gets damaged. Sounds can be added later.
    {
        if (!beingSlowed)
        {

            if (modelMaterial != null) foreach (SkinnedMeshRenderer renderer in modelMaterial) foreach (Material mat in renderer.materials) mat.color = colorUpdate;
            else if (pModelMaterial != null) foreach (Material mat in pModelMaterial.materials) mat.color = colorUpdate;

            yield return new WaitForSeconds(0.1f);


            if (modelMaterial != null) foreach (SkinnedMeshRenderer renderer in modelMaterial) foreach (Material mat in renderer.materials) mat.color = standardColor;
            else if (pModelMaterial != null) foreach (Material mat in pModelMaterial.materials) mat.color = standardColor;
        }
        else yield return null; // Need this so game doesnt data leak
    }



    private void Die() // This is called for enemies. Turret functionality is not going to be created.
    {
        if (HealthType != ObjectType.Enemy) return;

        PlayerManager.currentCurrency += _bounty; // Add player money according to the enemy's bounty.

        GameManager.enemiesAlive--; // Subtract from the index of enemies alive before proceeding.

        Destroy(this.gameObject);

    }
}
