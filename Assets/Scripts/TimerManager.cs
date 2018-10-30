using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public List<Timer> allTimers = new List<Timer>();
    public float updateRate;

    // Use this for initialization
    void Start() {
        InvokeRepeating("UpdateTimers", 0, updateRate);
    }

    void UpdateTimers() {
        for (int i = 0; i < allTimers.Count; i++)
        {
            allTimers[i].Update(updateRate);
        }
    }

    public Timer AddTimer(float time) {
        Timer timer = new Timer(time);
        allTimers.Add(timer);

        return timer;
    }

    public void RemoveTimer(Timer timer) {
        allTimers.Remove(timer);
    }
}

public class Timer
{ 
    bool running;
    public float actualTime;
    public float maxTime;

    public delegate void OnTimerEnd();
    public event OnTimerEnd triggeredEvent;

    public Timer(float maxTime) {
        this.maxTime = maxTime;
    }

    public void Start() {
        if (!running)
        {
            running = true;
        }
    }

    public void Stop() {
        actualTime = 0;
        running = false;
        triggeredEvent();
    }

    public void Pause() {
        running = false;
    }

    public void Resume() {
        running = true;
    }

    public void Reassign(float newMaxvalue) {
        running = true;
        actualTime = 0;
        maxTime = newMaxvalue;
    }

    public void Update(float updateTime) {

        if (running)
        {
            actualTime -= updateTime;
            if (actualTime <= 0)
            {
                Stop();
            }
        }
    }

    public bool isEnded() {

        if (actualTime == 0)
            return true;

        return false;
    }

    public float GetTime() {
        return actualTime;
    }

    public float GetMaxTimer() {
        return maxTime;
    }

}
