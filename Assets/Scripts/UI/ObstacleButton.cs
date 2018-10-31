using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ObstacleButton : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler {

    private string obstacleName;
    private int obstacleAmount;

    public Image obstacleImage;
    public TextMeshProUGUI obstacleAmountText;

    public GameObject draggableObstaclePrefab;
    GameObject draggableObstacleInstance;
    DraggableObstacle draggableObstacleCollisionChecker;
    public Vector3 draggableObstacleOffsetFromFinger;

    public void AssignObstacleTypeAndAmount(string type, int amount) {
        this.obstacleName = type;
        this.obstacleAmount = amount;
        AssignUIValues();
    }

    void AssignUIValues()
    {
        obstacleImage.sprite = Resources.Load<Sprite>("UIObstacleImages/" + obstacleName);
        obstacleAmountText.text = "x " + obstacleAmount;
    }

    //per ora non fa un cazzo 
    Vector3 SnapObstacleToGrid() {
        return Vector3.zero;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        draggableObstacleInstance = Instantiate(draggableObstaclePrefab, Camera.main.ScreenToWorldPoint(eventData.position) +  draggableObstacleOffsetFromFinger, Quaternion.identity);
        draggableObstacleInstance.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("UIObstacleImages/" + obstacleName);
        draggableObstacleCollisionChecker = draggableObstacleInstance.GetComponent<DraggableObstacle>();
        //draggableObstacleCollisionChecker.SetPosition()
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (draggableObstacleCollisionChecker.CheckIfSnapped())
        {
            GameObject obstaclePrefab = Instantiate(Resources.Load<GameObject>("Prefab/Obstacles/" + obstacleName));
            //obstaclePrefab.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(eventData.position).x, Camera.main.ScreenToWorldPoint(eventData.position).y, 0) + draggableObstacleOffsetFromFinger;
            obstaclePrefab.transform.position = draggableObstacleInstance.transform.position;
            Destroy(draggableObstacleInstance.gameObject);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 updatedPosition = new Vector3(Camera.main.ScreenToWorldPoint(eventData.position).x, Camera.main.ScreenToWorldPoint(eventData.position).y, 0) + draggableObstacleOffsetFromFinger;
        draggableObstacleCollisionChecker.UpdatePosition(updatedPosition);
    }



}
