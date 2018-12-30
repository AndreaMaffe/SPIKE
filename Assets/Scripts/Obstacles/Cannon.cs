using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cannon : ObstacleWithTimer {

    public GameObject bullet;
    public float rateOfFire;

    private Animator animator;

    private float direction;

    public GameObject particleExplosion;
    //offset rispetto all'origine dal quale parte il proiettile e le particle del'esplosione
    public Vector3 shootingOffset;


    protected override void OnPlayerDeath()
    {
        SetActive(false);
    }

    public override void OnObstacleDropped()
    {
        //1 se sx-->dx , -1 se sx
        direction = - this.transform.position.x / Mathf.Abs(this.transform.position.x);

        //inverti specularmente in base alla direzione
        this.transform.rotation = new Quaternion(0, Mathf.Acos(direction) * Mathf.Rad2Deg, 0, 1);

        animator = GetComponent<Animator>();
    }

    protected override void StartObstacle()
    {
    }

    //update apposito per gli ostacoli, usare questo anziché Update().
    protected override void UpdateObstacle () {

	}

    protected override void OnTimerEnd()
    {
        Shoot();
    }

    void Shoot()
    {
        if (active)
        {
            animator.SetTrigger("Shoot");
            GameObject particle = Instantiate(particleExplosion, transform.position + shootingOffset * direction, Quaternion.identity);
            Destroy(particle.gameObject, 1f);

            //spara un colpo
            Instantiate(bullet, this.transform.position + shootingOffset * direction, this.transform.rotation);

            FindObjectOfType<AudioManager>().Play("cannon");

            StartTimer();
        }
    }

    //chiamato al RunLevel()
    protected override void WakeUp()
    {
        //permette di sparare
        SetActive(true);

        //setta il timer
        SetTimer(rateOfFire);

        //spara il primo colpo e avvia i timer
        Shoot();
    }

    //chiamato al RetryLevel()
    protected override void Sleep()
    {
        //impedisce di sparare
        SetActive(false);

        //rimette il timer a zero e lo blocca
        ResetTimer();
    }

    public override ObstacleType GetObstacleType()
    {
        return ObstacleType.Cannon;
    }


}
