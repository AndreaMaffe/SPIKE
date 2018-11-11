using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : ObstacleWithTimer {

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

            //calcola la spinta proporzionalmente rispetto alla distanza dalla bomba
            float thrust = minExplosionForce + (explosionOuterRadius - Vector3.Distance(objectHit.transform.position, this.transform.position)) / (explosionOuterRadius - explosionInnerRadius) * maxExplosionForce;

            if (thrust > maxExplosionForce)
                thrust = maxExplosionForce;

            Debug.Log("Bomb thrust (" + objectHit.gameObject.name + "): " + thrust);

            //calcola il vettore direzione tra a e b come (b - a)
            Vector3 direction = objectHit.transform.position - this.transform.position;

            //applica la forza
            if (objectHit.GetComponent<Rigidbody2D>())
                objectHit.GetComponent<Rigidbody2D>().AddForce(direction * thrust);

            //se il player è troppo vicino, uccidilo
            if (Vector3.Distance(objectHit.transform.position, this.transform.position) < explosionInnerRadius && objectHit.tag == "Player")
                new PlayerDeathByBomb(objectHit.GetComponent<Player>(), this, this.transform.position, direction, maxExplosionForce).StartDeath();
        }

    }

    void SetVisible(bool value)
    {
        innerRadius.gameObject.SetActive(value);
        outerRadius.gameObject.SetActive(value);
        gameObject.GetComponent<SpriteRenderer>().enabled = value;
    }
}



