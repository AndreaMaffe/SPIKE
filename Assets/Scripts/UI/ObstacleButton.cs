using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ObstacleButton : DraggableObject
{
    [SerializeField]
    private int obstacleAmount;

    public Image obstacleImage;
    public TextMeshProUGUI obstacleAmountText;

    //metodo invocato dal level manager quando crea il bottone per dirgli che tipo di ostacolo sta tenendo
    public void AssignObstacleTypeAndAmount(ObstacleType type, int amount)
    {
        obstacleType = type;
        obstacleAmount = amount;
        AssignUIValues();
        //AnchorPoint.OnReaddObstacle += AddToSpecificObstacleAmount;
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
            SpawnObstacleInstance();
            //SpawnObstacleDraggerPrefab();
            HideObstacleButtons();
        }
    }
    public override void OnEndDrag(PointerEventData eventData)
    {
        //se non controllo se e' stato spawnato mi da' problemi dopo di null reference
        if (obstacleDragged != null)
        {
            //check se se snappato ad un anchor point range
            if (obstacleDragged.CheckIfSnapped())
            {
                UpdateObstacleNumber(-1);
                obstacleDragged.GetComponent<Obstacle>().ActivateObstacle();
                AddObstaclePositionedComponents();
            }
            //altrimenti distruggi il prefab e basta
            else
            {
                Destroy(obstacleDragged.gameObject);
                obstacleDragged = null;
                //non hai usato l'ostacolo quindi rimettilo a posto
                UpdateObstacleNumber(+1);
            }
            ShowObstacleButtons();
        }
    }

    void SpawnObstacleInstance() {
        GameObject obstacleInstance = Instantiate(Resources.Load<GameObject>("Prefab/Obstacles/" + obstacleType.ToString()), transform.position, Quaternion.identity);
        obstacleDragged = obstacleInstance.AddComponent<DraggableObjectPositionUpdater>();
    }

    void SpawnObstacleDraggerPrefab() {
        GameObject obstacleInstance = Instantiate(draggableObjectPrefab, transform.position, Quaternion.identity);

    }

    protected override void UpdateObstacleNumber(int amount) {
        if (amount < 0 )
            AddToObstacleAmount(amount);
    }

}
