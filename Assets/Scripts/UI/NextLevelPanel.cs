using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelPanel : MonoBehaviour
{

    private Animator animator;
    private SaveManager saveManager;

    void Start()
    {
        animator = GetComponent<Animator>();

        //Se il livello è completato facciamo scendere il pannello della vittoria
        LevelManager.endLevelEvent += DownScrollPanel;
        //Se il livello è completato aggiorniamo il massimo livello sbloccato
        LevelManager.endLevelEvent += UpdateMaxUnlockedLevel;
    }

    //Metodo che viene chiamato nel momento in cui il livello è stato vinto e che fa il pannello della vittoria
    public void DownScrollPanel()
    {
        animator.SetTrigger("DownScrollPanel");
    }

    //Metodo legato al bottone next level, aggiorna il livello corrente in modo tale che verrà caricato lo scriptable object corretto nella prossima scena
    public void NextLevel()
    {
        saveManager = SaveManager.SaveManagerInstance;
        saveManager = SaveUtility.LoadObject(saveManager, "saveFile");
        //Adesso bisogna settare currentLevel al livello successivo e caricare la scena
        saveManager.currentLevel += 1;
        SaveUtility.SaveObject(saveManager, "saveFile");
        SceneManager.LoadScene("SampleSceneRangeAle2");
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("SampleRangeAle2");
    }

    //Metodo che della schermata di vittoria ci riporta al main menu
    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

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

    private void OnDestroy()
    {
        //Unregister events
        LevelManager.endLevelEvent -= DownScrollPanel;
        LevelManager.endLevelEvent -= UpdateMaxUnlockedLevel;
    }

}

