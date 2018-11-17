using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class LevelManager : MonoBehaviour
{

    private enum LevelState { RUNNING, UNDER_CONSTRUCTION };

    private Level currentLevel;
    private LevelState state;
    private SaveManager saveManager;

    //Tutti gli scriptable objects dei livelli
    public Level[] levels;

    //[SerializeField]
    //public ObstacleData[] obstacleData;
    public GameObject obstacleButton;
    public int buttonInBetweenSpace;
    public int buttonWidth;
    public Transform buttonPanel;

    public GameObject nextLevelPanel;

    public delegate void OnRunLevel();
    public static event OnRunLevel runLevelEvent;

    public delegate void OnRetryLevel();
    public static event OnRetryLevel retryLevelEvent;

    public delegate void OnEndLevel();
    public static event OnEndLevel endLevelEvent;

    public Button playPauseButton;


    //struct che associa ogni ostacolo alla posizione in cui puo' andare
    [System.Serializable]
    public struct ObstaclePositionData
    {
        public ObstacleType obstacle;
        public AnchorPointPosition position;
    }

    //array di tutte le associazioni ostacolo-posizione. Serve una struttura dati del genere perche' altrimenti non sapresti come dire 
    //al draggable obstacle in quale posizione puo' andare e in quale no
    public ObstaclePositionData[] obstaclePositionData;

    private void Start()
    {
        Application.targetFrameRate = 60;

        //Carichiamo l'oggetto SaveManager per ottenere i dati salvati del gioco
        saveManager = SaveManager.SaveManagerInstance;
        saveManager = SaveUtility.LoadObject(saveManager, "saveFile");
        PrintSaveManager();

        LoadLevel();
        state = LevelState.UNDER_CONSTRUCTION;

    }

    void LoadLevel()
    {
        currentLevel = GetActualLevel();
        CreatePlatforms(currentLevel);
        CreateUIObstacleButtons(currentLevel);
    }

    //Metodo che posiziona le piattaforme del livello a partire dallo scriptable object
    private void CreatePlatforms(Level currentLevel)
    {
        foreach (PlatformData platform in currentLevel.platformDatas)
        {
            Instantiate(platform.platformType, platform.platformPos, Quaternion.identity);
        }
    }

    public Level GetActualLevel()
    {
        return levels[saveManager.currentLevel - 1];  //QUESTA E' LA LINEA DI CODICE CORRETTA, NON CANCELLARE
        //return levels[0]; //Questa è la linea di codice più comoda adesso in fase di testing
    }

    public void ChangeLevelState()
    {
        if (state == LevelState.UNDER_CONSTRUCTION)
        {
            state = LevelState.RUNNING;

            playPauseButton.image.overrideSprite = Resources.Load<Sprite>("UISprites/PauseButton");

            //attiva tutti gli eventi associati al RunLevel() (come i WakeUp() degli ostacoli)
            runLevelEvent();
        }

        else if (state == LevelState.RUNNING)
        {
            state = LevelState.UNDER_CONSTRUCTION;

            playPauseButton.image.overrideSprite = null;

            //attiva tutti gli eventi associati al RetryLevel() (come gli Sleep() degli ostacoli)
            retryLevelEvent();
        }

    }

    public void ReloadLevel() {

        //FindObjectOfType<TimerManager>().Clear();
        SceneManager.LoadSceneAsync("SampleSceneRange");
        //state = LevelState.UNDER_CONSTRUCTION;
    }

    //Metodo che lancia l'evento legato alla fine del livello
    public static void EndLevel()
    {
        endLevelEvent();
    }

    //metodo che serve ai bottoni per associare a ogni ostacolo la posizione in cui deve andare in modo da passare al 
    //draggable obstacle la posizione giusta dell'ancor point valido a cui agganciarsi
    public AnchorPointPosition CheckInWhatPositionTheObstacleGoes(ObstacleType obstacleName)
    {
        foreach (ObstaclePositionData obstacle in obstaclePositionData)
        {
            if (obstacleName == obstacle.obstacle)
                return obstacle.position;
        }
        return AnchorPointPosition.Platform;
    }

    //TODO: questo metodo andra' messo nello UIManager
    void CreateUIObstacleButtons(Level currentLevel)
    {
        ObstacleData[] obstacleData = currentLevel.obstacleDatas;
        for (int i = 0; i < obstacleData.Length; i++)
        {

            Vector2 buttonLocalPosition;
            //A seconda se i buttoni sono pari o dispari li posiziona nel modo giusto
            if (obstacleData.Length % 2 == 1)
                buttonLocalPosition = new Vector2(-(buttonWidth + buttonInBetweenSpace) * (obstacleData.Length / 2) + i * (buttonWidth + buttonInBetweenSpace), 0);
            else
                buttonLocalPosition = new Vector2(-((buttonWidth + buttonInBetweenSpace) / 2 + (obstacleData.Length / 2 - 1) * (buttonWidth + buttonInBetweenSpace)) + i * (buttonWidth + buttonInBetweenSpace), 0);

            //crea il bottone come figlio del panel giusto
            GameObject button = Instantiate(obstacleButton, buttonPanel, false);
            //posiziona il bottone
            button.GetComponent<RectTransform>().localPosition = buttonLocalPosition;
            button.GetComponent<ObstacleButton>().AssignObstacleTypeAndAmount(obstacleData[i].type, obstacleData[i].obstacleMaxAmount);
        }
    }

    private void PrintSaveManager()
    {
        Debug.Log("|CurrenteLevel: " + saveManager.currentLevel + "| |MaxUnlockedLevel: " + saveManager.maxUnlockedLevel + "| |TotalLevels: " + saveManager.totalLevels + "|");
    }
}

