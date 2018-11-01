using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Obstacle {

    private Timer timer;

    public float explosionForce;
    public float explosionInnerRadius;
    public float explosionOuterRadius;

    public Transform innerRadius;
    public Transform outerRadius;

    public float timeBeforeExplosion;

    public GameObject explosionParticlePrefab;


    //start apposito per gli ostacoli, usare questo anziché Start().
    protected override void StartObstacle() {

        //scala le sprite dei cerchi
        innerRadius.localScale = new Vector3(innerRadius.localScale.x /2 * explosionInnerRadius, innerRadius.localScale.y /2 * explosionInnerRadius, 1);
        outerRadius.localScale = new Vector3(outerRadius.localScale.x /4 * explosionOuterRadius, outerRadius.localScale.y /4 * explosionOuterRadius, 1);

        //crea il timer e lo associa al metodo Explode()
        timer = FindObjectOfType<TimerManager>().AddTimer(timeBeforeExplosion);
        timer.triggeredEvent += Explode;

    }

    protected override void WakeUp() {

        //avvia il timer
        timer.Start();
    }

    void Explode() {

        //DrawCircle(this.transform.position, explosionInnerRadius);

        //gets all the colliders within the radius of the explosion
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(this.transform.position, explosionOuterRadius);

        for (int i = 0; i < hitColliders.Length; i++) {

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
        GameObject explosionParticleInstance = Instantiate(explosionParticlePrefab, transform.position, Quaternion.identity);
        Destroy(explosionParticleInstance.gameObject, 1.5f);
        //destroy the bomb
        Destroy(this.gameObject);
    }

    //update apposito per gli ostacoli, usare questo anziché Update().
    protected override void UpdateObstacle()
    {
    }

    private void OnDestroy() {
        timer.triggeredEvent -= Explode;
    }
}
