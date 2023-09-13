/// MARK GASKINS 2023 ---
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TriggerCode
{
    PleaseAssignCode,
    EnemyDieCode,
    BossSpawnCode,
    PlayerHurtCode,
    PlayerDieCode,
    TurretBoughtCode
}

/// Manages the voices that are spoken during game time.
[RequireComponent(typeof(AudioSource))]
public class VoicesManager : MonoBehaviour
{
    [SerializeField] List<VoiceLine> AllVoiceLines;
    // the thing that plays the audio (AUTOMATICALLY ASSIGNED, ECHO EFFECT IS ATTACHED)
    private AudioSource CorePlayer;
    
    // Store previously used triggers to prevent voice lines from being used back to back. Increases randomness.
    private int[] triggerCodeTracker = { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 };

    private void Awake()
    {
        // Initialize stuff
        CorePlayer = FindObjectOfType<AudioSource>();
    }

    public void TriggerVoiceLine(TriggerCode triggerCode, bool isPlayerLine)
    {
        if (isPlayerLine && !playerIsSpeaking)  // This simple functionality prohibits voice lines from overlapping, if they're spoken by the player.
        {
            // Handle variations
            List<VoiceLine> voiceLineVariations = new();

            // Handle addition of the lines which share the required voice line ID
            foreach (VoiceLine line in AllVoiceLines)
            {
                if (line._triggerCode == triggerCode)
                {
                    voiceLineVariations.Add(line);
                }
            }

            // Handle Variation Shuffling
            AudioClip finalClip = ShuffleVariations(voiceLineVariations, ((int)triggerCode));


            /// Finalize and display results
            CorePlayer.PlayOneShot(finalClip);
            // Delay before the player may speak again.
            playerIsSpeaking = true;
            Invoke(nameof(AllowPlayerLines), finalClip.length + 1f);
        }
        else // Not for player. This just overrides the playerIsSpeaking delay, meaning it will speak over a playing voice line. Unless it is important, like a Boss line or a Death line, then this is not for your AudioClip.
        {
            // Handle variations
            List<VoiceLine> voiceLineVariations = new();

            // Handle addition of the lines which share the required voice line ID
            foreach (VoiceLine line in AllVoiceLines)
            {
                if (line._triggerCode == triggerCode)
                {
                    voiceLineVariations.Add(line);
                }
            }

            // Handle Variation Shuffling
            AudioClip finalClip = ShuffleVariations(voiceLineVariations, ((int)triggerCode));

            /// Finalize and display results
            CorePlayer.PlayOneShot(finalClip);
        }
    }

    // This simple functionality prohibits voice lines from overlapping, if they're spoken by the player.
    private bool playerIsSpeaking;
    private void AllowPlayerLines()
    {
        playerIsSpeaking = false;
    }

    public AudioClip ShuffleVariations(List<VoiceLine> variations, int trackerID)
    {
        AudioClip finalLine = null;
        int variationToChoose = Random.Range(0, variations.Count);

        if (variations.Count > 1)
        {
            if (triggerCodeTracker[trackerID] == variationToChoose)
            {
                // The process is denied. Try shuffling again.
                finalLine = ShuffleVariations(variations, trackerID);
            }
            else
            {
                // Process not denied. Use the expected voice line.
                finalLine = variations[variationToChoose]._voiceLine;
            }
        }
      
        // This is used to prevent the same voice line from repeating over and over.
        triggerCodeTracker[trackerID] = variationToChoose;


        return finalLine;
    }
}
// variations[variationToChoose]._voiceLine;