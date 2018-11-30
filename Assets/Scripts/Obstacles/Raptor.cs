using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raptor : Obstacle
{
    private GameObject objectToFollow;
    private Rigidbody2D rb;
    public float speed; // velocità del raptor

    //start apposito per gli ostacoli, usare questo anziché Start().
    protected override void StartObstacle()
    {
        objectToFollow = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();

        SetCollidersActive(false); SetDynamicRigidbodyActive(rb, false);;
    }

    //update apposito per gli ostacoli, usare questo anziché Update().
    protected override void UpdateObstacle()
    {
        //applica una forza orizzontale in direzione del Player
        Vector2 direction = (objectToFollow.GetComponent<Rigidbody2D>().worldCenterOfMass - this.rb.worldCenterOfMass).normalized;
        direction.y = 0;
        rb.AddForce(direction * speed /100, ForceMode2D.Impulse);

        //inverti la sprite se il player è a destra del Raptor
        if (objectToFollow.transform.position.x > this.transform.position.x)
            this.transform.rotation = new Quaternion(0, 180, 0, 1);
        else this.transform.rotation = Quaternion.identity;
    }

    //chiamato al RunLevel()
    protected override void WakeUp()
    {
        //abilita le animazioni
        GetComponent<Animator>().enabled = true;
        transform.Find("Body").transform.Find("Smoke").GetComponent<ParticleSystem>().Play();

        //permette di entrare nell'UpdateObstacle()
        SetActive(true);

        rb.drag = 0;

        SetCollidersActive(true); SetDynamicRigidbodyActive(rb, true);

        FindObjectOfType<AudioManagerBR>().Play("raptor");
    }

    //chiamato al RetryLevel()
    protected override void Sleep()
    {
        //disabilita le animazioni
        GetComponent<Animator>().enabled = false;
        transform.Find("Body").transform.Find("Smoke").GetComponent<ParticleSystem>().Stop();

        //impedisce di entrare nell'UpdateObstacle()
        SetActive(false);

        //risetta la posizione iniziale
        ResetPosition();

        SetCollidersActive(false); SetDynamicRigidbodyActive(rb, false);;

        FindObjectOfType<AudioManagerBR>().Stop("raptor");
    }

    protected override void OnEndLevel()
    {
        SetActive(false);
        rb.drag = 4;
    }

    public override ObstacleType GetObstacleType()
    {
        return ObstacleType.Raptor;
    }
}
