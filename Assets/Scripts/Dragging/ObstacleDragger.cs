using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


//classe che viene ereditata di bottoni e dagli anchor point e che serve a spawnare gli ostacoli col drag  and drop
public class ObstacleDragger : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    //il tipo di ostacolo viene settato nella classe figlio 
    [SerializeField]
    protected ObstacleType obstacleType;
    public LevelManager levelManager;

    //Variabili che tengono il prefab del draggable obstacle e vari parametri legati a questo prefab
    public GameObject draggableObstaclePrefab;
    public GameObject draggableObstacleRangePrefab;

    protected GameObject draggableObstacleInstance;
    protected DraggableObstacle draggableObstacleComponent;
    protected DraggableObstacleRange draggableObstacleRangeComponent;
    public Vector3 draggableObstacleOffsetFromFinger;


    //Eventi che vengono lanciati per aggiornare o l'illuminazione degli anchor points o lo show o hide dei bottoni
    public delegate void UpdateAnchorPoint(AnchorPointPosition position);
    public static event UpdateAnchorPoint onUpdateAnchorPoint;

    public delegate void DraggingObstacle();
    public static event DraggingObstacle onDraggingObstacle;
    public delegate void EndObstacleDrag();
    public static event EndObstacleDrag onEndDraggingObstacle;

    private void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
    }

    public void HideObstacleButtons()
    {
        onDraggingObstacle();
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
        if (draggableObstacleInstance != null)
        {
            Vector3 updatedPosition = new Vector3(Camera.main.ScreenToWorldPoint(eventData.position).x, Camera.main.ScreenToWorldPoint(eventData.position).y, 0) + draggableObstacleOffsetFromFinger;
            if (levelManager.anchorPointMode == AnchorPointMode.SINGLE)
            {
                draggableObstacleComponent.UpdatePosition(updatedPosition);
            }
            else if (levelManager.anchorPointMode == AnchorPointMode.RANGE)
            {
                draggableObstacleRangeComponent.UpdatePosition(updatedPosition);
            }
        }
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        //se non controllo se e' stato spawnato mi da' problemi dopo di null reference
        if (draggableObstacleInstance != null)
        {
            //check se se snappato ad un anchor point 
            if ((levelManager.anchorPointMode == AnchorPointMode.SINGLE && draggableObstacleComponent.CheckIfSnapped()) || (levelManager.anchorPointMode == AnchorPointMode.RANGE && draggableObstacleRangeComponent.CheckIfSnapped()))
            {
                SpawnPrefabOfObstacle();
                //brutto modo ma non mi viene in mente altro per settare l'anchor point a occupato
                if (levelManager.anchorPointMode == AnchorPointMode.SINGLE)
                {
                    draggableObstacleInstance.GetComponent<DraggableObstacle>().anchorPointSnapped.GetComponent<AnchorPoint>().SetOccupied(true);
                }
                //hai usato un ostacolo quindi decrementa il tutto 
                UpdateObstacleNumber(-1);
                //distruggi il draggable object che tanto ha finito il suo corso
                Destroy(draggableObstacleInstance.gameObject);
            }
            //altrimenti distruggi il prefab e basta
            else {
                Destroy(draggableObstacleInstance.gameObject);
                //non hai usato l'ostacolo quindi rimettilo a posto
                UpdateObstacleNumber(+1);
            }
            onEndDraggingObstacle();
        }
    }

    //metodo che aggiorna il numero di ostacoli a disposizione, implementato effettivamente nelle classi figlio
    protected virtual void UpdateObstacleNumber(int amount) {
        
    }

    //semplicemente spanwa il prefab dell'ostacolo e lo mette nella posizine dell'anchor point
    protected virtual void SpawnPrefabOfObstacle()
    {
        GameObject obstaclePrefab = Instantiate(Resources.Load<GameObject>("Prefab/Obstacles/" + obstacleType.ToString()));
        obstaclePrefab.transform.position = draggableObstacleInstance.transform.position;
        //assegna all'anchor point il gameObject appena spawnato solo se in single mode
        if (levelManager.anchorPointMode == AnchorPointMode.SINGLE)
        {
            draggableObstacleInstance.GetComponent<DraggableObstacle>().anchorPointSnapped.GetComponent<AnchorPoint>().SetObstacleAnchored(obstaclePrefab);
        }
    }

    //assegna ad un draggble object una posizione tra TOP, SIDE e PLATFORM
    protected virtual void AssignPositionWhereDraggableObstacleCanGo()
    {
        //cerca la posizione in cui il draggable object puo' andare: TOP,SIDE,PLATFORM, uso questa reference perche' serve dopo due volte
        AnchorPointPosition positionWhereObstacleGoes = levelManager.CheckInWhatPositionTheObstacleGoes(obstacleType);
        //assegna la posizione in cui puo' andare
        if (levelManager.anchorPointMode == AnchorPointMode.SINGLE)
        {
            draggableObstacleComponent.SetSnappingDesiredPosition(positionWhereObstacleGoes);
        }
        else if (levelManager.anchorPointMode == AnchorPointMode.RANGE)
        {
            draggableObstacleRangeComponent.SetSnappingDesiredPosition(positionWhereObstacleGoes);
        }

        //lancia l'evento per tutti gli anchor point in ascolto che si illuminano o spengono a seconda se la posizione e' la loro
        onUpdateAnchorPoint(positionWhereObstacleGoes);
    }

    protected virtual void SpawnDraggableObjectInstanceAndGetComponents(PointerEventData eventData)
    {
        //crea l'istanza del draggable object
        //assegna l'immagine giusta presa dalla cartella resources
        if (levelManager.anchorPointMode == AnchorPointMode.SINGLE)
        {
            draggableObstacleInstance = Instantiate(draggableObstaclePrefab, Camera.main.ScreenToWorldPoint(eventData.position) + draggableObstacleOffsetFromFinger, Quaternion.identity);
            draggableObstacleInstance.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("UIObstacleImages/" + obstacleType.ToString());
            draggableObstacleComponent = draggableObstacleInstance.GetComponent<DraggableObstacle>();
        }

        else if (levelManager.anchorPointMode == AnchorPointMode.RANGE) {
            draggableObstacleInstance = Instantiate(draggableObstacleRangePrefab, Camera.main.ScreenToWorldPoint(eventData.position) + draggableObstacleOffsetFromFinger, Quaternion.identity);
            draggableObstacleInstance.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("UIObstacleImages/" + obstacleType.ToString());
            draggableObstacleRangeComponent = draggableObstacleInstance.GetComponent<DraggableObstacleRange>();

        }


    }
}
