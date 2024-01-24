/// MARK GASKINS 2023 ---
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Voice Line", menuName = "Add Voice Line")]
public class VoiceLine : ScriptableObject
{
    // Fields
    [Header("Modifiers")]
    [SerializeField] public TriggerCode _triggerCode = TriggerCode.PleaseAssignCode;
    [SerializeField] public AudioClip _voiceLine;
    [SerializeField] public string _caption = "Insert Text";
}
