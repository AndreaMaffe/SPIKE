using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : ObstacleWithTimer {

    private Collider2D coll;
    private Rigidbody2D rb;

    public float timeBeforeExplosion;
    public float explosionThrust;
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

        SetCollidersActive(false); SetDynamicRigidbodyActive(rb, false);

    }

    protected override void UpdateObstacle()
    {

    }
   
    void Explode()
    {
        FindObjectOfType<AudioManagerBR>().Play("bomb");

        //genera l'onda d'urto 
        ShockWave();

        //crea effetti particellari
        Destroy(Instantiate(explosionParticlePrefab, transform.position, Quaternion.identity), 1.5f);

        //disattiva la Sprite
        SetVisible(false);

        //disabilita la fisica legata al gameobject
        SetCollidersActive(false); SetDynamicRigidbodyActive(rb, false);
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
        SetCollidersActive(true); SetDynamicRigidbodyActive(rb, true);;
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
        SetCollidersActive(false); SetDynamicRigidbodyActive(rb, false);
    }

    void ShockWave()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(this.transform.position, explosionOuterRadius);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            GameObject objectHit = hitColliders[i].gameObject;
            Rigidbody2D rigidbodyHit = objectHit.GetComponent<Rigidbody2D>();

            if (rigidbodyHit && objectHit!=this.gameObject)
            {
                Vector2 direction = rigidbodyHit.worldCenterOfMass - rb.worldCenterOfMass;               

                //se non si interpongono oggetti tra la bomba e l'oggetto
                if (Physics2D.RaycastAll(rb.worldCenterOfMass, direction)[1].collider.gameObject.name == objectHit.name || Physics2D.RaycastAll(rb.worldCenterOfMass, direction)[0].collider.gameObject.name == objectHit.name)
                {
                    //applica una spinta all'oggetto pari a explosionThrust
                    rigidbodyHit.AddForce(direction.normalized * explosionThrust /10, ForceMode2D.Impulse);

                    //se il player è troppo vicino, uccidilo
                    if (direction.magnitude <= explosionInnerRadius && objectHit.tag == "Player")
                        new PlayerDeathByBomb(objectHit.GetComponent<Player>(), this, this.transform.position, direction, explosionThrust).StartDeath();
                }
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



