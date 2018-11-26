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

    private void Start()
    {
        DraggableObjectPositioned.onReaddObstacle += AddToSpecificObstacleAmount;
    }

    //public Image obstacleImage;
    public TextMeshProUGUI obstacleAmountText;

    //metodo invocato dal level manager quando crea il bottone per dirgli che tipo di ostacolo sta tenendo
    public void AssignObstacleTypeAndAmount(ObstacleType type, int amount)
    {
        obstacleType = type;
        obstacleAmount = amount;
        AssignUIValues();
    }

    //metodo per decrementare il numero di ostacoli usati sul bottone
    void AddToObstacleAmount(int amount)
    {
        obstacleAmount += amount; 
        AssignUIValues();
    }

    //se droppi un ostacolo gia' posizionato deve restituirti un ostacolo a disposizione
    void AddToSpecificObstacleAmount(ObstacleType type) {
        if (obstacleType == type) {
            obstacleAmount += 1;
            AssignUIValues();
        }       
    }

    //aggiorna i valori dei bottoni nella GUI
    void AssignUIValues()
    {
        GetComponent<Image>().sprite = Resources.Load<Sprite>("UIButtonImages/Bottone" + obstacleType.ToString());
        GetComponent<Image>().SetNativeSize();
        obstacleAmountText.text = obstacleAmount.ToString();
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        //se ho abbastanza ostacoli da piazzare
        if (obstacleAmount > 0)
        {
            SpawnObstacleInstance();
            HideObstacleButtons();
        }
    }
    public override void OnEndDrag(PointerEventData eventData)
    {
        //se non controllo se e' stato spawnato mi da' problemi dopo di null reference
        if (obstacleDragged != null)
        {
            //check se se snappato ad un anchor point range e non sei sopra un altro ostacolo
            if (obstacleDragged.CheckIfSnapped() && !obstacleDragged.CheckIfOverAnotherObstacle())
            {
                UpdateObstacleNumber(-1);
                obstacleDragged.GetComponent<Obstacle>().OnObstacleDropped();
                AddObstaclePositionedComponents();
                obstacleDragged = null;
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

    protected override void UpdateObstacleNumber(int amount) {
        if (amount < 0 )
            AddToObstacleAmount(amount);
    }

    private void OnDisable()
    {
        DraggableObjectPositioned.onReaddObstacle -= AddToSpecificObstacleAmount;

    }

}
