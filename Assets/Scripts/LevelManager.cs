using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class LevelManager : MonoBehaviour
{
    //Tutti gli scriptable objects dei livelli
    public Level[] levels;
    //numero del livello attuale partendo da 1
    private int indexLevel;
    private Level currentLevel;

    private bool levelStarted { get; set; } //true se il giocatore ha premuto il tasto play

    //[SerializeField]
    //public ObstacleData[] obstacleData;

    public GameObject obstacleButton;
    public int buttonInBetweenSpace;
    public int buttonWidth;
    public Transform buttonPanel;

    public delegate void OnStartLevel();
    public static event OnStartLevel triggeredEvent;

    private void Start()
    {
        Application.targetFrameRate = 60;
        //TODO: adesso allo start viene messo direttamente il primo livello ma in futuro và cambiato nel caso in cui viene selezionato subito un livello successivo dal menù
        indexLevel = 1;
        LoadLevel();
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
        //i livelli partono da 1
        //return levels[indexLevel - 1];
        return levels[0]; //!!!!!Roba HARDCODED, devo trovare una soluzione per rendere visibile currentLevel (inizializzato) anche agli altri script
    }

    public void StartLevel()
    {
        triggeredEvent();
    }

    public void RestartLevel()
    {
        SceneManager.LoadSceneAsync("SampleScene");
    }

    //TODO: questo metodo andra' messo nello UIManager
    /*
    void CreateUIObstacleButtons()
    {

        for (int i = 0; i < testObstacle.Length; i++)
        {

            Vector2 buttonLocalPosition;
            //A seconda se i buttoni sono pari o dispari li posiziona nel modo giusto
            if (testObstacle.Length % 2 == 1)
                buttonLocalPosition = new Vector2(-(buttonWidth + buttonInBetweenSpace) * (testObstacle.Length / 2) + i * (buttonWidth + buttonInBetweenSpace), 0);
            else
                buttonLocalPosition = new Vector2(-((buttonWidth + buttonInBetweenSpace) / 2 + (testObstacle.Length / 2 - 1) * (buttonWidth + buttonInBetweenSpace)) + i * (buttonWidth + buttonInBetweenSpace), 0);

            //crea il bottone come figlio del panel giusto
            GameObject button = Instantiate(obstacleButton, buttonPanel, false);
            //posiziona il bottone
            button.GetComponent<RectTransform>().localPosition = buttonLocalPosition;
            button.GetComponent<ObstacleButton>().AssignObstacleTypeAndAmount(testObstacle[i].obstacleName, testObstacle[i].obstacleMaxAmount);
        }
    }
    */

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
            button.GetComponent<ObstacleButton>().AssignObstacleTypeAndAmount(obstacleData[i].type.ToString(), obstacleData[i].obstacleMaxAmount);
        }
    }
}

