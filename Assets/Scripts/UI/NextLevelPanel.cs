using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelPanel : MonoBehaviour
{

    public LevelManager levelManager;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        LevelManager.endLevelEvent += DownScrollPanel;
    }

    public void DownScrollPanel()
    {
        animator.SetTrigger("DownScrollPanel");
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }


    
}

