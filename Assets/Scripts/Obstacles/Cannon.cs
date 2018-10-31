using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : Obstacle {

    public float rateOfFire;
    public GameObject bullet;

    private Timer timer;

    //start apposito per gli ostacoli, usare questo anziché Start().
    protected override void StartObstacle() {

        //crea il timer e lo associa al metodo Shoot()
        timer = FindObjectOfType<TimerManager>().AddTimer(rateOfFire);
        timer.triggeredEvent += Shoot;
     
    }

    //update apposito per gli ostacoli, usare questo anziché Update().
    protected override void UpdateObstacle () {

	}

    protected override void WakeUp() {

        //avvia il timer
        timer.Start();
    }

    void Shoot() {

        //spara un colpo e riavvia il timer
        Instantiate(bullet, this.transform.position, this.transform.rotation);
        timer.Start();
    }
}
