using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum AnchorPointPosition { Top, Side, Platform };

public class AnchorPoint : MonoBehaviour, IBeginDragHandler, IDragHandler,IEndDragHandler {
 
    [Header("Position can be: top, side o platform")]
    public AnchorPointPosition position;
    public bool edge;
    [SerializeField]
    private bool occupied;

    //quando il level manager attiva il livello sto bool diventa false e quando e' underconstruction diventa true
    private bool canDragObstacle;

    public SpriteRenderer spriteRenderer;
    public CircleCollider2D circleCollider;

    public GameObject obstacleAnchored;


    private void Start()
    {
        //sottoscrizione all'evento lanciato dai bottoni per aggiornare la visibilita' degli anchor points
        ObstacleButton.onUpdateAnchorPoint += UpdateAnchorPointSprite;
        LevelManager.runLevelEvent += HideAnchorPoint;
        LevelManager.runLevelEvent += SetCanDragObstacleToFalse;
        LevelManager.retryLevelEvent += SetCanDragObstacleToTrue;

        spriteRenderer.enabled = false;
    }

    void SetCanDragObstacleToTrue() {
        canDragObstacle = true;
    }

    void SetCanDragObstacleToFalse ()
    {
        canDragObstacle = false;
    }

    private void OnDisable()
    {
        ObstacleButton.onUpdateAnchorPoint -= UpdateAnchorPointSprite;
        LevelManager.runLevelEvent -= HideAnchorPoint;
    }

    // abilita o disabilita lo sprite renderer a seconda che la posizione sia occupata e che tu stai trascinando un ostacolo del suo tipo
    void UpdateAnchorPointSprite(AnchorPointPosition pos)
    {
        if (pos == position && !occupied)
            spriteRenderer.enabled = true;
        else
            spriteRenderer.enabled = false;
    }

    void HideAnchorPoint() {
        spriteRenderer.enabled = false;
    }

    public void SetOccupied(bool occupied) {
        this.occupied = occupied;
        if (occupied) {
            spriteRenderer.enabled = false;
            circleCollider.enabled = false;
        }
        else
            circleCollider.enabled = true;

    }

    public bool GetOccupied() {
        return occupied;
    }

    public void SetPosition(AnchorPointPosition position) {
        this.position = position; 
    }

    public AnchorPointPosition GetPosition() {
        return position;
    }

    public void SetEdge(bool edge) {
        this.edge = edge;
    }

    public void SetObstacleAnchored(GameObject obstacle)
    {
        obstacleAnchored = obstacle;
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin Drag");
        if (canDragObstacle && occupied) {


        }
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
    }
}
