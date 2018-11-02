using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : ObstacleWithTimer {

    public float timeBeforeExplosion;
    public float explosionForce;
    public float explosionInnerRadius;
    public float explosionOuterRadius;

    public Transform innerRadius;
    public Transform outerRadius;

    public GameObject explosionParticlePrefab;


    //start apposito per gli ostacoli, usare questo anziché Start().
    protected override void StartObstacle()
    {
        SetTimer(timeBeforeExplosion);

        //scala le sprite dei cerchi
        innerRadius.localScale = new Vector3(innerRadius.localScale.x /2 * explosionInnerRadius, innerRadius.localScale.y /2 * explosionInnerRadius, 1);
        outerRadius.localScale = new Vector3(outerRadius.localScale.x /4 * explosionOuterRadius, outerRadius.localScale.y /4 * explosionOuterRadius, 1);

    }

    protected override void UpdateObstacle()
    {
    }

    void Explode()
    {
        //genera l'onda d'urto 
        ShockWave();

        //crea effetti particellari
        Destroy(Instantiate(explosionParticlePrefab, transform.position, Quaternion.identity), 1.5f);

        //destroy the bomb
        SetVisible(false);
    }

    protected override void OnTimerEnd()
    {
        Explode();
    }

    //chiamato al RunLevel()
    protected override void WakeUp()
    {
        //avvia il timer per l'esplosione
        StartTimer();
    }

    //chiamato al RetryLevel()
    protected override void Sleep()
    {
        //rigenera l'immagine
        SetVisible(true);

        //risetta la posizione iniziale
        ResetPosition();
    }

    void ShockWave()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(this.transform.position, explosionOuterRadius);

        for (int i = 0; i < hitColliders.Length; i++)
        {

            GameObject objectHit = hitColliders[i].gameObject;

            //if the object hit is within the ExplosionInnerRadius, destroy it
            if (Vector3.Distance(objectHit.transform.position, this.transform.position) < explosionInnerRadius && objectHit.tag == "Player")
                Destroy(objectHit);

            //otherwise, push it in the opposite direction with a thrust proportional to the distance from the explosion
            else
            {
                float thrust = (explosionOuterRadius - Vector3.Distance(objectHit.transform.position, this.transform.position)) / explosionInnerRadius * explosionForce;
                Vector3 direction = objectHit.transform.position - this.transform.position;

                if (objectHit.GetComponent<Rigidbody2D>())
                    objectHit.GetComponent<Rigidbody2D>().AddForce(direction * thrust);
            }

        }

    }

    void SetVisible(bool value)
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = value;
        gameObject.GetComponent<CircleCollider2D>().enabled = value;
    }
}



