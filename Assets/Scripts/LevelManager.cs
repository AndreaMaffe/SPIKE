using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    //array che contiene gli scruptable object di tutti i livelli
    Level[] allLevels;
    //numero del livello attuale partendo da 1
    private int currentLevel;

    [SerializeField]
    public ObstacleData[] testObstacle;

    public GameObject obstacleButton;
    public int buttonInBetweenSpace;
    public int buttonWidth;
    public Transform buttonPanel; 

    private void Start()
    {
        LoadLevel();
    }

    void LoadLevel() {
        CreateUIObstacleButtons();
    }

    public Level GetActualLevel() {
        //i livelli partono da 1
        return allLevels[currentLevel - 1];
    }

    void CreateUIObstacleButtons() {

        for (int i = 0; i < testObstacle.Length; i++) {

            Vector2 buttonLocalPosition;
            //A seconda se i buttoni sono pari o dispari li posiziona nel modo giusto
            if (testObstacle.Length % 2 == 1)
                buttonLocalPosition = new Vector2(-(buttonWidth + buttonInBetweenSpace) * (testObstacle.Length / 2)+ i * (buttonWidth + buttonInBetweenSpace), 0);
            else
                buttonLocalPosition = new Vector2(-((buttonWidth + buttonInBetweenSpace)/2 + (testObstacle.Length / 2 - 1) * (buttonWidth + buttonInBetweenSpace)) + i * (buttonWidth + buttonInBetweenSpace), 0);

            //crea il bottone come figlio del panel giusto
            GameObject button = Instantiate(obstacleButton, buttonPanel, false);
            //posiziona il bottone
            button.GetComponent<RectTransform>().localPosition = buttonLocalPosition;
            button.GetComponent<ObstacleButton>().AssignObstacleTypeAndAmount(testObstacle[i].obstacleName, testObstacle[i].obstacleMaxAmount);
        }
    }
}

[System.Serializable]
public struct ObstacleData {
    public string obstacleName;
    public int obstacleMaxAmount;
}