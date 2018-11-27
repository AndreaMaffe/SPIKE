using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

    private SaveManager saveManager;

    private void Awake()
    {
        //Andiamo ad inizializzare la classe SaveManager nel caso non esistesse ancora

        //PlayerPrefs.DeleteKey("firstLaunch");  //Decommentare questa riga di codice in fase di testing per riazzerare la PlayerPref che ci serve
        
        //Qui controlliamo se è il primo lancio del gioco oppure no, nel caso lo fosse andiamo a settare il nostro file di salvataggio
        if (!PlayerPrefs.HasKey("firstLaunch"))
        {
            saveManager = SaveManager.SaveManagerInstance;
            saveManager.maxUnlockedLevel = 1;
            saveManager.totalLevels = 20; //TODO vedere come fargli prendere il reale numero massimo dei livelli, facile ma poco elegante reinserendo di nuovo una lista pubblica degli scriptables
            SaveUtility.SaveObject(saveManager, "saveFile");
            PlayerPrefs.SetInt("firstLaunch", 0);
        }
        

    }

    //Metodo chiamato dopo aver premuto il tasto Play
    public void StartGame()
    {
        //Cambiare il LoadScene con il nome definitivo della scena che continene il livello di gioco
        SceneManager.LoadScene("SampleSceneRange");
    }

    //Metodo chiamato dopo aver premuto il tasto Levels
    public void LevelSelectionButtonPressed()
    {
        SceneManager.LoadScene("LevelSelection");
    }
}
