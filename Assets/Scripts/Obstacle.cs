using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {

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

    void UpdateObstacle() { 

    }

    public void WakeUp() {

    }

    public void SetActive() {
        active = true;
    }

}
