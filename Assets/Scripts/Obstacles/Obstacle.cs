using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObstacleType
{
    //N.B: AGGIUNGERE SEMPRE I TIPI NUOVI IN FONDO ALLA LISTA!
    Bomb,
    Bullet,
    Cannon,
    Spring,
    Pendolum,
    Laser,
    Elevator,
    Raptor,
    FallingSpikes,
    Flamethrower
}

public abstract class Obstacle : MonoBehaviour
{ 
    protected bool active;
    protected Vector3 originalPosition;
    private Quaternion originalRotation;

    [Tooltip("The position of the anchor that it can occupy")]
    public AnchorPointPosition anchorPosition;
    [Tooltip("How many anchor point needs")]
    public int anchorSlotOccupied;
    [Tooltip("All the rigidbodies attached to the prefab")]
    public Rigidbody2D[] rigidbodies;
    [Tooltip("Tutti i collider non legati al drag and drop")]
    public Collider2D[] allNonDraggableColliders;

    [SerializeField]
    CircleCollider2D draggingCollider; 

    // Usata per inizializzare il drag dell'ostacolo ma  non per la sua attivazione effettiva
    protected virtual void Start ()
    {
        active = false;

        LevelManager.runLevelEvent += WakeUp;
        LevelManager.retryLevelEvent += Sleep;
        LevelManager.runLevelEvent += DisableDraggingSystem;
        LevelManager.retryLevelEvent += EnableDraggingSystem;

        StartObstacle();
    }

    //chiamato quando l'ostacolo viene rilasciato
    public virtual void OnObstacleDropped()
    {
        originalPosition = this.transform.position;
        originalRotation = this.transform.rotation;
    }

    // Update is called once per frame
    protected void Update()
    {
        if (active)
            UpdateObstacle();
    }

    //riabilita tutta la fisica del rigidbody
    protected virtual void EnablePhysics() {
        for (int i = 0; i < rigidbodies.Length; i++)
            rigidbodies[i].isKinematic = false;
        for (int i = 0; i < allNonDraggableColliders.Length; i++)
            allNonDraggableColliders[i].enabled = true;
    }

    public virtual void DisablePhysics()
    {
        //disabilita i rigidbody e toglie ogni velocita' che avevano residua
        for (int i = 0; i < rigidbodies.Length; i++)
        {
            rigidbodies[i].isKinematic = true;
            rigidbodies[i].velocity = Vector2.zero;
            rigidbodies[i].angularVelocity = 0;
        }

        //disattiva tutti i collider, questo serve a evitare che il collider fisico interferisca con il drag and drop
        for (int i = 0; i < allNonDraggableColliders.Length; i++)
            allNonDraggableColliders[i].enabled = false;
    }

    //metodo che crea un collider per il drag and drop
    public void CreateCircleDraggingCollider()
    {
        draggingCollider = gameObject.AddComponent<CircleCollider2D>();
        draggingCollider.isTrigger = true;
        draggingCollider.radius = 0.7f;
        draggingCollider.offset = Vector2.zero;
    }

    protected virtual void EnableDraggingSystem()
    {
        if (draggingCollider != null)
            draggingCollider.enabled = true; 
        GetComponent<DraggableObjectPositionUpdater>().enabled = true;
        GetComponent<DraggableObjectPositioned>().enabled = true;
    }

    public void DisableDraggingSystem()
    {
        draggingCollider.enabled = false;
        if (GetComponent<DraggableObjectPositionUpdater>() != null)
            GetComponent<DraggableObjectPositionUpdater>().enabled = false;
        if (GetComponent<DraggableObjectPositioned>() != null)
            GetComponent<DraggableObjectPositioned>().enabled = false;
    }

    protected void SetActive(bool value)
    {
        active = value;
    }

    protected virtual void ResetPosition()
    {
        this.transform.position = this.originalPosition;
        this.transform.rotation = this.originalRotation;
    }

    protected virtual void OnDestroy()
    {
        LevelManager.runLevelEvent -= WakeUp;
        LevelManager.retryLevelEvent -= Sleep;
        LevelManager.runLevelEvent -= DisableDraggingSystem;
        LevelManager.retryLevelEvent -= EnableDraggingSystem;
    }

    public PlayerDeathEvent CreatePlayerDeathEvent(Player player, Vector3 position)
    {
        switch (GetObstacleType())
        {
            case ObstacleType.Bomb: return new PlayerDeathByExplosion(player, this, position);
            case ObstacleType.Bullet: return new PlayerDeathByExplosion(player, this, position);
            case ObstacleType.Pendolum: return new PlayerDeathBySpike(player, this, position, transform.Find("Blade").gameObject);
            case ObstacleType.Raptor: return new PlayerDeathBySpike(player, this, position, this.gameObject);
            case ObstacleType.FallingSpikes: return new PlayerDeathBySpike(player, this, position, transform.Find("Spikes").gameObject);

            default: return new PlayerDeathByExplosion(player, this, position);
        }
    }

    protected abstract void StartObstacle();
    protected abstract void UpdateObstacle();
    protected abstract void WakeUp();
    protected abstract void Sleep();
    public abstract ObstacleType GetObstacleType();

}



public abstract class ObstacleWithTimer : Obstacle
{
    protected Timer timer;

    protected void SetTimer(float time)
    {
        timer = FindObjectOfType<TimerManager>().AddTimer(time);
        timer.triggeredEvent += OnTimerEnd;
    }

    protected void ResetTimer()
    {
        //il timer potrebbe non esistere se non è stato chiamato il SetTimer()
        if (timer != null)
            timer.triggeredEvent -= OnTimerEnd;
    }

    protected void StartTimer()
    {
        timer.Start();
    }

    protected abstract void OnTimerEnd();

    protected override void OnDestroy()
    {
        ResetTimer();
        base.OnDestroy();
    }

}

