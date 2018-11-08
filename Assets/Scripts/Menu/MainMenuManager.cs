using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

    private SaveManager saveManager;

    private void Awake()
    {
        //Andiamo ad inizializzare la classe SaveManager nel caso non esistesse ancora

        //PlayerPrefs.DeleteKey("firstLaunch");  decommentare questa riga di codice in fase di testing per riazzerare la PlayerPref che ci serve
        saveManager = SaveManager.SaveManagerInstance;
        //Qui controlliamo se è il primo lancio del gioco oppure no, nel caso lo fosse andiamo a settare il nostro file di salvataggio
        if (!PlayerPrefs.HasKey("firstLaunch"))
        {
            Debug.Log("Sono dentro l'if");
            saveManager.currentLevel = 1;
            saveManager.maxUnlockedLevel = 1;
            saveManager.totalLevels = 20; //TODO vedere come fargli prendere il reale numero massimo dei livelli, facile ma poco elegante reinserendo di nuovo una lista pubblica degli scriptables
            SaveUtility.SaveObject(saveManager, "saveFile");
            Debug.Log("Il path del saveFile è " + Application.persistentDataPath);
            PlayerPrefs.SetInt("firstLaunch", 0);
        }
        

    }

    //Metodo chiamato dopo aver premuto il tasto Play
    public void StartGame()
    {
        //Cambiare il LoadScene con il nome definitivo della scena che continene il livello di gioco
        SceneManager.LoadScene("SampleScene");
    }

    //Metodo chiamato dopo aver premuto il tasto Levels
    public void LevelSelectionButtonPressed()
    {
        SceneManager.LoadScene("LevelSelection");
    }
}
