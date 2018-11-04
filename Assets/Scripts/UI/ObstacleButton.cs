using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ObstacleButton : ObstacleDragger, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    private int obstacleAmount;

    public Image obstacleImage;
    public TextMeshProUGUI obstacleAmountText;

    //metodo invocato dal level manager quando crea il bottone per dirgli che tipo di ostacolo sta tenendo
    public void AssignObstacleTypeAndAmount(ObstacleType type, int amount)
    {
        obstacleType = type;
        obstacleAmount = amount;
        AssignUIValues();
        AnchorPoint.OnReaddObstacle += AddToSpecificObstacleAmount;
    }

    //metodo per decrementare il numero di ostacoli usati sul bottone
    void AddToObstacleAmount(int amount)
    {
        obstacleAmount += amount;
        AssignUIValues();
    }

    void AddToSpecificObstacleAmount(ObstacleType type, int amount) {
        if (obstacleType == type) {
            obstacleAmount += amount;
            AssignUIValues();
        }       
    }

    //aggiorna i valori dei bottoni nella GUI
    void AssignUIValues()
    {
        obstacleImage.sprite = Resources.Load<Sprite>("UIObstacleImages/" + obstacleType.ToString());
        obstacleImage.SetNativeSize();
        obstacleAmountText.text = obstacleAmount.ToString();
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        //se ho abbastanza ostacoli da piazzare
        if (obstacleAmount > 0)
        {
            SpawnDraggableObjectInstanceAndGetComponents(eventData);
            AssignPositionWhereDraggableObstacleCanGo();
            HideObstacleButtons();
        }
    }

    protected override void UpdateObstacleNumber(int amount) {
        if (amount < 0 )
            AddToObstacleAmount(amount);
    }

}
