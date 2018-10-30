using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : Obstacle {

    public float rateOfFire;
    public GameObject bullet;

    private float timer;

	// Use this for initialization
	void Start () {

        timer = rateOfFire;
	}

    //update apposito per gli ostacoli, usare questo anziché Update().
    void UpdateObstacle () {

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            Instantiate(bullet, this.transform.position, this.transform.rotation);
            timer = rateOfFire;
        }
	}
}
