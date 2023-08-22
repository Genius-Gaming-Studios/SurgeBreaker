using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TutorialEnemyAnimator : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        StartCoroutine(RandomizeAnimations());
    }

    private IEnumerator RandomizeAnimations()
    {
        while (true)
        {
            int animationIndex = Random.Range(1, 4);
            _animator.SetInteger("AttackIndex", animationIndex);
            Debug.Log("Attack Index " + animationIndex);
            yield return new WaitForSeconds(.5f);
        }
        
    }
}
