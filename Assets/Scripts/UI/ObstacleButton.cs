using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObstacleButton : MonoBehaviour {

    private string obstacleName;
    private int obstacleAmount;

    public Image obstacleImage;
    public TextMeshProUGUI obstacleAmountText;

    public void AssignObstacleTypeAndAmount(string type, int amount) {
        this.obstacleName = type;
        this.obstacleAmount = amount;
        AssignUIValues();
    }

    void AssignUIValues() {
        obstacleImage.sprite = Resources.Load<Sprite>("UIObstaclesImages/" + obstacleName);
        Debug.Log("UIObstaclesImages/" + obstacleName);
        obstacleAmountText.text = "x " + obstacleAmount;
    }
}
