using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public enum GameMode
{
    Idle,
    Build,
    Combat,
    GameOver,
    LvlComplete,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}

    [Space(10)]
    [Header("Debug Controls [DEBUG ONLY]")]
    [Tooltip("[DEBUG ONLY] Do infinite money")] [SerializeField] public bool doInfiniteMoney;
    [Tooltip("[DEBUG ONLY] Do remove wait times")] [SerializeField] public bool doRemoveWaitTimes;
    [Tooltip("[DEBUG ONLY] Do allow press tab to toggle game mode")] [SerializeField] public bool doAllowTabToggling;

    [Space(10)]
    [Header("Game Bosses")]
    [Tooltip("(Important) These are the cycle numbers that bosses will spawn on.  *More details below. \n\n[Boss Wave Description]\n- At the end of the cycle, there will be a 3-5 second pause, before a big boss should spawn. \n- After beating the boss, the game will continue along with the usual sequence.\n- Multiple bosses can spawn in a level.\n- The cycle(s) that bosses spawn on are determined by a list called the 'BossCycles'.\n- The boss that spawns on the Boss Wave is determined by a list called the 'BossWaveBosses'.\n- The boss that will spond corresponds with the boss wave that it should spawn at, as set by the BossCycles list.\n\t(e.g. if the first element of the BossCycles list is [2], then on Cycle [2], the boss from the first element of the 'BossWaveBosses' will spawn.)\n\t(The boss from the 'BossWaveBosses' list can either be a path boss or a player boss. But either way, they should be super slow!)")] [SerializeField] public List<int> BossCycles;
    [Tooltip("(Important) These are the bosses that will spawn on their corresponding 'BossCycle' from the 'BossCycles' list. *More details below. \n\n[Boss Wave Description]\n- At the end of the cycle, there will be a 3-5 second pause, before a big boss should spawn. \n- After beating the boss, the game will continue along with the usual sequence.\n- Multiple bosses can spawn in a level.\n- The cycle(s) that bosses spawn on are determined by a list called the 'BossCycles'.\n- The boss that spawns on the Boss Wave is determined by a list called the 'BossWaveBosses'.\n- The boss that will spond corresponds with the boss wave that it should spawn at, as set by the BossCycles list.\n\t(e.g. if the first element of the BossCycles list is [2], then on Cycle [2], the boss from the first element of the 'BossWaveBosses' will spawn.)\n\t(The boss from the 'BossWaveBosses' list can either be a path boss or a player boss. But either way, they should be super slow!)")] [SerializeField] public List<Enemy> BossWaveBosses;

    [Header("Difficulty Modifiers")]
    [Tooltip("(Difficulty) The amount of times that the game cycles between build mode and combat mode.")] [SerializeField] [Range(1, 100)] public int amountOfCycles;
    [Tooltip("(Difficulty) The amount of enemies to spawn per spawner.")] [SerializeField] [Range(1, 100)] public int enemiesPerSpawner;
    [Tooltip("(Difficulty) The amount of enemies that are added to the enemiesPerSpawner, per each cycled.")] [SerializeField] [Range(1, 40)] public int enemiesIncreasePerCycle;
    [Tooltip("(Difficulty) The amount of enemies that spawn per generator in wave one.")] [SerializeField] [Range(1, 25)] public int waveOneEnemies = 10;
    [Tooltip("(Difficulty) The amount of extra money given to the player at the beginning of each round.")] [SerializeField] [Range(25, 125)] int waveMoneyBoost = 25;
    [Tooltip("(Difficulty) The money boost multiplier given to the money boost at the beginning of each round. \n\t(1.0 > No boost every round, 2.0 > Doubled every round)")] [SerializeField] [Range(1.0f, 2.0f)] float moneyBoostMultiplier = 1.2f;

    [Tooltip("This is the amount of time that a player will have in either of the specified modes.")] [SerializeField] [Range(10, 49)] public int timeInBuildMode = 30, timeInCombatMode = 30, timeInWaveOneBuild = 30;

    [Space(10)]
    [Header("Technical")]
    [Tooltip("This is the current GameMode that majorly effects how the game acts. Idle = 0, Build = 1, Combat = 2")] public GameMode currentMode = GameMode.Build;

    [SerializeField] GameObject CombatCanvas, BuildCanvas, MainCanvas, GameOverCanvas, MissionSucessCanvas;
    [Tooltip("This will be turned off when the player is in Build Mode!")] [SerializeField] GameObject WeaponsParent;
    [Tooltip("This is the text shown for teh game time that is remaining.")] [SerializeField] TextMeshProUGUI GameTimerText;
    [Tooltip("This is the text shown for the current game cycle.")] [SerializeField] TextMeshProUGUI GameCycleText;
    [Tooltip("This is the text shown for the current amount of enemies alive.")] [SerializeField] TextMeshProUGUI EnemiesAliveText;
    [Tooltip("Turns off when the player wins or loses.")] [SerializeField] GameObject GameMusicPlayer;
    [Tooltip("Shown each time the player wins a round. This is the amount of extra health that they were given.")] [SerializeField] TextMeshProUGUI MoneyBoostText, EnemyBoostText;
    private float timerTime = 0; /// The Displayed timer time on the GUI. Not for technical functionality, only for the GUI functionality.
    private int currentCycle = 1;

    private Color gameTimeDefaultColor; // Default color of the game timer, used when making time turn red for the last 9 seconds of a cycle.
    public static int enemiesAlive;
    public int _enemiesAlive;

    public static bool hasWon;

    [Tooltip("A reference for the health object of each of the generators in the level.")] [SerializeField] Health[] GeneratorsInLevel;

    private void Awake()
    {
        // Check if there is already an Instance of this in the scene
        if (Instance != null)
        {   
            // Destroy this extra copy if this is
            Destroy(gameObject);
            Debug.LogError("Cannot Have More Than One Instance of [GameManager] In The Scene!");
            return;
        } 

        Instance = this;
    }
     
    private void Start()
    {
        GameOverCanvas.SetActive(false);
        MissionSucessCanvas.SetActive(false);

        // Checks if the TutorialManager is running first before starting gameplay
        if (currentMode != GameMode.Idle)
        {
            StartGameCycles();
        }
        
        if (doRemoveWaitTimes) timeInWaveOneBuild = 0;
        enemiesAlive = 0;
        hasWon = false;

        gameTimeDefaultColor = GameTimerText.color;

        

        Time.timeScale = 1.0f;
        AudioListener.pause = false;
    }



    /// <summary>
    /// Update timer time every second.
    /// </summary>
    private void TickGameTimer()
    {
        if (timerTime > 0) timerTime -= 0.1f;

        // Format the text
        if (timerTime > 9) { GameTimerText.color = gameTimeDefaultColor; GameTimerText.text = timerTime.ToString("##.#"); }
        else { GameTimerText.color = Color.red; GameTimerText.text = timerTime.ToString("#.##"); }
    }

    private void UpdateGenerators()
    {
        /// Handle health of generators 
        List<bool> generatorIsAlive = new List<bool>();
        foreach (Health generatorHealth in GeneratorsInLevel) { if (generatorHealth.currentHealth > 0) generatorIsAlive.Add(true); else generatorIsAlive.Add(false); }


        bool hasLost = false;

        for (int i = 0; i < generatorIsAlive.Count; ++i)
        {
            if (!generatorIsAlive[i])
            {
                hasLost = true;
            }


            if (hasLost)
            {
                PlayerManager.generatorsDestroyed = true;
                foreach (MusicPlayer player in FindObjectsOfType<MusicPlayer>()) player.GetComponent<AudioSource>().Stop();
                Debug.Log("A generator in this level has been destroyed. Player has lost.");
            }
        }

    }

    private void DoBossCheck()
    {
        bossCheckComplete = false;
        // Begin checking to see if this is a boss round or not
        isBossRound = false;
        bossToSpawn = 0;

        for (int bossID = 0; bossID < BossCycles.Count; bossID++)
        {

            if (BossCycles[bossID] == currentCycle) 
            {
                Debug.Log("<color=green> Proceed to spawn boss.</color>");

                FindObjectOfType<VoicesManager>().TriggerVoiceLine(TriggerCode.BossSpawnCode, false); // It is false because we need it to override.

                isBossRound = true; 
                bossToSpawn = bossID; 
            }
        }

        bossCheckComplete = true;
    }

    bool bossCheckComplete = false;
    bool isBossRound = false;
    int bossToSpawn = 0;

    private IEnumerator GameCycleSequence()
    {
        for (int i = 1; i < amountOfCycles + 1; i++)
        {
            Debug.Log($"<color=cyan>[Game Manager]</color> Begin Cycle {i}/{amountOfCycles}.");
            GameCycleText.text = $"{i}/{amountOfCycles}";
            enemiesAlive = 0;

            /// Starts in build mode
            SwitchGamemode(GameMode.Build);
            if (i > 1)
            {
                timerTime = timeInBuildMode;
                yield return new WaitForSeconds(timeInBuildMode);
            }
            else // Distinction between wave one difficulty and all of the other difficulties
            {
                timerTime = timeInWaveOneBuild;
                yield return new WaitForSeconds(timeInWaveOneBuild);
            }
            /// Provide 2 seconds between cycles. Do not show on the game timer. This is invisible time.
            timerTime = 0;
            yield return new WaitForSeconds(2);
            /// Switch to combat mode
            SwitchGamemode(GameMode.Combat);
            /// Initiate the combat mode
            foreach (EnemySpawner spawner in FindObjectsOfType<EnemySpawner>())
            {
                if (i > 1)
                {
                    EnemyBoostText.text = $"+{enemiesPerSpawner}";
                    EnemyBoostText.GetComponent<Animation>().Play();

                    spawner.enemyHealthIncrease *= 2;
                    spawner.enemiesToSpawn = enemiesPerSpawner;
                    enemiesAlive += enemiesPerSpawner;
                }
                else // Distinction between wave one difficulty and all of the other difficulties
                {
                    spawner.enemiesToSpawn = waveOneEnemies;
                    enemiesAlive += waveOneEnemies;
                }

                spawner.StartCoroutine(spawner.SpawnWave());
            }
            /// Wait until all enemies are dead, not for the timer, to set the round to a win state.
            timerTime = 0;
            while (enemiesAlive > 0) yield return null;

            DoBossCheck();
            while (!bossCheckComplete) yield return null;

            /// Boss round returned true, begin operating boss round.
            if (isBossRound)
            {
                yield return new WaitForSeconds(3);
                enemiesAlive++; // Register boss enemy

                /// Find spawner for boss enemy
                List<EnemySpawner> AllSpawners = new List<EnemySpawner>();
                foreach (EnemySpawner spawner in FindObjectsOfType<EnemySpawner>()) AllSpawners.Add(spawner);

                AllSpawners[Random.Range(0, AllSpawners.Count)].Spawn(BossWaveBosses[bossToSpawn].gameObject);

                /// Wait until boss is dead, not for the timer, to set the round to a win state.
                timerTime = 0;
                while (enemiesAlive > 0) yield return null;
            }


            /// Win Delay ------------------------
            /// Provide 5 seconds between cycles. Do not show on the game timer. This is invisible time.
            timerTime = 0;
            yield return new WaitForSeconds(2.5f);
            // Show the money boost dislay
            MoneyBoostText.text = $"+{waveMoneyBoost}";
            MoneyBoostText.GetComponent<Animation>().Play();
            // Add extra money to the player, because they completed the round.
            PlayerManager.currentCurrency += waveMoneyBoost;
            yield return new WaitForSeconds(2.5f);

            /// Cycle has been won
            currentCycle++;
            waveMoneyBoost = Mathf.RoundToInt(waveMoneyBoost * moneyBoostMultiplier);

            if (i > 1)
            {
                enemiesPerSpawner += enemiesIncreasePerCycle; // Increase game difficulty if it isn't the first wave.
            }
            /// Game Has been won
            if (i == amountOfCycles)
            {
                hasWon = true;


                // Store Win Data (save)
                if (SceneManager.GetActiveScene().name.ToLower().Trim() == "level 1") PlayerPrefs.SetInt("winlv1", 1);
                if (SceneManager.GetActiveScene().name.ToLower().Trim() == "level 2") PlayerPrefs.SetInt("winlv2", 1);
                if (SceneManager.GetActiveScene().name.ToLower().Trim() == "level 3") PlayerPrefs.SetInt("winlv3", 1);
                if (SceneManager.GetActiveScene().name.ToLower().Trim() == "level 4") PlayerPrefs.SetInt("winlv4", 1);
                if (SceneManager.GetActiveScene().name.ToLower().Trim() == "level 5") PlayerPrefs.SetInt("winlv5", 1);
                if (SceneManager.GetActiveScene().name.ToLower().Trim() == "level 6") PlayerPrefs.SetInt("winlv6", 1);
                if (SceneManager.GetActiveScene().name.ToLower().Trim() == "level 7") PlayerPrefs.SetInt("winlv7", 1);
                if (SceneManager.GetActiveScene().name.ToLower().Trim() == "level 8") PlayerPrefs.SetInt("winlv8", 1);
                if (SceneManager.GetActiveScene().name.ToLower().Trim() == "level 9") PlayerPrefs.SetInt("winlv9", 1);
                if (SceneManager.GetActiveScene().name.ToLower().Trim() == "level 10") PlayerPrefs.SetInt("winlv10", 1);

                PlayerPrefs.Save();

                LevelComplete();
                Debug.Log("<b>[Game Manager]</b> <color=green>Game Won! (Show UI Prompt Now)</color>");
            }
        }
    }


    /// <summary>
    /// The only audio player that the FX come out of.
    /// </summary>
    public AudioSource CoreFXPlayer;

    /// <summary>
    /// The only audio player that the Music should come out of.
    /// </summary>
    public AudioSource CoreMusicPlayer;

    public static AudioSource GetCorePlayer() { return FindObjectOfType<GameManager>().CoreFXPlayer; }

    [SerializeField] public GameObject BuildMenu, DeleteMenu;


    private void Update()
    {
        // Do nothing if this is set to "Idle"
        if (currentMode == GameMode.Idle) return;


        if (doAllowTabToggling && Input.GetKeyDown(KeyCode.Tab))
        {
            switch (currentMode)
            {
                case GameMode.Build: SwitchGamemode(GameMode.Combat); break;
                case GameMode.Combat: SwitchGamemode(GameMode.Build); break;

            }
        }


        if (doRemoveWaitTimes)
        {
            timeInBuildMode = 0;
            timeInCombatMode = 0;
            timeInWaveOneBuild = 0;
        }


        _enemiesAlive = enemiesAlive;
        EnemiesAliveText.text = enemiesAlive.ToString();
        switch (currentMode)
        {
            case GameMode.Build:
                MainCanvas.SetActive(true);
                CombatCanvas.SetActive(false);
                BuildCanvas.SetActive(true);
                WeaponsParent.SetActive(false);

                foreach (BuildNode node in FindObjectsOfType<BuildNode>())
                {
                    node.Enable(); // Show all node mesh renderes
                }


                break;

            case GameMode.Combat:
                CombatCanvas.SetActive(true);
                BuildCanvas.SetActive(false);
                WeaponsParent.SetActive(true);
                foreach (BuildNode node in FindObjectsOfType<BuildNode>()) node.Disable(); // Hide all node mesh renderers

                break;

            case GameMode.GameOver:
                MainCanvas.SetActive(false);
                GameOverCanvas.SetActive(true);
                GameMusicPlayer.SetActive(false);
                Cursor.visible = true;
                break;

            case GameMode.LvlComplete:

                MainCanvas.SetActive(false);
                MissionSucessCanvas.SetActive(true);
                GameMusicPlayer.SetActive(false);

                Cursor.visible = true;
                break;

            default:
                CombatCanvas.SetActive(false);
                BuildCanvas.SetActive(false);
                WeaponsParent.SetActive(false);
                foreach (BuildNode node in FindObjectsOfType<BuildNode>()) node.Disable(); // Hide all node mesh renderers

                break;
        }



        // Surge Breaker (1.5p)
        // This will stop all coroutines, and pause the game. This means, animations will need to be animators and mustn't rely on TimeScale if an animation is to be seen within Build Mode. 
        // This is primarily used for pausing the spawning of waves while in build mode.
        // This method may be deprecated in the future due to all of the problems that may come from pausing time itself while in build mode. Try not to take this off of your radar.

        // if (currentMode == GameMode.Build) Time.timeScale = 0;
        // else Time.timeScale = 1; // Normalize Time Scale. This may need more TLC if there is a pause menu that is implemented.
    }



    public void SwitchGamemode(GameMode switchTo) // This manually alternates the gamemode
    {
        currentMode = switchTo;

        Debug.Log($"Switched gamemode to <color=\"yellow\">{currentMode}</color>.");
    }


    public void SwitchGamemode() // This automatically alternates the gamemode when you press tab
    {
        if (currentMode == GameMode.Build)
        {
            currentMode = GameMode.Combat;
        }
        else if (currentMode == GameMode.Combat)
        {
            currentMode = GameMode.Build;
        }
        else if (currentMode.GetHashCode() == 0)
        {
            currentMode++;
        }

        Debug.Log($"Switched gamemode to <color=\"yellow\">{currentMode}</color>.");
    }

    public void GameOver() // Called by the PlayerManager when a lose condition is met
    {
        SwitchGamemode(GameMode.GameOver);
    }

    public void LevelComplete()
    {
        SwitchGamemode(GameMode.LvlComplete);


    }

    public void LoadLevel(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
    }

    public void HideAllUI()
    {
        GameOverCanvas.SetActive(false);
        MissionSucessCanvas.SetActive(false);
        MainCanvas.SetActive(false);
    }

    public void StartGameCycles()
    {
        /// Starts all the necessary cycles to start the actual gameplay

        /// Initialize the game, Restart static values
            InvokeRepeating(nameof(UpdateGenerators), 0, 3);
            InvokeRepeating(nameof(TickGameTimer), 0, .1f);
            StartCoroutine(GameCycleSequence());
            SwitchGamemode(GameMode.Build);

            MainCanvas.SetActive(true);
    }

}
