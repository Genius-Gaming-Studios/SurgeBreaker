/// Coded by Mark Anthony Gaskins II, April 2024
/// Modified for runtime-safe builds using Resources.Load

using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Juice Controller", menuName = "New Enemy Juice Module")]
public class EnemyJuice : ScriptableObject
{
    [HideInInspector]
    [Tooltip("(left) Quieter -> Louder (right); This is the chance that the enemy will make a sound when asked to.")]
    [Range(0, 6)]
    public int loudness = 2;

    [Header("Enemy SFX References")]
    [Tooltip("Hit sound is randomly chosen between hurtVariation1 and hurtVariation2 when the enemy gets hit.")]
    public AudioClip hurtVariation1;
    [Tooltip("Hit sound is randomly chosen between hurtVariation1 and hurtVariation2 when the enemy gets hit.")]
    public AudioClip hurtVariation2;

    [Tooltip("Hit sound is randomly chosen between deathVariation1 and deathVariation2 when the enemy gets killed.")]
    public AudioClip deathVariation1;
    [Tooltip("Hit sound is randomly chosen between deathVariation1 and deathVariation2 when the enemy gets killed.")]
    public AudioClip deathVariation2;

    [Tooltip("These audios will play when the enemy spawns/attacks.")]
    public AudioClip spawnSound;
    public AudioClip attackSound;

    private bool initialized = false;

    private void OnEnable()
    {
        if (!initialized)
        {
            ResetToDefaults();
            initialized = true;
        }
    }

    /// <summary>
    /// Reset all audio clip references to default values using Resources.Load.
    /// </summary>
    public void ResetToDefaults()
    {
        // Audio files must be inside "Resources" folder
        spawnSound = Resources.Load<AudioClip>("Audio/FX/Enemy SFX/Sapper/Sapper Spawn");
        attackSound = Resources.Load<AudioClip>("Audio/FX/Enemy SFX/Warrior/Warrior Attack");
        deathVariation1 = Resources.Load<AudioClip>("Audio/FX/Enemy SFX/Warrior/Warrior Death");
        deathVariation2 = Resources.Load<AudioClip>("Audio/FX/Enemy SFX/Warrior/Warrior Death_2");
        hurtVariation1 = Resources.Load<AudioClip>("Audio/FX/Enemy SFX/Warrior/Warrior Pain");
        hurtVariation2 = Resources.Load<AudioClip>("Audio/FX/Enemy SFX/Warrior/Warrior Pain 2");

        // Reset loudness
        loudness = 2;

        // Optional: warn if any clip failed to load
        if (!spawnSound || !attackSound || !deathVariation1 || !deathVariation2 || !hurtVariation1 || !hurtVariation2)
        {
            Debug.LogWarning("[EnemyJuice] One or more default audio clips could not be loaded. Make sure they are in the Resources folder with correct paths.");
        }
    }
}
