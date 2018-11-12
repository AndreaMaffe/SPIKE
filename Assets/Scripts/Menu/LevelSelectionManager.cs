using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectionManager : MonoBehaviour {

    [Header("Inserire il prefab del levelButton")]
    public GameObject levelButton;
    [Header("Inserire il parent del levelButton")]
    public GameObject panel;
    [Header("Distanza hor/ver tra i bottoni")]
    public float horizontalSpacing;
    public float verticalSpacing;

    private SaveManager saveManager;

    public void Start()
    {
        //Carichiamo l'oggetto SaveManager per ottenere i dati salvati del gioco
        saveManager = SaveManager.SaveManagerInstance;
        saveManager = SaveUtility.LoadObject(saveManager, "saveFile");

        //Creiamo un bottone per ogni livello creato, adesso il ciclo for è puramente inventato dato che non abbiamo altri livelli oltre il primo
        int x = 0;
        int y = 0;
        for (int i = 0; i < saveManager.totalLevels; i++)
        {
            if(x >= 5)
            {
                x = 0;
                y += 1;
            }

            GameObject newButton;
            newButton = Instantiate(Resources.Load<GameObject>("Prefab/UI/LevelButton"));
            newButton.GetComponentInChildren<Text>().text = (i + 1).ToString();

            //Entra nell'if solo se il livello selezionato è sbloccato
            if (i < saveManager.maxUnlockedLevel)
            {
                newButton.transform.GetChild(1).gameObject.SetActive(false);
                newButton.GetComponent<Button>().onClick.AddListener(ChooseLevel);
            }

            newButton.transform.SetParent(panel.transform, false);
            newButton.transform.localPosition += new Vector3(horizontalSpacing*x, -verticalSpacing*y, 0);
            
            x++;
        }
    }

    //Metodo da rivedere una volta che iniziamo ad avere qualche livello definitivo
    private void ChooseLevel()
    {
        int levelIndex;
        levelIndex = int.Parse(levelButton.GetComponentInChildren<Text>().text);
        saveManager.currentLevel = levelIndex;
        SaveUtility.SaveObject(saveManager, "saveFile");
        if (levelIndex <= saveManager.maxUnlockedLevel)
        {
            SceneManager.LoadScene("SampleSceneRange");
            //Non appena la scena viene caricata dobbiamo riprendere da SaveManager l'informazione del livello scelto (currentLevel) 
            //in modo tale da caricare lo scriptable object relativo
        }
    }

}
