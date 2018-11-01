using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Obstacle : MonoBehaviour
{
    private bool active;

    [Header("The position of the anchor that it can occupy")]
    public AnchorPointPosition anchorPosition;
    [Header("How many anchor point needs")]
    public int anchorSlotOccupied;

	// Use this for initialization
	protected void Start () {
        active = false;
        LevelManager.triggeredEvent += WakeUp;
        LevelManager.triggeredEvent += SetActive;
        StartObstacle();
    }

    // Update is called once per frame
    protected void Update() {
        if (active)
            UpdateObstacle();
    }

    protected abstract void UpdateObstacle();
    protected abstract void StartObstacle();
    protected abstract void WakeUp();
    
    protected void SetActive() {
        active = true;
    }

}

public abstract class ObstacleWithTimer : Obstacle
{
    protected Timer timer;
    public float timerTime;

    protected override void StartObstacle() {
        timer = FindObjectOfType<TimerManager>().AddTimer(timerTime);
        timer.triggeredEvent += OnTimerEnd;
        StartObstacleWithTimer();
    }

    protected override void UpdateObstacle()
    {
        UpdateObstacleWithTimer();
    }

    protected abstract void OnTimerEnd();
    protected abstract void StartObstacleWithTimer();
    protected abstract void UpdateObstacleWithTimer();

    protected void OnDisable()
    {
        timer.triggeredEvent -= OnTimerEnd;

    }

    protected void OnDestroy()
    {
        timer.triggeredEvent -= OnTimerEnd;
    }
    
    protected void StartTimer()
    {
        timer.Start();
    }


}
