using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour {

    public List<Timer> allTimers = new List<Timer>();
    public float updateRate;
  
	// Use this for initialization
	void Start () {
        InvokeRepeating("UpdateTimers", 0, updateRate);
	}

    void UpdateTimers() {
        for (int i = 0; i < allTimers.Count; i++) {
            allTimers[i].UpdateTimer(updateRate);
        }
    }

    public void AddTimer(Timer timer) {
        allTimers.Add(timer);
    }

    public void RemoveTimer(Timer timer) {
        allTimers.Remove(timer);
    }
}

public class Timer {

    bool stopped;
    bool started;
    public float actualTime;
    public float maxTime;

    //se e' un timer del salto o un timer che deve stare fermo
    private string type;

    public delegate void OnTimerEnd();
    public event OnTimerEnd TimerEnded;


    public Timer(float maxTime, string type) {
        this.type = type;
        this.maxTime = maxTime;
    }

    public void StartTimer() {
        started = true;
        stopped = false;
        actualTime = 0;
    }

    public void ReassignTimer(float newMaxvalue) {
        started = true;
        stopped = false;
        actualTime = 0;
        maxTime = newMaxvalue;
    }

    public void UpdateTimer(float updateTime) {
        if (!stopped) {
            actualTime += updateTime;
            if (actualTime >= maxTime && !stopped)
            {
                EndTimer();
            }
        }        
    }

    public void StopTimer() {
        stopped = true;
    }

    public void EndTimer() {      
        StopTimer();
        TimerEnded();
    }

    public float GetTimer() {
        return actualTime;
    }

    public float GetMaxTimer()
    {
        return maxTime;
    }

}
