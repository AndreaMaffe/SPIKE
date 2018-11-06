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

    //Variabili di prova
    private float totalLevels = 11;
    private float maxUnlockedLevel = 1;

    public void Start()
    {
        //Creiamo un bottone per ogni livello creato, adesso il ciclo for è puramente inventato dato che non abbiamo altri livelli oltre il primo
        int x = 0;
        int y = 0;
        for (int i = 0; i < 18; i++)
        {
            if(x >= 5)
            {
                x = 0;
                y += 1;
            }

            GameObject newButton;
            if (i < totalLevels)
            {
                newButton = Instantiate(Resources.Load<GameObject>("Prefab/UI/LevelButton"));
                newButton.GetComponentInChildren<Text>().text = (i + 1).ToString();

                if (i < maxUnlockedLevel)
                    newButton.transform.GetChild(1).gameObject.SetActive(false);

            }
            else {
                newButton = Instantiate(Resources.Load<GameObject>("Prefab/UI/WIPfakeButton"));
            }

            newButton.transform.SetParent(panel.transform, false);
            newButton.transform.localPosition += new Vector3(horizontalSpacing*x, -verticalSpacing*y, 0);
            
            x++;
        }
    }

    public void ChooseLevel()
    {
        int levelIndex;
        levelIndex = int.Parse(levelButton.GetComponentInChildren<Text>().text);
        //TODO bisogna tener conto del livello scelto in modo tale da caricare lo scriptable object corretto
        if (levelIndex < maxUnlockedLevel)
        {
            SceneManager.LoadScene("SampleScene");
        }
    }

}
