using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextLevelPanel : MonoBehaviour
{
    private Animator animator;
    private SaveManager saveManager;
    private bool panelDown;

    public GameObject blackBackground;

    public Text GoodJobText;
    public Text levelNumberText;
    public Text numberOfAttemptsText;

    public Image[] endLevelStars;
    public Sprite whiteStart;


    void Start()
    {
        animator = GetComponent<Animator>();

        //Se il livello è completato facciamo scendere il pannello della vittoria
        LevelManager.endLevelEvent += DownScrollPanel;
        LevelManager.endLevelEvent += EndLevelUIUpdate;

        //Se il livello è completato aggiorniamo il massimo livello sbloccato
        //LevelManager.endLevelEvent += UpdateMaxUnlockedLevel;
        //Se viene premuto il tasto reply al di fuori del pannello, il pannello ritorna su (probabilmente ci serve solo in fase di testing)
        LevelManager.retryLevelEvent += UpScrollPanel;

    }

    private void UpScrollPanel()
    {
        if (panelDown)
        {
            animator.SetTrigger("UpScrollPanel");
            blackBackground.SetActive(false);
            panelDown = false;
        }
            
    }

    public void DownScrollPanel()
    {
        animator.SetTrigger("DownScrollPanel");
        blackBackground.SetActive(true);
        panelDown = true;
    }

    void EndLevelUIUpdate()
    {
        levelNumberText.text = "LEVEL: " + (LevelManager.CurrentLevelIndex +1);
        int numberOfStars = LevelManager.GetNumberOfStars();

        if (numberOfStars == 0)
        {
            GoodJobText.text = "YOU HAVE TO PLACE OBSTACLES!";
            GoodJobText.fontSize = 70;
        }
        else
        {            
            GoodJobText.text = "GOOD JOB!";
            GoodJobText.fontSize = 130;
        }

        //prima azzero in caso ci siano problemi
        for (int i = 0; i < 3; i++)   
            endLevelStars[i].overrideSprite = null;
        //dopo coloro quelle giuste
        for (int i = 0; i < numberOfStars; i++)   
            endLevelStars[i].overrideSprite = whiteStart;
        
    }

    /*
    //*** LA LOGICA DI GIOCO NON DOVREBBE STARE NELLA UI! (Spostare in LevelManager) ***
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
        LevelManager.endLevelEvent -= DownScrollPanel;
        LevelManager.endLevelEvent -= EndLevelUIUpdate;
        LevelManager.retryLevelEvent -= UpScrollPanel;
    }

}

