using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : ObstacleWithTimer {

    public GameObject bullet;
    public float rateOfFire;

    private float direction;

    //lista per tenere traccia dei proiettili sparati e distruggerli al RetryLevel()
    private List<GameObject> bullets;

    //start apposito per gli ostacoli, usare questo anziché Start().
    protected override void StartObstacle()
    {
        //1 se sx-->dx , -1 se sx
        direction = - this.transform.position.x / Mathf.Abs(this.transform.position.x);

        //inverti specularmente in base alla direzione
        this.transform.rotation = new Quaternion(0, Mathf.Acos(direction) * Mathf.Rad2Deg, 0, 1);

        bullets = new List<GameObject>();
        SetTimer(rateOfFire);
    }

    //update apposito per gli ostacoli, usare questo anziché Update().
    protected override void UpdateObstacle () {

	}

    protected override void OnTimerEnd()
    {
        if (active)
        {
            //spara un colpo e aggiungilo alla List bullets
            bullets.Add(Instantiate(bullet, this.transform.position, this.transform.rotation));

            StartTimer();
        }
    }

    //chiamato al RunLevel()
    protected override void WakeUp()
    {
        //permette di sparare
        SetActive(true);

        //avvia il timer per sparare
        StartTimer();
    }

    //chiamato al RetryLevel()
    protected override void Sleep()
    {
        //impedisce di sparare
        SetActive(false);

        //distrugge i proiettili ancora in scena
        foreach (GameObject bullet in bullets)
            Destroy(bullet);

        //svuota la lista dei proiettili
        bullets.Clear();
    }


}
