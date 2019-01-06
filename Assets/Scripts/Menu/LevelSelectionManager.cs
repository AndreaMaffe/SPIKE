using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectionManager : MonoBehaviour {


    public Text numberIfDeathsText;
    [Header("Inserire il prefab del levelButton")]
    public GameObject levelButton;
    [Header("Inserire il parent del levelButton")]
    public GameObject panel;
    [Header("Distanza hor/ver tra i bottoni")]
    public float horizontalSpacing;
    public float verticalSpacing;
    [Header("Scegliere il numero massimo di livelli su una pagina")]
    public int levelAmountTest;

    private SaveManager saveManager;

    private GameObject[] allButtons;
    private int currentLevelPage = 0;
    private int maxLevelNumber = 1000; 

    public void Start()
    {
        numberIfDeathsText.text = LevelManager.TotalNumberOfDeaths.ToString();
        allButtons = new GameObject[levelAmountTest];
        //Carichiamo l'oggetto SaveManager per ottenere i dati salvati del gioco
        saveManager = SaveManager.SaveManagerInstance;
        saveManager = SaveUtility.LoadObject(saveManager, "saveFile");

        CreateLevelButtons();  
    }

    private void CreateLevelButtons() {
        //Creiamo un bottone per ogni livello creato, adesso il ciclo for è puramente inventato dato che non abbiamo altri livelli oltre il primo
        int x = 0;
        int y = 0;
        //for (int i = 0; i < saveManager.totalLevels; i++)
        for (int i = 0; i < levelAmountTest; i++)
        {
            if (x >= 5)
            {
                x = 0;
                y += 1;
            }

            GameObject newButton;
            newButton = Instantiate(Resources.Load<GameObject>("Prefab/UI/LevelButton"));
            allButtons[y * 5 + x] = newButton;
            newButton.GetComponentInChildren<Text>().text = (i + 1).ToString();

            //Entra nell'if solo se il livello selezionato è sbloccato
            //if (i < saveManager.maxUnlockedLevel) LINEA DI CODICE CORRETTA, USIAMO LA PROSSIMA IN FASE DI TESTING
            if (i < levelAmountTest)
            {
                newButton.transform.GetChild(1).gameObject.SetActive(false);
                newButton.GetComponent<Button>().onClick.AddListener(ChooseLevel);
                newButton.GetComponent<LevelButton>().AssignStars(SaveManager.SaveManagerInstance.stars[i]);
            }

            newButton.transform.SetParent(panel.transform, false);
            newButton.transform.localPosition += new Vector3(horizontalSpacing * x, -verticalSpacing * y, 0);

            x++;
        }
    }

    //riassegna i valori dei bottoni a seconda della pagina di livelli a cui sei (ogni pagina ha 15 livelli)
    private void ReassignButtons(int pageIndex)
    {
        for (int i = 0; i < allButtons.Length; i++) {
            allButtons[i].GetComponentInChildren<Text>().text = (pageIndex * levelAmountTest + i + 1).ToString();
        }
    }

    public void ChangePageIndex(int amount) 
    {
        currentLevelPage += amount;
        currentLevelPage = Mathf.Clamp(currentLevelPage, 0, maxLevelNumber);
        ReassignButtons(currentLevelPage);   
    }

    //Metodo da rivedere una volta che iniziamo ad avere qualche livello definitivo
    private void ChooseLevel()
    {
        int levelIndex = int.Parse(EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text);
        LevelManager.CurrentLevelIndex = levelIndex -1;
        //if (levelIndex <= saveManager.maxUnlockedLevel) LINEA DI CODICE CORRETTA, USIAMO LA PROSSIMA IN FASE DI TESTING   

        SceneManager.LoadScene("SampleSceneRange");    
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }

}
