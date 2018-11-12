using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelPanel : MonoBehaviour
{

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        LevelManager.endLevelEvent += DownScrollPanel;
    }

    private void DownScrollPanel()
    {
        Debug.Log("E' stato chiamto DownScrollPanel");
        animator.SetTrigger("DownScrollPanel");
    }
}

