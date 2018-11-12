using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


//classe Assegnata a tutti gli oggetti che possono effettuare il drag, ossia i bottoni e gli ostacoli stessi
public class DraggableObject : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler {

    //offset dal dito della posizione dell'ostacolo;
    public Vector3 draggableObstacleOffsetFromFinger;

    [SerializeField]
    protected DraggableObjectPositionUpdater obstacleDragged;

    [SerializeField]
    protected ObstacleType obstacleType;

    public GameObject draggableObjectPrefab;


    //Eventi che vengono lanciati per aggiornare o l'illuminazione degli anchor points o lo show o hide dei bottoni
    public delegate void UpdateAnchorPoint(AnchorPointPosition position);
    public static event UpdateAnchorPoint onUpdateAnchorPoint;

    public delegate void DraggingObstacle();
    public static event DraggingObstacle onDraggingObstacle;
    public delegate void EndObstacleDrag();
    public static event EndObstacleDrag onEndDraggingObstacle;

    private void Start()
    {
        draggableObstacleOffsetFromFinger = new Vector3(0, 1, 0);
    }

    public void HideObstacleButtons()
    {
        onDraggingObstacle();
        onUpdateAnchorPoint(obstacleDragged.GetComponent<Obstacle>().anchorPosition);
    }


    public void ShowObstacleButtons()
    {
        onEndDraggingObstacle();
    }

    // sia i bottoni che gli anchor point lo implementano diversamente
    public virtual void OnBeginDrag(PointerEventData eventData)
    {

    }

   

    public virtual void OnDrag(PointerEventData eventData)
    {
        if (obstacleDragged != null)
        {
            Vector3 updatedPosition = new Vector3(Camera.main.ScreenToWorldPoint(eventData.position).x, Camera.main.ScreenToWorldPoint(eventData.position).y, 0) + draggableObstacleOffsetFromFinger;                  
            obstacleDragged.UpdatePosition(updatedPosition);
        }
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        //se non controllo se e' stato spawnato mi da' problemi dopo di null reference
        if (obstacleDragged != null)
        {
            //check se se snappato ad un anchor point range
            if (obstacleDragged.CheckIfSnapped()) {
                obstacleDragged.GetComponent<Obstacle>().OnObstacleDropped();
                UpdateObstacleNumber(-1);
            }
            //altrimenti distruggi il prefab e basta
            else
            {
                Destroy(obstacleDragged.gameObject);
                obstacleDragged = null;
                //non hai usato l'ostacolo quindi rimettilo a posto
                UpdateObstacleNumber(+1);
            }
            onEndDraggingObstacle();
        }
    }

    protected virtual void AddObstaclePositionedComponents() {
        if (obstacleDragged.GetComponent<DraggableObjectPositioned>() == null)
            obstacleDragged.gameObject.AddComponent<DraggableObjectPositioned>();
        obstacleDragged.GetComponent<Obstacle>().CreateCircleDraggingCollider();
    }

    //metodo che aggiorna il numero di ostacoli a disposizione, implementato effettivamente nelle classi figlio
    protected virtual void UpdateObstacleNumber(int amount)
    {

    }

}
