using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance {get; private set;}

    [SerializeField] private bool _isTutorialEnabled;

    private void Awake()
    {
        // Check if there is already an Instance of this in the scene
        if (Instance != null)
        {   
            // Destroy this extra copy if this is
            Destroy(gameObject);
            Debug.LogError("Cannot Have More Than One Instance of [TutorialManager] In The Scene!");
            return;
        } 

        Instance = this;
    }

    private void Start()
    {
        // Do nothign if the tutorial is disabled
        if (!_isTutorialEnabled) return;

        GameManager.Instance.HideAllUI();
        GameManager.Instance.SwitchGamemode(GameMode.Idle);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) GameManager.Instance.StartGameCycles();
    }
}
