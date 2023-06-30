using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour
{

    public static bool GameIsPaused = false;

    [SerializeField, Tooltip("The parent Gameobject to all UI Panels")] private GameObject _pauseMenuPanel;
    [SerializeField, Tooltip("Loading Progress Slider")]                private Slider     _loadSlider;
    [SerializeField, Tooltip("Text inside loading slider")]             private TMP_Text   _loadText;

    private void Awake()
    {
       _pauseMenuPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !PlayerManager.isDead && !PlayerManager.generatorsDestroyed)
        {
            GameIsPaused = !GameIsPaused;

            if (GameIsPaused) PauseGame();
            else ResumeGame();
        }

    }

    public void ResumeGame()
    {
        _pauseMenuPanel.SetActive(false);

        /// The audio listeners must unpause when the game is resumed.
        AudioListener.pause = false;
        
        FindObjectOfType<PlayerManager>().CursorsParent.gameObject.SetActive(true);

        /// Cursor must be hidden ingame..
        Cursor.visible = false;

        Time.timeScale = 1.0f; // Resume time while game is playing
        GameIsPaused = false;
    }

    /// <summary>
    /// Automatically reloads the game
    /// </summary>
    public void RetryGame()
    {
        LoadLevel(SceneManager.GetActiveScene().buildIndex);
    }
    public void PauseGame()
    {
        _pauseMenuPanel.SetActive(true);

        /// The audio listeners must pause when the game is resumed.
        AudioListener.pause = true;

        /// Cursor must be visible in the menu..
        Cursor.visible = true;

        /// The cursors parent must be turned off, otherwise, cursors get in the way of the menu.
        FindObjectOfType<PlayerManager>().CursorsParent.gameObject.SetActive(false);

        Time.timeScale = 0.0f; // Freeze time while game is paused
        GameIsPaused = true;
    }

    public void LoadLevel(int sceneIndex)
    {
        /// Notice: It is crutial to resume the game before switching scenes.
        AudioListener.pause = false;
        Time.timeScale = 1.0f;
        Cursor.visible = true;

        /// Now that it is unpaused, the scene may be loaded asynchronously.. 
        StartCoroutine(LoadSceneAsynchronously(sceneIndex));
    }

    private IEnumerator LoadSceneAsynchronously (int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!operation.isDone) 
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            _loadSlider.value = progress;
            _loadText.text = progress * 100 + "%";
            yield return null;
        }
    }
}
