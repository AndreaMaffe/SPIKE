using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : ObstacleWithTimer {

    private Collider2D coll;
    private Rigidbody2D rb;

    public float timeBeforeExplosion;
    public float minExplosionForce;
    public float maxExplosionForce;
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
        rb = FindObjectOfType<Rigidbody2D>();
        coll = FindObjectOfType<Collider2D>();

        DisablePhysics();
    }

    protected override void UpdateObstacle()
    {
    }

   
    void Explode()
    {
        //FindObjectOfType<AudioManagerBR>().Play("bomb");

        //genera l'onda d'urto 
        ShockWave();

        //crea effetti particellari
        Destroy(Instantiate(explosionParticlePrefab, transform.position, Quaternion.identity), 1.5f);

        //destroy the bomb
        SetVisible(false);

        //disabilita la fisica legata al gameobject
        DisablePhysics();
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

        //disabilita la fisica legata al gameobject
        DisablePhysics();
    }

    void ShockWave()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(this.transform.position, explosionOuterRadius);

        for (int i = 0; i < hitColliders.Length; i++)
        {

            GameObject objectHit = hitColliders[i].gameObject;
            Rigidbody2D rigidbodyHit = objectHit.GetComponent<Rigidbody2D>();

            /*
            //calcola la spinta proporzionalmente rispetto alla distanza dalla bomba
            float thrust = minExplosionForce + (explosionOuterRadius - Vector3.Distance(objectHit.transform.position, this.transform.position)) / (explosionOuterRadius - explosionInnerRadius) * maxExplosionForce;

            if (thrust > maxExplosionForce)
                thrust = maxExplosionForce;
            */

            //applica la forza
            if (rigidbodyHit)
            {
                Vector2 direction = rigidbodyHit.worldCenterOfMass - new Vector2(this.transform.position.x - 0.5f, this.transform.position.y - 1.5f);

                float thrust = maxExplosionForce;

                rigidbodyHit.AddForce(direction * thrust);


                //se il player è troppo vicino, uccidilo
                if (Vector2.Distance(objectHit.transform.position, this.transform.position) < explosionInnerRadius && objectHit.tag == "Player")
                    new PlayerDeathByBomb(objectHit.GetComponent<Player>(), this, this.transform.position, direction, maxExplosionForce).StartDeath();
            }



        }

    }

    protected override void EnablePhysics()
    {
        rb.isKinematic = false;
        coll.enabled = true;

    }

    protected override void DisablePhysics()
    {
        rb.isKinematic = true;
        rb.angularVelocity = 0;
        rb.velocity = Vector2.zero;
        coll.enabled = false;
    }

    void SetVisible(bool value)
    {
        innerRadius.gameObject.SetActive(value);
        outerRadius.gameObject.SetActive(value);
        gameObject.GetComponent<SpriteRenderer>().enabled = value;
    }
}



