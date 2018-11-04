using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cannon : ObstacleWithTimer {

    public GameObject bullet;
    public float rateOfFire;

    private float direction;

    //lista per tenere traccia dei proiettili sparati e distruggerli al RetryLevel()
    private List<GameObject> bullets;

    public Animator animator;
    public GameObject particleExplosion;
    //offset rispetto all'origine dal quale parte il proiettile e le particle del'esplosione
    public Vector3 shootingOffset;

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
            animator.SetTrigger("Shoot");
            GameObject particle = Instantiate(particleExplosion, transform.position + shootingOffset * direction, Quaternion.identity);
            Destroy(particle.gameObject, 1f);
            //spara un colpo e aggiungilo alla List bullets
            bullets.Add(Instantiate(bullet, this.transform.position + shootingOffset * direction, this.transform.rotation));

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
