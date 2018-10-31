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
    DraggableObstacle draggableObstacleCollisionChecker;
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
        obstacleAmountText.text = "x " + obstacleAmount;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        draggableObstacleInstance = Instantiate(draggableObstaclePrefab, Camera.main.ScreenToWorldPoint(eventData.position) +  draggableObstacleOffsetFromFinger, Quaternion.identity);
        draggableObstacleInstance.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("UIObstacleImages/" + obstacleType.ToString());
        draggableObstacleCollisionChecker = draggableObstacleInstance.GetComponent<DraggableObstacle>();
        AnchorPointPosition positionWhereObstacleGoes = levelManager.CheckInWhatPositionTheObstacleGoes(obstacleType);
        draggableObstacleCollisionChecker.SetSnappingDesiredPosition(positionWhereObstacleGoes);
        onUpdateAnchorPoint(positionWhereObstacleGoes);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (draggableObstacleCollisionChecker.CheckIfSnapped())
        {
            GameObject obstaclePrefab = Instantiate(Resources.Load<GameObject>("Prefab/Obstacles/" + obstacleType.ToString()));
            obstaclePrefab.transform.position = draggableObstacleInstance.transform.position;
            Destroy(draggableObstacleInstance.gameObject);
            draggableObstacleInstance.GetComponent<DraggableObstacle>().anchorPointSnapped.GetComponent<AnchorPoint>().SetOccupied(true);
            //hai usato un ostacolo quindi decrementa il tutto 
            AddToObstacleAmount(-1);
        }
        else
            Destroy(draggableObstacleInstance.gameObject);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 updatedPosition = new Vector3(Camera.main.ScreenToWorldPoint(eventData.position).x, Camera.main.ScreenToWorldPoint(eventData.position).y, 0) + draggableObstacleOffsetFromFinger;
        draggableObstacleCollisionChecker.UpdatePosition(updatedPosition);
    }



}
