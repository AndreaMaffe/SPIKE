using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : ObstacleWithTimer {

    public float timeActive;
    public float timeNotActive;
    float direction;
    Animator animator;

    public ParticleSystem flameParticles;

    public override ObstacleType GetObstacleType()
    {
        return ObstacleType.Flamethrower;
    }

    protected override void OnTimerEnd()
    {
        ShootFlames();
    }

    protected override void Sleep()
    {
        //impedisce di sparare
        SetActive(false);

        //rimette il timer a zero e lo blocca
        ResetTimer();
    }

    protected override void StartObstacle()
    {
        //1 se sx-->dx , -1 se sx
        direction = -this.transform.position.x / Mathf.Abs(this.transform.position.x);

        //inverti specularmente in base alla direzione
        this.transform.rotation = new Quaternion(0, Mathf.Acos(direction) * Mathf.Rad2Deg, 0, 1);

        animator = GetComponent<Animator>();
    }

    protected override void UpdateObstacle()
    {
    }

    protected override void WakeUp()
    {
        //permette di sparare
        SetActive(true);

        //setta il timer
        SetTimer(timeActive);

        //spara il primo colpo e avvia i timer
        ShootFlames();
    }

    protected void ShootFlames() {
        if (active) {
            flameParticles.Play();
        }

        StartTimer();
    }
}
