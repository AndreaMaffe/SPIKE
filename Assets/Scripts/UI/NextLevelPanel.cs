using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelPanel : MonoBehaviour
{

    private Animator animator;
    private SaveManager saveManager;
    private bool panelDown;

    void Start()
    {
        animator = GetComponent<Animator>();
        panelDown = false;

        //Se il livello è completato facciamo scendere il pannello della vittoria
        LevelManager.endLevelEvent += DownScrollPanel;
        //Se il livello è completato aggiorniamo il massimo livello sbloccato
        //LevelManager.endLevelEvent += UpdateMaxUnlockedLevel;
        //Se viene premuto il tasto reply al di fuori del pannello, il pannello ritorna su (probabilmente ci serve solo in fase di testing)
        LevelManager.retryLevelEvent += UpScrollPanel;
    }

    //Metodo che viene chiamato nel momento in cui il livello è stato vinto e che fa il pannello della vittoria
    private void UpScrollPanel()
    {
        if (panelDown)
        {
            animator.SetTrigger("UpScrollPanel");
            panelDown = false;
        }
            
    }

    //Metodo che viene chiamato nel momento in cui il livello è stato vinto e che fa il pannello della vittoria
    public void DownScrollPanel()
    {
        animator.SetTrigger("DownScrollPanel");
        panelDown = true;
    }

    //Metodo legato al bottone next level, aggiorna il livello corrente in modo tale che verrà caricato lo scriptable object corretto nella prossima scena
    public void NextLevel()
    {
        saveManager = SaveManager.SaveManagerInstance;
        saveManager = SaveUtility.LoadObject(saveManager, "saveFile");
        //Adesso bisogna settare currentLevel al livello successivo e caricare la scena
        LevelManager.CurrentLevelIndex += 1;
        SaveUtility.SaveObject(saveManager, "saveFile");
        SceneManager.LoadScene("SampleSceneRange");
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("SampleSceneRange");
    }

    //Metodo che della schermata di vittoria ci riporta al main menu
    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    /*
    //Metodo che aggiorna il massimo livello sbloccato
    private void UpdateMaxUnlockedLevel()
    {
        saveManager = SaveManager.SaveManagerInstance;
        saveManager = SaveUtility.LoadObject(saveManager, "saveFile");
        if (saveManager.currentLevel == saveManager.maxUnlockedLevel)
        {
            saveManager.maxUnlockedLevel += 1;
            SaveUtility.SaveObject(saveManager, "saveFile");
            Debug.Log("Ho aggiornato maxunlockedlevel a: " + saveManager.maxUnlockedLevel);
        }
    }
    */

    private void OnDestroy()
    {
        //Unregister events
        LevelManager.endLevelEvent -= DownScrollPanel;
        //LevelManager.endLevelEvent -= UpdateMaxUnlockedLevel;
        LevelManager.retryLevelEvent -= UpScrollPanel;
    }

}

