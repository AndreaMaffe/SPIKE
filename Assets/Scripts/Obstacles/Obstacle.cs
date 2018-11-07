using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObstacleType
{
    Bomb,
    Bullet,
    Cannon,
    Spring,
    Pendolum,
    Laser,
    Elevator,
    Raptor,
    FallingSpikes
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

    // Use this for initialization
    protected void Start ()
    {
        active = false;
        originalPosition = this.transform.position;
        originalRotation = this.transform.rotation;
        LevelManager.runLevelEvent += WakeUp;
        LevelManager.retryLevelEvent += Sleep;
        StartObstacle();
    }

    // Update is called once per frame
    protected void Update()
    {
        if (active)
            UpdateObstacle();
    }


    protected abstract void StartObstacle();
    protected abstract void UpdateObstacle();
    protected abstract void WakeUp();
    protected abstract void Sleep();
    public abstract ObstacleType GetObstacleType();


    //riabilita tutta la fisica del rigidbody
    protected virtual void EnablePhysics() {
        for (int i = 0; i < rigidbodies.Length; i++)
            rigidbodies[i].simulated = true;
        for (int i = 0; i < allNonDraggableColliders.Length; i++)
            allNonDraggableColliders[i].enabled = true;
    }

    protected virtual void DisablePhysics()
    {
        //disabilita i rigidbody e toglie ogni velocita' che avevano residua
        for (int i = 0; i < rigidbodies.Length; i++) {
            rigidbodies[i].simulated = false;
            rigidbodies[i].velocity = Vector2.zero;
            rigidbodies[i].angularVelocity = 0;
        }

        //disattiva tutti i collider, questo serve a evitare che il collider fisico interferisca con il drag and drop
        for (int i = 0; i < allNonDraggableColliders.Length; i++)
            allNonDraggableColliders[i].enabled = false;
    }
    
    protected void SetActive(bool value)
    {
        active = value;
    }

    protected void ResetPosition()
    {
        this.transform.position = this.originalPosition;
        this.transform.rotation = this.originalRotation;
    }

    protected virtual void OnDestroy()
    {
        LevelManager.runLevelEvent -= WakeUp;
        LevelManager.retryLevelEvent -= Sleep;
    }

    public PlayerDeathEvent CreatePlayerDeathEvent(Player player, Vector3 position)
    {
        switch (GetObstacleType())
        {
            case ObstacleType.Bomb: return new PlayerDeathByExplosion(player, this, position);
            case ObstacleType.Bullet: return new PlayerDeathByExplosion(player, this, position);
            case ObstacleType.Pendolum: return new PlayerDeathBySpike(player, this, position);
            case ObstacleType.Raptor: return new PlayerDeathBySpike(player, this, position);

            default: return new PlayerDeathByExplosion(player, this, position);
        }
    }

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

