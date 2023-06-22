/* MARK GASKINS
 * THERE SHOULD ONLY BE ONE INSTANCE OF THIS SCRIPT.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UniversalPreferences : MonoBehaviour
{
    [Header("Read Only")]
    /// <summary>
    /// The universal master volume of the game.  (0.0f - 1.0f)
    /// </summary>
    public float _universalVolume = 1.0f;

    /// <summary>
    /// The universal FX volume of the game.  (0.0f - 1.0f)
    /// </summary>
    public float _fxVolume = 1.0f;

    /// <summary>
    /// The universal Music volume of the game.  (0.0f - 1.0f)
    /// </summary>
    public float _musicVolume = 1.0f;

    /// <summary>
    /// The universal voices volume of the game. (0.0f - 1.0f)
    /// </summary>
    public float _voicesVolume = 1.0f;

    public static UniversalPreferences Instance { get; private set; }
    private void Awake()
    {
        /// Bring back the saved values
        _universalVolume = PlayerPrefs.GetFloat("_universalVolume", 1.00f);
        _fxVolume = PlayerPrefs.GetFloat("_fxVolume", .50f);
        _voicesVolume = PlayerPrefs.GetFloat("_voicesVolume", .75f);
        _musicVolume = PlayerPrefs.GetFloat("_musicVolume", .4f);

        DontDestroyOnLoad(this.gameObject);
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void Update()
    {
        HandleGlobalVolume();
    }

    /// <summary>
    /// Updates the global volume levels in the scene based on the universal volume levels.
    /// </summary>
    public void HandleGlobalVolume()
    {
        // This is the master volume, which puts a ceiling on all audio levels.
        AudioListener.volume = _universalVolume;

        // This is the music volume levels. Notice: For the music object to be found, it must have the music tag script assigned to it.
         foreach(MusicPlayer musicPlayer in FindObjectsOfType<MusicPlayer>())
        {
            musicPlayer.GetComponent<AudioSource>().volume = _musicVolume;
        }

        // This is the fx volume levels. Notice: For the fx object to be found, it must have the music tag script assigned to it.
        foreach (FXPlayer fxPlayer in FindObjectsOfType<FXPlayer>())
        {
            fxPlayer.GetComponent<AudioSource>().volume = _fxVolume;
        }

        // This is the voices volume levels. Notice: For the voices object to be found, it must have the music tag script assigned to it.
        foreach (VoicesPlayer voicesPlayer in FindObjectsOfType<VoicesPlayer>())
        {
            voicesPlayer.GetComponent<AudioSource>().volume = _voicesVolume;
        }
    }

}
