using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : Obstacle {

    public float rateOfFire;
    public GameObject bullet;

    private Timer timer;

	// Use this for initialization
	void Start () {

        timer = FindObjectOfType<TimerManager>().AddTimer(rateOfFire);
        timer.triggeredEvent += Shoot;
     
    }

    //update apposito per gli ostacoli, usare questo anziché Update().
    protected override void UpdateObstacle () {

	}

    public override void WakeUp() {

    }

    void Shoot() {
        Instantiate(bullet, this.transform.position, this.transform.rotation);
        timer.Start();
    }
}
