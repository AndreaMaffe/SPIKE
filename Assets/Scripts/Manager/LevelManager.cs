using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class LevelManager : MonoBehaviour
{
    private enum LevelState { RUNNING, UNDER_CONSTRUCTION };

    public static int CurrentLevelIndex { get; set; }
    public static Level CurrentLevel { get; set; }
    public static Dictionary<ObstacleType, int> NumberOfObstacles { get; set; }

    private List<Level> levels;
    private LevelState state;

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

    public delegate void OnPlayerDeath();
    public static event OnPlayerDeath playerDeathEvent;

    public Button playPauseButton;

    public Text currentLevelText;
    public Text currentTimeText;
    private float currentTime;

    public bool playerSimulator;


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

    void Start()
    {
        Application.targetFrameRate = 60;

        levels = new List<Level>();

        //inizializza il Dictionary degli ostacoli a zero
        LevelManager.NumberOfObstacles = new Dictionary<ObstacleType, int>();
        foreach (ObstacleType type in System.Enum.GetValues(typeof(ObstacleType)))
            NumberOfObstacles.Add(type, 0);

        try
        {
            for (int i = 1; i < 100; i++)
            {
                String levelPath = "Levels/Level" + i.ToString();
                levels.Add(Resources.Load<Level>(levelPath));
            }
        }
        catch (NullReferenceException e) { }
     

        CurrentLevel = levels[CurrentLevelIndex];
        currentLevelText.text = "LV. " + (CurrentLevelIndex+1);

        if (CurrentLevel == null)
            Debug.Log("Il livello corrente è null nel LevelManager");
       
        LoadLevel(CurrentLevel);

        state = LevelState.UNDER_CONSTRUCTION;

        currentTime = 0.0f;
    }

    private void FixedUpdate()
    {
        if (state == LevelState.RUNNING)
        currentTime += Time.deltaTime;
        currentTimeText.text = currentTime.ToString();
    }

    void LoadLevel(Level level)
    {
        CreatePlatforms(level);
        CreateUIObstacleButtons(level);
        Instantiate(Resources.Load<GameObject>("Prefab/Player"), level.startingPoint, Quaternion.identity);
        if (playerSimulator) 
            Instantiate(Resources.Load<GameObject>("Prefab/PlayerSimulator"), level.startingPoint, Quaternion.identity);
        Instantiate(Resources.Load<GameObject>("Prefab/Flag"), level.endingPoint, Quaternion.identity);
        currentTime = 0.0f;
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadSceneAsync("LevelSelection");
    }

    //Metodo che posiziona le piattaforme del livello a partire dallo scriptable object
    private void CreatePlatforms(Level currentLevel)
    {
        foreach (PlatformData platform in currentLevel.platformDatas)
        {
            Instantiate(platform.platformType, platform.platformPos, Quaternion.identity);
        }
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
            currentTime = 0.0f;

            //attiva tutti gli eventi associati al RetryLevel() (come gli Sleep() degli ostacoli)
            retryLevelEvent();
        }
    }

    public void ReloadLevel()
    {
        //FindObjectOfType<TimerManager>().Clear();
        SceneManager.LoadSceneAsync("SampleSceneRange");
        //state = LevelState.UNDER_CONSTRUCTION;
    }

    //Metodo che lancia l'evento legato alla fine del livello
    public static void EndLevel()
    {
        FindObjectOfType<AudioManagerBR>().GetComponent<AudioManager>().PlayWinAudio();

        endLevelEvent();

        GameObject.Find("CanvasFront").transform.Find("EndLevelPanelContent").Find("Text").GetComponent<Text>().text = "*** Well done! You get " + GetNumberOfStars() + " stars! ***";
    }

    public static void PlayerDeath()
    {
        FindObjectOfType<AudioManagerBR>().GetComponent<AudioManager>().PlayFailAudio();

        playerDeathEvent();
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

    private static int GetNumberOfStars()
    {
        int points = 0;

        foreach (ObstacleData obstacle in CurrentLevel.obstacleDatas)
            points += NumberOfObstacles[obstacle.type] * obstacle.points;

        if (points >= CurrentLevel.pointsForThreeStars)
            return 3;
        else if (points >= CurrentLevel.pointsForTwoStars)
            return 2;
        else if (points >= CurrentLevel.pointsForOneStar)
            return 1;
        else return 0;
    }
}

