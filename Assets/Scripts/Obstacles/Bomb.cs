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

    public override ObstacleType GetObstacleType()
    {
        return ObstacleType.Bomb;
    }

    //start apposito per gli ostacoli, usare questo anziché Start().
    protected override void StartObstacle()
    {
        DisablePhysics();
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
        //inizializza il timer per l'esplosione
        SetTimer(timeBeforeExplosion);

        //avvia il timer
        StartTimer();
        
        //risveglia i rigidbodies e i collider
        EnablePhysics();
    }

    //chiamato al RetryLevel()
    protected override void Sleep()
    {
        //rigenera l'immagine
        SetVisible(true);

        //risetta la posizione iniziale
        ResetPosition();

        //rimette il timer a zero e lo blocca
        ResetTimer();

        //disabilita la fisica legata a quel gameobject
        DisablePhysics();
    }

    void ShockWave()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(this.transform.position, explosionOuterRadius);

        for (int i = 0; i < hitColliders.Length; i++)
        {

            GameObject objectHit = hitColliders[i].gameObject;

            //if the object hit is within the ExplosionInnerRadius, destroy it
            if (Vector3.Distance(objectHit.transform.position, this.transform.position) < explosionInnerRadius && objectHit.tag == "Player")
                objectHit.GetComponent<Player>().SetActiveRagdoll(true);

            //otherwise, push it in the opposite direction with a thrust proportional to the distance from the explosion
            else
            {

                //float thrust = (explosionOuterRadius - Vector3.Distance(objectHit.transform.position, this.transform.position)) / explosionInnerRadius * explosionForce;
                Vector3 direction = objectHit.transform.position - this.transform.position;

                if (objectHit.GetComponent<Rigidbody2D>())
                    objectHit.GetComponent<Rigidbody2D>().AddForce(direction * explosionForce);
            }

        }

    }

    void SetVisible(bool value)
    {
        innerRadius.gameObject.SetActive(value);
        outerRadius.gameObject.SetActive(value);
        gameObject.GetComponent<SpriteRenderer>().enabled = value;
    }
}



