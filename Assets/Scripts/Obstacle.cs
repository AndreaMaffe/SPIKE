using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Obstacle : MonoBehaviour {

    private bool active;

	// Use this for initialization
	void Start () {
        active = false;
	}

    // Update is called once per frame
    void Update() {
        if (active)
            UpdateObstacle();
    }

    protected abstract void UpdateObstacle();

    public abstract void WakeUp();

    public void SetActive() {
        active = true;
    }

}
