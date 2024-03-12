 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class StartMenu_Controller : MonoBehaviour
{
    public static StartMenu_Controller Instance {get; private set;}

    [Header("[DEBUG]")]
    public bool doUnlockAllLevels;
    [Header("Level Select Screen")]

    [SerializeField] Button Level2_Button;
    [SerializeField] Button Level3_Button, Level4_Button, Level5_Button, Level6_Button, Level7_Button, Level8_Button, Level9_Button, Level10_Button;
    [SerializeField] GameObject Level1Hint, Level2Hint, Level3Hint, Level4Hint, Level5Hint;

    [Header("Loadout Screen")]

    [SerializeField][Tooltip("The prefab of the turret button")] private Transform _mechButtonPrefab;

    [SerializeField][Tooltip("The transform component of the turret button container")] private Transform _mechButtonContainerTransform;

    [SerializeField] private List<Turret> fullTurretList = new List<Turret>();
    [SerializeField] private Loadout _defaultLoadout;

    [Header("References")]
    public GameObject MainScreen;
    public GameObject LevelScreen;
    public GameObject OptionsScreen;
    public GameObject LoadoutScreen;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else 
        {
            Debug.LogError("Only one instance of [StartMenu_Controller] can exist in the scene!");
            Destroy(this.gameObject);
            return;
        }

        AudioListener.pause = false;

        MainScreen.SetActive(true);
        LevelScreen.SetActive(false);
        LoadoutScreen.SetActive(false);
    }

    public void OptionsButton()
    {
        MainScreen.SetActive(false);
        OptionsScreen.SetActive(true);
    }

    public void ReturnButton()
    {
        MainScreen.SetActive(true);
        OptionsScreen.SetActive(false);
        LevelScreen.SetActive(false);
        LoadoutScreen.SetActive(false);
    }

    public void LevelsButton()
    {
        MainScreen.SetActive(false);
        LevelScreen.SetActive(true);
    }

   public void LoadLevel(string levelName) 
   {
        SceneManager.LoadScene(levelName);
   }

   public void LoadoutButton()
   {
        MainScreen.SetActive(false);
        LevelScreen.SetActive(false);
        LoadoutScreen.SetActive(true);

        LoadoutUI.Instance.CreateLoadoutUI();
   }

    /// <summary>
    /// Caution: Will clear ALL player prefs and saved data!
    /// </summary>
    public void ResetAllData()
    {
        Debug.Log("<color=#ff4133><b>[Game Manager]</b></color> <color=#ff948c>Cleared all data. Can not be restored.</color>");

        PlayerPrefs.DeleteAll();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Update()
    {
        CheckForResetData();

        // Win Level UI ------
        if (!doUnlockAllLevels)
        {
            if (PlayerPrefs.GetInt("winlv1", 0) == 1) { Level2_Button.interactable = true; } else Level2_Button.interactable = false;
            if (PlayerPrefs.GetInt("winlv2", 0) == 1) { Level3_Button.interactable = true; } else Level3_Button.interactable = false;
            if (PlayerPrefs.GetInt("winlv3", 0) == 1) Level4_Button.interactable = true; else Level4_Button.interactable = false;
            if (PlayerPrefs.GetInt("winlv4", 0) == 1) Level5_Button.interactable = true; else Level5_Button.interactable = false;
            if (PlayerPrefs.GetInt("winlv5", 0) == 1) Level6_Button.interactable = true; else Level6_Button.interactable = false;
            if (PlayerPrefs.GetInt("winlv6", 0) == 1) Level7_Button.interactable = true; else Level7_Button.interactable = false;
            if (PlayerPrefs.GetInt("winlv7", 0) == 1) Level8_Button.interactable = true; else Level8_Button.interactable = false;
            if (PlayerPrefs.GetInt("winlv8", 0) == 1) Level9_Button.interactable = true; else Level9_Button.interactable = false;
            if (PlayerPrefs.GetInt("winlv9", 0) == 1) Level10_Button.interactable = true; else Level10_Button.interactable = false;
        }
        else
        {
            Level2_Button.interactable = true;
            Level3_Button.interactable = true;
            Level4_Button.interactable = true;
            Level5_Button.interactable = true;
            Level6_Button.interactable = true;
            Level7_Button.interactable = true;
            Level8_Button.interactable = true;
            Level9_Button.interactable = true;
            Level10_Button.interactable = true;
        }

        if (PlayerPrefs.GetInt("winlv1", 0) == 0 && PlayerPrefs.GetInt("winlv2", 0) == 0) Level1Hint.SetActive(true); else Level1Hint.SetActive(false);
        if (PlayerPrefs.GetInt("winlv2", 0) == 0 && PlayerPrefs.GetInt("winlv1", 0) == 1) Level2Hint.SetActive(true); else Level2Hint.SetActive(false);
        if (PlayerPrefs.GetInt("winlv3", 0) == 0 && PlayerPrefs.GetInt("winlv2", 0) == 1) Level3Hint.SetActive(true); else Level3Hint.SetActive(false);
        if (PlayerPrefs.GetInt("winlv4", 0) == 0 && PlayerPrefs.GetInt("winlv3", 0) == 1) Level4Hint.SetActive(true); else Level4Hint.SetActive(false);
        if (PlayerPrefs.GetInt("winlv5", 0) == 0 && PlayerPrefs.GetInt("winlv4", 0) == 1) Level5Hint.SetActive(true); else Level5Hint.SetActive(false);

    }
    public void ExitGame()
   {
        Application.Quit();
   }

   public Loadout GetLoadout()
   {
        return _defaultLoadout;
   }

    private bool ctrlPressed = false;
    private bool zeroPressed = false;
    private float ctrlZeroHoldTime = 3f;
    private float currentHoldTime = 0f;

    private void CheckForResetData()
    {
        // Check for Ctrl key press
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightControl))
        {
            ctrlPressed = true;
        }

        // Check for '0' key press
        if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0))
        {
            zeroPressed = true;
        }

        // Check for Ctrl key release
        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightControl))
        {
            ctrlPressed = false;
        }

        // Check for '0' key release
        if (Input.GetKeyUp(KeyCode.Alpha0) || Input.GetKeyUp(KeyCode.Keypad0))
        {
            zeroPressed = false;
        }

        // If Ctrl + '0' are both pressed, start counting the hold time
        if (ctrlPressed && zeroPressed)
        {
            currentHoldTime += Time.deltaTime;

            // If the hold time exceeds the desired duration, run the function
            if (currentHoldTime >= ctrlZeroHoldTime)
            {
                ResetAllData();
            }
        }
        else
        {
            // Reset the hold time if either Ctrl or '0' are released
            currentHoldTime = 0f;
        }
    }

}
