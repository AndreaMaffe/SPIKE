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
    public static int NumberOfDeaths { get; set; }
    public static int TotalNumberOfDeaths { get; set; }

    private List<Level> levels;
    private LevelState state;

    public GameObject obstacleButton;
    public int buttonInBetweenSpace;
    public int buttonWidth;
    public Transform buttonPanel;

    public string[] hints;

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

    public GameObject playerTutorial;
    public GameObject tutorialPanel;
    public GameObject blackScreen;
    public GameObject deathPanel;

    public GameObject[] stars;

    private GameObject playerSimulatorObject;

    private SaveManager saveManager;

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

        PlayerSimulator.playerSimulatorFlagReached += ActivateChangeLevelStateButton;
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
        currentLevelText.text = "LV. " + (CurrentLevelIndex + 1);

        if (CurrentLevel == null)
            Debug.Log("Il livello corrente è null nel LevelManager");

        SetUpLevel();

        state = LevelState.UNDER_CONSTRUCTION;

        currentTime = 0.0f;

        saveManager = SaveManager.SaveManagerInstance;
        saveManager = SaveUtility.LoadObject(saveManager, "saveFile");
        TotalNumberOfDeaths = saveManager.totalDeathsCounter;
    }

    private void FixedUpdate()
    {
        if (state == LevelState.RUNNING)
            currentTime += Time.deltaTime;
        currentTimeText.text = currentTime.ToString();
    }

    void SetUpLevel()
    {
        //crea le piattaforme
        try
        {
            foreach (PlatformData platform in CurrentLevel.platformDatas)
                Instantiate(platform.platformType, platform.platformPos, Quaternion.identity);
        }
        catch (NullReferenceException e) { }

        //crea pulsanti, player e flag
        CreateUIObstacleButtons(CurrentLevel);
        Instantiate(Resources.Load<GameObject>("Prefab/Player"), CurrentLevel.startingPoint, Quaternion.identity);
        Instantiate(Resources.Load<GameObject>("Prefab/Flag"), CurrentLevel.endingPoint, Quaternion.identity);

        //crea il PlayerSimulator
        playerSimulatorObject = Instantiate(Resources.Load<GameObject>("Prefab/PlayerSimulator"), CurrentLevel.startingPoint, Quaternion.identity);

        if (CurrentLevel.isTutorial)
        {
            playerTutorial.SetActive(true);
            tutorialPanel.SetActive(true);
            blackScreen.SetActive(true);

            tutorialPanel.transform.Find("Title").GetComponent<Text>().text = CurrentLevel.tutorialTitle;
            tutorialPanel.transform.Find("Image").GetComponent<Image>().sprite = CurrentLevel.tutorialImage;
            tutorialPanel.transform.Find("Image").GetComponent<Image>().SetNativeSize();
            tutorialPanel.transform.Find("Text").Find("Line1").GetComponent<Text>().text = CurrentLevel.tutorialText[0];
            tutorialPanel.transform.Find("Text").Find("Line2").GetComponent<Text>().text = CurrentLevel.tutorialText[1];
            tutorialPanel.transform.Find("Text").Find("Line3").GetComponent<Text>().text = CurrentLevel.tutorialText[2];
        }

        currentTime = 0.0f;
    }

    //invoca le animazioni per cui il pannello sale e il player scende
    public void HideTutorial()
    {
        playerTutorial.GetComponent<Animator>().SetTrigger("Down");
        tutorialPanel.GetComponent<Animator>().SetTrigger("Up");
        blackScreen.GetComponent<Animator>().SetTrigger("Fade");

        Invoke("DeactivateTutorialElements", 2f);
    }

    //disattiva il player tutorial e il pannello
    private void DeactivateTutorialElements()
    {
        blackScreen.SetActive(false);
        playerTutorial.SetActive(false);
        tutorialPanel.SetActive(false);
    }

    public void GoToLevelSelection()
    {
        SceneManager.LoadSceneAsync("LevelSelection");
        NumberOfDeaths = 0;
    }

    void ActivateChangeLevelStateButton()
    {
        playPauseButton.gameObject.SetActive(true);
    }

    public void ChangeLevelState()
    {
        if (state == LevelState.UNDER_CONSTRUCTION)
        {
            state = LevelState.RUNNING;

            playPauseButton.image.overrideSprite = Resources.Load<Sprite>("UISprites/PauseButton");
            //disattiva la trail del player simulator
            playerSimulatorObject.SetActive(false);
            //attiva tutti gli eventi associati al RunLevel() (come i WakeUp() degli ostacoli)
            runLevelEvent();
        }

        else if (state == LevelState.RUNNING)
        {
            state = LevelState.UNDER_CONSTRUCTION;

            //attiva il player simulator
            playerSimulatorObject.SetActive(true);
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

    public void LoadNextLevel()
    {
        CurrentLevelIndex += 1;
        SceneManager.LoadScene("SampleSceneRange");
        NumberOfDeaths = 0;
    }

    //Metodo che lancia l'evento legato alla fine del livello
    public static void EndLevel()
    {
        FindObjectOfType<AudioManager>().GetComponent<AudioManager>().PlayWinAudio();
        GameObject.Find("NumberOfAttemptsText").GetComponent<Text>().text = "NUMBER OF DEATHS: " + LevelManager.NumberOfDeaths;

        endLevelEvent();
        SaveStarsNumber();
    }

    private static void SaveStarsNumber()
    {
        SaveManager saveManager = SaveManager.SaveManagerInstance;
        saveManager = SaveUtility.LoadObject<SaveManager>(saveManager, "saveFile");

        if (saveManager.stars[CurrentLevelIndex] < GetNumberOfStars())
        {
            saveManager.stars[CurrentLevelIndex] = GetNumberOfStars();
            SaveUtility.SaveObject(saveManager, "saveFile");
        }
    }

    public static void StartFailureEvent(string message)
    {
        
        FindObjectOfType<AudioManager>().GetComponent<AudioManager>().PlayFailAudio();
        FindObjectOfType<LevelManager>().GetComponent<LevelManager>().ShowDeathPanel();
        FindObjectOfType<LevelManager>().GetComponent<LevelManager>().deathPanel.transform.Find("Title").GetComponent<Text>().text = message;
        playerDeathEvent();       
    }


    public void ShowDeathPanel()
    {
        blackScreen.SetActive(true);
        deathPanel.SetActive(true);
        deathPanel.transform.Find("Hint").GetComponent<Text>().text = "HINT:  " + hints[new System.Random().Next(0, hints.Length - 1)];
    }

    //metodo invocato dal bottone del pannello death per reiniziare il building del livello
    public void DeathPanelRestartLevel()
    {
        deathPanel.GetComponent<Animator>().SetTrigger("Up");
        blackScreen.GetComponent<Animator>().SetTrigger("Fade");

        Invoke("DeactivateDeathPanel", 1f);
        ChangeLevelState();
    }

    private void DeactivateDeathPanel()
    {
        deathPanel.SetActive(false);
        blackScreen.SetActive(false);
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

    public void UpdateStars()
    {
        int numberOfStars = GetNumberOfStars();

        foreach (GameObject star in stars)
        {
            if (Array.IndexOf(stars, star) + 1 <= numberOfStars)
                star.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            else star.gameObject.GetComponent<SpriteRenderer>().color = new Color(0.1981132f, 0.131764f, 0.131764f);
        }
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

    public static int GetNumberOfStars()
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

    private void OnDisable()
    {
        PlayerSimulator.playerSimulatorFlagReached -= ActivateChangeLevelStateButton;
    }
}

