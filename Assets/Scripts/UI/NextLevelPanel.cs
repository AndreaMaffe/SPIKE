using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelPanel : MonoBehaviour
{

    private Animator animator;
    private SaveManager saveManager;

    void Start()
    {
        animator = GetComponent<Animator>();

        //Carichiamo l'oggetto SaveManager per ottenere i dati salvati del gioco
        saveManager = SaveManager.SaveManagerInstance;
        saveManager = SaveUtility.LoadObject(saveManager, "saveFile");

        LevelManager.endLevelEvent += DownScrollPanel;
    }

    public void DownScrollPanel()
    {
        animator.SetTrigger("DownScrollPanel");
    }

    public void BackToMenu()
    {
        //Controlliamo se siamo nel massimo livello sbloccato in modo tale da aumentarlo di uno
        if (saveManager.currentLevel == saveManager.maxUnlockedLevel)
        {
            saveManager.maxUnlockedLevel += 1;
            Debug.Log("Ho aggiornato maxunlockedlevel a: " + saveManager.maxUnlockedLevel);
        }
        SaveUtility.SaveObject(saveManager, "saveFile");
        SceneManager.LoadScene("MainMenu");
    }

    private void OnDestroy()
    {
        LevelManager.endLevelEvent -= DownScrollPanel;
    }



}

