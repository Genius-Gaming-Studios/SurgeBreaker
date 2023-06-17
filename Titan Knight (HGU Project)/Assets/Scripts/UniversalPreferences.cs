/* MARK GASKINS
 * THERE SHOULD ONLY BE ONE INSTANCE OF THIS SCRIPT.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalPreferences : MonoBehaviour
{
    /// <summary>
    /// The universal master volume of the game.  (0.0f - 1.0f)
    /// </summary>
    public static float _universalVolume = 1.0f;

    /// <summary>
    /// The universal FX volume of the game.  (0.0f - 1.0f)
    /// </summary>
    public static float _fxVolume = 1.0f;

    /// <summary>
    /// The universal voices volume of the game. (0.0f - 1.0f)
    /// </summary>
    public static float _voicesVolume = 1.0f;

    public static UniversalPreferences Instance { get; private set; }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
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
        // This is the master volume, which ove
        AudioListener.volume = _universalVolume;
    }

}
