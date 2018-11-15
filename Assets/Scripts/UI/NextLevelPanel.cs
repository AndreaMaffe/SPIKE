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
        LevelManager.endLevelEvent += UpdateMaxUnlockedLevel;
    }

    //Metodo che viene chiamato nel momento in cui il livello è stato vinto e che fa il pannello della vittoria
    public void DownScrollPanel()
    {
        animator.SetTrigger("DownScrollPanel");
    }

    public void NextLevel()
    {
        //Adesso bisogna settare currentLevel al livello successivo e caricare la scena
        saveManager.currentLevel += 1;
        SaveUtility.SaveObject(saveManager, "saveFile");
        SceneManager.LoadScene("SampleSceneRangeAle2");
    }

    public void RestartLevel()
    {

    }

    //Metodo che della schermata di vittoria ci riporta al main menu
    public void BackToMenu()
    {
        SaveUtility.SaveObject(saveManager, "saveFile");
        SceneManager.LoadScene("MainMenu");
    }

    private void OnDestroy()
    {
        //Unregister events
        LevelManager.endLevelEvent -= DownScrollPanel;
        LevelManager.endLevelEvent -= UpdateMaxUnlockedLevel;
    }

    //Metodo che aggiorna il massimo livello sbloccato
    private void UpdateMaxUnlockedLevel()
    {
        if (saveManager.currentLevel == saveManager.maxUnlockedLevel)
        {
            saveManager.maxUnlockedLevel += 1;
            SaveUtility.SaveObject(saveManager, "saveFile");
            Debug.Log("Ho aggiornato maxunlockedlevel a: " + saveManager.maxUnlockedLevel);
        }
    }

}

