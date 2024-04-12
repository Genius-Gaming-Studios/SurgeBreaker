/// Coded by Mark Anthony Gaskins II, April 2024
/// Contact me at => mgazillion@thevillagemethod.org
/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Juice Controller", menuName = "New Enemy Juice Module")]
public class EnemyJuice : ScriptableObject
{
    [HideInInspector] [Tooltip("(left) Quieter -> Louder (right); This is the chance that the enemy will make a sound when asked to.")] [Range(6,0)] public int loudness = 2;

    [Header("Enemy SFX References")]
    [Tooltip("Hit sound is randomly chosen between hurtVariation1 and hurtVariation2 when the when the enemy gets hit.")] public AudioClip hurtVariation1;
    [Tooltip("Hit sound is randomly chosen between hurtVariation1 and hurtVariation2 when the when the enemy gets hit.")] public AudioClip hurtVariation2;

    [Tooltip("Hit sound is randomly chosen between deathVariation1 and deathVariation2 when the when the enemy gets killed.")] public AudioClip deathVariation1;
    [Tooltip("Hit sound is randomly chosen between deathVariation1 and deathVariation2 when the when the enemy gets killed.")] public AudioClip deathVariation2;

    [Tooltip("These audios will play when the enemy spawns/attacks")] public AudioClip spawnSound, attackSound;

    private bool initialized = false;

    //e


    private void OnEnable()
    {
        if (!initialized)
        {
            ResetToDefaults();
            initialized = true;
        }
    }

    public void ResetToDefaults()
    {
        // Set default audio clip paths here
        string defaultSpawnSoundPath = "Assets/Audio/FX/Enemy SFX/Sapper/Sapper Spawn.wav";
        string defaultAttackSoundPath = "Assets/Audio/FX/Enemy SFX/Warrior/Warrior Attack.wav";
        string defaultDeathSound1Path = "Assets/Audio/FX/Enemy SFX/Warrior/Warrior Death.wav";
        string defaultDeathSound2Path = "Assets/Audio/FX/Enemy SFX/Warrior/Warrior Death_2.wav";
        string defaultHitSound1Path = "Assets/Audio/FX/Enemy SFX/Warrior/Warrior Pain.wav";
        string defaultHitSound2Path = "Assets/Audio/FX/Enemy SFX/Warrior/Warrior Pain 2.wav";

        // Load default audio clips
        spawnSound = UnityEditor.AssetDatabase.LoadAssetAtPath<AudioClip>(defaultSpawnSoundPath);
        attackSound = UnityEditor.AssetDatabase.LoadAssetAtPath<AudioClip>(defaultAttackSoundPath);
        deathVariation1 = UnityEditor.AssetDatabase.LoadAssetAtPath<AudioClip>(defaultDeathSound1Path);
        deathVariation2 = UnityEditor.AssetDatabase.LoadAssetAtPath<AudioClip>(defaultDeathSound2Path);
        hurtVariation1 = UnityEditor.AssetDatabase.LoadAssetAtPath<AudioClip>(defaultHitSound1Path);
        hurtVariation2 = UnityEditor.AssetDatabase.LoadAssetAtPath<AudioClip>(defaultHitSound2Path);
        loudness = 2;
    }
}
