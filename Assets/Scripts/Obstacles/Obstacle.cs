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
    Flamethrower,
    Spikes
}

public abstract class Obstacle : MonoBehaviour
{ 
    protected bool active;
    protected Vector3 originalPosition;
    private Quaternion originalRotation;
    private CircleCollider2D draggingCollider;

    public AnchorPointPosition anchorPosition;
    public Collider2D[] allColliders;

    // Usata per inizializzare il drag dell'ostacolo ma  non per la sua attivazione effettiva
    protected virtual void Start ()
    {
        active = false;

        LevelManager.runLevelEvent += WakeUp;
        LevelManager.retryLevelEvent += Sleep;
        LevelManager.endLevelEvent += OnEndLevel;
        LevelManager.playerDeathEvent += OnPlayerDeath;

        LevelManager.runLevelEvent += DisableDraggingSystem;
        LevelManager.retryLevelEvent += EnableDraggingSystem;

        LevelManager.NumberOfObstacles[GetObstacleType()] += 1;
                  

        StartObstacle();
    }

    //chiamato quando l'ostacolo viene rilasciato
    public virtual void OnObstacleDropped()
    {
        originalPosition = this.transform.position;
        originalRotation = this.transform.rotation;
    }

    // Update is called once per frame
    protected void FixedUpdate()
    {
        if (active)
            UpdateObstacle();
    }

     protected void SetCollidersActive(bool value)
    {
        foreach (Collider2D collider in allColliders)
            collider.enabled = value;
    }

    protected virtual void SetDynamicRigidbodyActive(Rigidbody2D rb, bool value)
    {
        if (value)
            rb.isKinematic = false;
        else
        {
            rb.isKinematic = true;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0;
        }            
    }

    //metodo che crea un collider per il drag and drop
    public void CreateCircleDraggingCollider()
    {
        draggingCollider = gameObject.AddComponent<CircleCollider2D>();
        draggingCollider.isTrigger = true;
        draggingCollider.radius = 0.6f;
        draggingCollider.offset = Vector2.zero;
    }

    protected virtual void EnableDraggingSystem()
    {
        if (draggingCollider != null)
            draggingCollider.enabled = true; 
        if (GetComponent<DraggableObjectPositionUpdater>() != null)
            GetComponent<DraggableObjectPositionUpdater>().enabled = true;
        if (GetComponent<DraggableObjectPositioned>() != null)
            GetComponent<DraggableObjectPositioned>().enabled = true;
    }

    public void DisableDraggingSystem()
    {
        if (draggingCollider != null)
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
        LevelManager.endLevelEvent -= OnEndLevel;

        LevelManager.runLevelEvent -= DisableDraggingSystem;
        LevelManager.retryLevelEvent -= EnableDraggingSystem;

        LevelManager.NumberOfObstacles[GetObstacleType()] -= 1;
    }

    public PlayerDeathEvent CreatePlayerDeathEvent(Player player, Vector3 position)
    {
        switch (GetObstacleType())
        {
            //case ObstacleType.Bomb: return new PlayerDeathByExplosion(player, this, position);
            case ObstacleType.Bullet: return new PlayerDeathByExplosion(player, this, position);
            case ObstacleType.Pendolum: return new PlayerDeathBySpike(player, this, position, transform.Find("Body").transform.Find("Blade").gameObject);
            case ObstacleType.Raptor: return new PlayerDeathBySpike(player, this, position, transform.Find("Saw").gameObject);
            case ObstacleType.FallingSpikes: return new PlayerDeathBySpike(player, this, position, transform.Find("Spikes").gameObject);
            case ObstacleType.Spring: return new PlayerDeathBySpike(player, this, position, transform.Find("Spikes").gameObject);
            case ObstacleType.Spikes: return new PlayerDeathBySpike(player, this, position, this.gameObject);
            case ObstacleType.Flamethrower: return new PlayerDeathByExplosion(player, this, position);
            case ObstacleType.Elevator: return new PlayerDeathBySpike(player, this, position, this.gameObject);

            default: return new PlayerDeathByExplosion(player, this, position);
        }
    }

    protected virtual void OnEndLevel()
    {
        SetActive(false);
    }

    protected virtual void OnPlayerDeath()
    {
        SetActive(false);
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

