using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : ObstacleWithTimer {

    public GameObject bullet;

    //start apposito per gli ostacoli, usare questo anziché Start().
    protected override void StartObstacleWithTimer() {

    }

    //update apposito per gli ostacoli, usare questo anziché Update().
    protected override void UpdateObstacleWithTimer () {

	}

    //chiamato al RunLevel()
    protected override void WakeUp() {
        StartTimer();
    }

    void Shoot() {

        //spara un colpo e riavvia il timer
        Instantiate(bullet, this.transform.position, this.transform.rotation);
        StartTimer();
    }


    protected override void OnTimerEnd()
    {
        Shoot();
    }

}
