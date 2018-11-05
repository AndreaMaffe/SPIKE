using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Obstacle : MonoBehaviour
{
    protected bool active;
    private Vector3 originalPosition;

    [Tooltip("The position of the anchor that it can occupy")]
    public AnchorPointPosition anchorPosition;
    [Tooltip("How many anchor point needs")]
    public int anchorSlotOccupied;
       
    // Use this for initialization
    protected void Start ()
    {
        active = false;
        originalPosition = this.transform.position;
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

    protected abstract void Sleep();
    protected abstract void UpdateObstacle();
    protected abstract void StartObstacle();
    protected abstract void WakeUp();
    public abstract ObstacleType GetObstacleType();
    
    protected void SetActive(bool value)
    {
        active = value;
    }

    protected void ResetPosition()
    {
        this.transform.position = this.originalPosition;
    }

    protected virtual void OnDestroy()
    {
        LevelManager.runLevelEvent -= WakeUp;
        LevelManager.retryLevelEvent -= Sleep;
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

