using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum AnchorPointPosition { Top, Side, Platform };

public class AnchorPoint : ObstacleDragger, IBeginDragHandler, IDragHandler,IEndDragHandler {
 
    [Header("Position can be: top, side o platform")]
    public AnchorPointPosition position;
    public bool edge;
    [SerializeField]
    private bool occupied;

    //quando il level manager attiva il livello sto bool diventa false e quando e' underconstruction diventa true
    private bool canDragObstacle = true;

    SpriteRenderer spriteRenderer;
    public CircleCollider2D circleCollider;

    public GameObject obstacleAnchored;

    public delegate void ReaddObstacleToUse(ObstacleType obstacleType, int amount);
    public static event ReaddObstacleToUse OnReaddObstacle;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        levelManager = FindObjectOfType<LevelManager>();
        //sottoscrizione all'evento lanciato dai bottoni per aggiornare la visibilita' degli anchor points
        onUpdateAnchorPoint += UpdateAnchorPointSprite;
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

    //desottoscrivi gli eventi 
    private void OnDisable()
    {
        onUpdateAnchorPoint -= UpdateAnchorPointSprite;
        LevelManager.runLevelEvent -= HideAnchorPoint;
        LevelManager.runLevelEvent -= SetCanDragObstacleToFalse;
        LevelManager.retryLevelEvent -= SetCanDragObstacleToTrue;
    }

    // abilita o disabilita lo sprite renderer a seconda che la posizione sia occupata e che tu stai trascinando un ostacolo del suo tipo
    void UpdateAnchorPointSprite(AnchorPointPosition pos)
    {
        if (pos == position && !occupied)
            ShowAnchorPoint();
        else
            HideAnchorPoint();
    }

    void ShowAnchorPoint() {
        spriteRenderer.enabled = true;    
        circleCollider.enabled = true;
    }

    void HideAnchorPoint() {
        spriteRenderer.enabled = false;
        if (!occupied)
            circleCollider.enabled = false;
    }

    public void SetOccupied(bool occupied) {
        this.occupied = occupied;
        if (occupied) {
            spriteRenderer.enabled = false;
            
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
        obstacleType = obstacleAnchored.GetComponent<Obstacle>().obstacleType;
    }


    void DeassignObstacle() {
        occupied = false;
        UpdateAnchorPointSprite(position);
        SetOccupied(occupied);
        Destroy(obstacleAnchored);
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
       
        // se c'e' un ostacolo ed e sono nella fase di under construction
        if (canDragObstacle && occupied) {
            Debug.Log("Begin Drag");
            SpawnDraggableObjectInstanceAndGetComponents(eventData);
            AssignPositionWhereDraggableObstacleCanGo();
            DeassignObstacle();
            HideObstacleButtons();
        }
    }

    protected override void UpdateObstacleNumber(int amount)
    {
        if (amount > 0)
            OnReaddObstacle(obstacleType, amount);
    }

}
