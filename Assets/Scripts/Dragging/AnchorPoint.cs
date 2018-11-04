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
    private bool canDragObstacle = true;

    public SpriteRenderer spriteRenderer;
    public CircleCollider2D circleCollider;

    public GameObject obstacleAnchored;
    public ObstacleType obstacleType;
    LevelManager levelManager;

    public GameObject draggableObstaclePrefab;
    GameObject draggableObstacleInstance;
    DraggableObstacle draggableObstacleComponent;
    public Vector3 draggableObstacleOffsetFromFinger;

    public delegate void UpdateAnchorPoint(AnchorPointPosition position);
    public static event UpdateAnchorPoint onUpdateAnchorPoint;
    public delegate void DraggingObstacle();
    public static event DraggingObstacle onDraggingObstacle;
    public delegate void EndObstacleDrag();
    public static event EndObstacleDrag onEndDraggingObstacle;

    private void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        //sottoscrizione all'evento lanciato dai bottoni per aggiornare la visibilita' degli anchor points
        ObstacleButton.onUpdateAnchorPoint += UpdateAnchorPointSprite;
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
        ObstacleButton.onUpdateAnchorPoint -= UpdateAnchorPointSprite;
        LevelManager.runLevelEvent -= HideAnchorPoint;
        LevelManager.runLevelEvent -= SetCanDragObstacleToFalse;
        LevelManager.retryLevelEvent -= SetCanDragObstacleToTrue;
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

    public void OnBeginDrag(PointerEventData eventData)
    {
       
        // se c'e' un ostacolo ed e sono nella fase di under construction
        if (canDragObstacle && occupied) {
            Debug.Log("Begin Drag");
            SpawnDraggableObjectInstanceAndGetComponents(eventData);
            AssignPositionWhereDraggableObstacleCanGo();
            DeassignObstacle();
            onDraggingObstacle();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggableObstacleInstance != null)
        {
            Vector3 updatedPosition = new Vector3(Camera.main.ScreenToWorldPoint(eventData.position).x, Camera.main.ScreenToWorldPoint(eventData.position).y, 0) + draggableObstacleOffsetFromFinger;
            draggableObstacleComponent.UpdatePosition(updatedPosition);
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
               
                //distruggi il draggable object che tanto ha finito il suo corso
                Destroy(draggableObstacleInstance.gameObject);

            }
            //altrimenti distruggi il prefab e basta
            else {
                Destroy(draggableObstacleInstance.gameObject);
                //AddToObstacleAmount(+1);
            }
                

            onEndDraggingObstacle();
        }
    }

    //assegna ad un draggble object una posizione tra TOP, SIDE e PLATFORM
    void AssignPositionWhereDraggableObstacleCanGo()
    {
        //cerca la posizione in cui il draggable object puo' andare: TOP,SIDE,PLATFORM, uso questa reference perche' serve dopo due volte
        AnchorPointPosition positionWhereObstacleGoes = levelManager.CheckInWhatPositionTheObstacleGoes(obstacleType);
        //assegna la posizione in cui puo' andare
        draggableObstacleComponent.SetSnappingDesiredPosition(positionWhereObstacleGoes);
        //lancia l'evento per tutti gli anchor point in ascolto che si illuminano o spengono a seconda se la posizione e' la loro
        onUpdateAnchorPoint(positionWhereObstacleGoes);
    }

    void SpawnDraggableObjectInstanceAndGetComponents(PointerEventData eventData)
    {
        //crea l'istanza del draggable object
        draggableObstacleInstance = Instantiate(draggableObstaclePrefab, Camera.main.ScreenToWorldPoint(eventData.position) + draggableObstacleOffsetFromFinger, Quaternion.identity);
        //assegna l'immagine giusta presa dalla cartella resources
        draggableObstacleInstance.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("UIObstacleImages/" + obstacleType.ToString());
        draggableObstacleComponent = draggableObstacleInstance.GetComponent<DraggableObstacle>();
    }

    //semplicemente spanwa il prefab dell'ostacolo e lo mette nella posizine dell'anchor point
    void SpawnPrefabOfObstacle()
    {

        GameObject obstaclePrefab = Instantiate(Resources.Load<GameObject>("Prefab/Obstacles/" + obstacleType.ToString()));
        obstaclePrefab.transform.position = draggableObstacleInstance.transform.position;
        //assegna all'anchor point il gameObject appena spawnato
        draggableObstacleInstance.GetComponent<DraggableObstacle>().anchorPointSnapped.GetComponent<AnchorPoint>().SetObstacleAnchored(obstaclePrefab);

    }
}
