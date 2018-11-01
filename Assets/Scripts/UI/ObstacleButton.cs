using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ObstacleButton : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler {

    private ObstacleType obstacleType;
    private int obstacleAmount;

    public Image obstacleImage;
    public TextMeshProUGUI obstacleAmountText;

    public LevelManager levelManager;

    //Variabili che tengono il prefab del draggable obstacle e vari parametri legati a questo prefab
    public GameObject draggableObstaclePrefab;
    GameObject draggableObstacleInstance;
    DraggableObstacle draggableObstacleComponent;
    public Vector3 draggableObstacleOffsetFromFinger;

    public delegate void UpdateAnchorPoint(AnchorPointPosition position);
    public static event UpdateAnchorPoint onUpdateAnchorPoint;

    //metodo invocato dal level manager quando crea il bottone per dirgli che tipo di ostacolo sta tenendo
    public void AssignObstacleTypeAndAmount(ObstacleType type, int amount) {
        levelManager = GameObject.FindObjectOfType<LevelManager>();
        this.obstacleType = type;
        this.obstacleAmount = amount;
        AssignUIValues();
    }

    //metodo per decrementare il numero di ostacoli usati sul bottone
    void AddToObstacleAmount(int amount) {
        obstacleAmount += amount;
        AssignUIValues();
    }

    //aggiorna i valori dei bottoni nella GUI
    void AssignUIValues()
    {
        obstacleImage.sprite = Resources.Load<Sprite>("UIObstacleImages/" + obstacleType.ToString());
        obstacleImage.SetNativeSize();
        obstacleAmountText.text = obstacleAmount.ToString();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //se ho abbastanza ostacoli da piazzare
        if (obstacleAmount > 0) {       
            SpawnDraggableObjectInstanceAndGetComponents(eventData);
            AssignPositionWhereDraggableObstacleCanGo();       
        }      
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //se non controllo se e' stato spawnato mi da' problemi dopo di null reference
        if (draggableObstacleInstance != null)
        {
            //check se se snappato ad un anchor point 
            if (draggableObstacleComponent.CheckIfSnapped())
            {
                SpawnPrefabOfObstacle();
                //brutto modo ma non mi viene in mente altro per settare l'anchor point a occupato
                draggableObstacleInstance.GetComponent<DraggableObstacle>().anchorPointSnapped.GetComponent<AnchorPoint>().SetOccupied(true);
                //hai usato un ostacolo quindi decrementa il tutto 
                AddToObstacleAmount(-1);
                //distruggi il draggable object che tanto ha finito il suo corso
                Destroy(draggableObstacleInstance.gameObject);

            }
            //altrimenti distruggi il prefab e basta
            else
                Destroy(draggableObstacleInstance.gameObject);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggableObstacleInstance != null) {
            Vector3 updatedPosition = new Vector3(Camera.main.ScreenToWorldPoint(eventData.position).x, Camera.main.ScreenToWorldPoint(eventData.position).y, 0) + draggableObstacleOffsetFromFinger;
            draggableObstacleComponent.UpdatePosition(updatedPosition);
        }
      
    }

    //semplicemente spanwa il prefab dell'ostacolo e lo mette nella posizine dell'anchor point
    void SpawnPrefabOfObstacle() {

        GameObject obstaclePrefab = Instantiate(Resources.Load<GameObject>("Prefab/Obstacles/" + obstacleType.ToString()));
        obstaclePrefab.transform.position = draggableObstacleInstance.transform.position;
        FindObjectOfType<LevelManager>().AddObstacleInstance(obstaclePrefab, obstaclePrefab.transform.position);
        
    }

    //assegna ad un draggble object una posizione tra TOP, SIDE e PLATFORM
    void AssignPositionWhereDraggableObstacleCanGo() {
        //cerca la posizione in cui il draggable object puo' andare: TOP,SIDE,PLATFORM, uso questa reference perche' serve dopo due volte
        AnchorPointPosition positionWhereObstacleGoes = levelManager.CheckInWhatPositionTheObstacleGoes(obstacleType);
        //assegna la posizione in cui puo' andare
        draggableObstacleComponent.SetSnappingDesiredPosition(positionWhereObstacleGoes);
        //lancia l'evento per tutti gli anchor point in ascolto che si illuminano o spengono a seconda se la posizione e' la loro
        onUpdateAnchorPoint(positionWhereObstacleGoes);
    }

    void SpawnDraggableObjectInstanceAndGetComponents(PointerEventData eventData) {
        //crea l'istanza del draggable object
        draggableObstacleInstance = Instantiate(draggableObstaclePrefab, Camera.main.ScreenToWorldPoint(eventData.position) + draggableObstacleOffsetFromFinger, Quaternion.identity);
        //assegna l'immagine giusta presa dalla cartella resources
        draggableObstacleInstance.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("UIObstacleImages/" + obstacleType.ToString());
        draggableObstacleComponent = draggableObstacleInstance.GetComponent<DraggableObstacle>();
    }


}
