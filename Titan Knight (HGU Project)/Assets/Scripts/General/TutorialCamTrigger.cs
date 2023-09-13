using System.Collections;
using System.Collections.Generic;
using SlimUI.Vivid;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialCamTrigger : MonoBehaviour
{
    [SerializeField]
    private float timer = 0;
    private void OnTriggerEnter(Collider other) 
    {
        if (!other.CompareTag("Tutorial")) return; // Do nothing if this trigger is not the camTrigger

        timer = 3;
        TutorialManager.Instance.EnablePanCamera();  
        Destroy(other.gameObject);  
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0) 
        {
            PlayerManager.Instance.enabled = true; 
            TutorialManager.Instance.DisablePanCamera();
            timer = 0;
        } else 
        {
            PlayerManager.Instance.ResetAnimations();
            PlayerManager.Instance.enabled = false; // Disable player controls during 'Cutscene'
        }
        
    }
}
