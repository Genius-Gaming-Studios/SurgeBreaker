/* MARK GASKINS DEV NOTE
 * Do not worry about the fact that there may not be a UniversalPreferences.cs script in this scene.
 * The UniversalPreferences.cs script is set to not destroy on load. 
 * This means, changing options will work after using the main menu.
 * 
 * Likewise, this also means it will not work in scenes that do not already have a '$$ Universal Preferences' object. 
 * In that case, Do not be alarmed if changing options does not work after playing inside of a demo scene, it does not mean the game is broken.
 * The script will operate as intended after loading from the MENU SCENE. Or, you could fix it by just dragging in a `$$ Universal Preferences` prefab into your demo scene.
 * 6/18/2023
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Preferences Handler should handle all updating of existing preferences. Upon adding a new preference, it is notable that you will need to assign its new references to EVERY scene. This is why it could be a good idea to ensure that this script object is in a prefab at all times, as to save time.
/// </summary>
public class PreferencesHandler : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("This scene's pause menu or option menu reference to the Master Volume Slider.")] [SerializeField] Slider MasterVolume_Slider;
    [Tooltip("This scene's pause menu or option menu reference to the Voices Volume Slider.")] [SerializeField] Slider VoicesVolume_Slider;
    [Tooltip("This scene's pause menu or option menu reference to the Sound Effects Volume Slider.")] [SerializeField] Slider SFXVolume_Slider;
    [Tooltip("This scene's pause menu or option menu reference to the Sound Effects Volume Slider.")] [SerializeField] Slider MusicVolume_Slider;

    [Space(10)]
    [Tooltip("This scene's pause menu or option menu reference to the Master Volume Percentage Text (eg. 75%, MAX, 25%).")] [SerializeField] TextMeshProUGUI MasterVolume_Text;
    [Tooltip("This scene's pause menu or option menu reference to the Sound Effects Volume Percentage Text (eg. 75%, MAX, 25%).")] [SerializeField] TextMeshProUGUI SFXVolume_Text;
    [Tooltip("This scene's pause menu or option menu reference to the Voices Volume Percentage Text (eg. 75%, MAX, 25%).")] [SerializeField] TextMeshProUGUI VoicesVolume_Text;
    [Tooltip("This scene's pause menu or option menu reference to the Voices Volume Percentage Text (eg. 75%, MAX, 25%).")] [SerializeField] TextMeshProUGUI MusicVolume_Text;

    [Header("Audio Testing References")]
    [SerializeField] AudioClip gunShotClip;
    [SerializeField] AudioClip sampleVoiceClip;

    private AudioSource player;

    private void Start()
    {
        Cursor.visible = true;

        /// Update the existing values from the Player Preferences.
        Invoke(nameof(InitializeSavedValues), 0.1f);
        /// Try to assign the audio source on this object to its place.
        try
        {
            player = this.GetComponent<AudioSource>();
        }
        catch
        {
            Debug.LogWarningFormat("[Preferences Handler Notice] No audio source on the Preferences Handler object. It now assumes there will be no audio testing in the options menu of this scene.");
        }
    }

    /// <summary>
    ///  Update the existing values from the Player Preferences.
    /// </summary>
    private void InitializeSavedValues()
    {
        OnValueChanged_MasVol(FindObjectOfType<UniversalPreferences>()._universalVolume); // From the universal (master) volume player pref
        OnValueChanged_SFXVol(FindObjectOfType<UniversalPreferences>()._fxVolume); // From the SFX volume player pref
        OnValueChanged_VoiVol(FindObjectOfType<UniversalPreferences>()._voicesVolume); // From the voices volume player pref
        OnValueChanged_MusVol(FindObjectOfType<UniversalPreferences>()._musicVolume); // From the music volume player pref
    }

    /// <summary>
    /// Samples the corresponding volume when the corresponding test button is pressed.
    /// </summary>
    public void SampleGunVolume()
    {
        if (player == null) { Debug.LogErrorFormat("[Preferences Handler Error] No player assigned to play testing audio."); return; }

        player.volume = FindObjectOfType<UniversalPreferences>()._fxVolume;
        player.clip = gunShotClip;
        player.Play();
    }


    /// <summary>
    /// Samples the corresponding volume when the corresponding test button is pressed.
    /// </summary>
    public void SampleVoiceVolume()
    {
        if (player == null) { Debug.LogErrorFormat("[Preferences Handler Error] No player assigned to play testing audio."); return; }

        player.volume = FindObjectOfType<UniversalPreferences>()._voicesVolume;
        player.clip = sampleVoiceClip;
        player.Play();
    }

    /// <summary>
    /// Short cut for muting all volume.
    /// </summary>
    public void Mute_Button()
    {
        OnValueChanged_MasVol(0);
        OnValueChanged_SFXVol(0);
        OnValueChanged_VoiVol(0);
        OnValueChanged_MusVol(0);

    }


    /// <summary>
    /// When the value of the music volume slider is updated.   
    /// </summary>
    public void OnValueChanged_MusVol(float value)
    {
        /// Though this seems obsolete, this is being used for the Start Method, when they are initially initialized. It is harmless to the standard OnValueChanged method, however it is very helpful for initialization.
        MusicVolume_Slider.value = value;

        /// Set New Values
        // Set the Universal preference
        FindObjectOfType<UniversalPreferences>()._musicVolume = value;

        // Save Player Preferences data 
        PlayerPrefs.SetFloat("_musicVolume", value);
        PlayerPrefs.Save();

        // Update the corresponding text that should've been assigned.
        if (MusicVolume_Text == null) Debug.LogErrorFormat("<color=red>[Preferences Handler Error]</color> You have not assigned a volume percentage indication text for modification of the Music Volume's slider. Ensure that you've assigned it to this scene's Preferences Handler.");
        else if (value == 1) MusicVolume_Text.text = $"MAX";
        else if (value == 0) MusicVolume_Text.text = $"MUTE";
        else MusicVolume_Text.text = $"{Mathf.RoundToInt(value * 100)}%";
    }

    /// <summary>
    /// When the value of the master volume slider is updated.   
    /// </summary>
    public void OnValueChanged_MasVol(float value)
    {       
        /// Though this seems obsolete, this is being used for the Start Method, when they are initially initialized. It is harmless to the standard OnValueChanged method, however it is very helpful for initialization.
        MasterVolume_Slider.value = value;

        /// Set New Values
        // Set the Universal preference
        FindObjectOfType<UniversalPreferences>()._universalVolume = value;

        // Save Player Preferences data 
        PlayerPrefs.SetFloat("_universalVolume", value);
        PlayerPrefs.Save();
        // Update the corresponding text that should've been assigned.
        if (MasterVolume_Text == null) Debug.LogErrorFormat("<color=red>[Preferences Handler Error]</color> You have not assigned a volume percentage indication text for modification of the Master Volume's slider. Ensure that you've assigned it to this scene's Preferences Handler.");
        else if (value == 1) MasterVolume_Text.text = $"MAX";
        else if (value == 0) MasterVolume_Text.text = $"MUTE";
        else MasterVolume_Text.text = $"{Mathf.RoundToInt(value *100)}%";
    }

    /// <summary>
    /// When the value of the sound effects volume slider is updated.   
    /// </summary>
    public void OnValueChanged_SFXVol(float value)
    {
        /// Though this seems obsolete, this is being used for the Start Method, when they are initially initialized. It is harmless to the standard OnValueChanged method, however it is very helpful for initialization.
        SFXVolume_Slider.value = value;

        /// Set New Values
        // Set the Universal preference
        FindObjectOfType<UniversalPreferences>()._fxVolume = value;

        // Save Player Preferences data 
        PlayerPrefs.SetFloat("_fxVolume", value);
        PlayerPrefs.Save();

        // Update the corresponding text that should've been assigned.
        if (SFXVolume_Text == null) Debug.LogErrorFormat("<color=red>[Preferences Handler Error]</color> You have not assigned a volume percentage indication text for modification of the SFX Volume's slider. Ensure that you've assigned it to this scene's Preferences Handler.");
        else if (value == 1) SFXVolume_Text.text = $"MAX";
        else if (value == 0) SFXVolume_Text.text = $"MUTE";
        else SFXVolume_Text.text = $"{Mathf.RoundToInt(value * 100)}%";
    }

    /// <summary>
    /// When the value of the voices volume slider is updated.   
    /// </summary>
    public void OnValueChanged_VoiVol(float value)
    {
        /// Though this seems obsolete, this is being used for the Start Method, when they are initially initialized. It is harmless to the standard OnValueChanged method, however it is very helpful for initialization.
        VoicesVolume_Slider.value = value;

        /// Set New Values
        // Set the Universal preference
        FindObjectOfType<UniversalPreferences>()._voicesVolume = value;

        // Save Player Preferences data 
        PlayerPrefs.SetFloat("_voicesVolume", value);
        PlayerPrefs.Save();

        // Update the corresponding text that should've been assigned.
        if (VoicesVolume_Text == null) Debug.LogErrorFormat("<color=red>[Preferences Handler Error]</color> You have not assigned a volume percentage indication text for modification of the Voices Volume's slider. Ensure that you've assigned it to this scene's Preferences Handler.");
        else if (value == 1) VoicesVolume_Text.text = $"MAX";
        else if (value == 0) VoicesVolume_Text.text = $"MUTE";
        else VoicesVolume_Text.text = $"{Mathf.RoundToInt(value * 100)}%";
    }
}
