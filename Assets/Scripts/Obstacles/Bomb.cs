using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Bomb : ObstacleWithTimer {

    private Collider2D coll;
    private Rigidbody2D rb;
    private Canvas canvas;
    private Text timerText;
    private SpriteRenderer img;

    public Sprite disabledBomb;
    public Sprite activatedBomb;

    public float timeBeforeExplosion;
    private float currentTime;
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
        canvas = GetComponentInChildren<Canvas>();
        timerText = canvas.GetComponentInChildren<Text>();
        img = GetComponent<SpriteRenderer>();

        SetCollidersActive(false); SetDynamicRigidbodyActive(rb, false);

    }

    protected override void UpdateObstacle()
    {
        currentTime -= Time.deltaTime;
        if (currentTime < 0)
            timerText.text = "";
        else
            timerText.text = Math.Ceiling(currentTime).ToString();
        if (currentTime - Math.Floor(currentTime) < 0.5)
            img.sprite = activatedBomb;
        else
            img.sprite = disabledBomb;
    }
   
    void Explode()
    {
        FindObjectOfType<AudioManager>().Play("bomb");

        //genera l'onda d'urto 
        ShockWave();

        //crea effetti particellari
        Destroy(Instantiate(explosionParticlePrefab, transform.position, Quaternion.identity), 1.5f);

        //disattiva la Sprite e il timer
        SetVisible(false);
        canvas.enabled = false;

        //disabilita collider e rigidbody
        SetCollidersActive(false); SetDynamicRigidbodyActive(rb, false);
    }

    protected override void OnTimerEnd()
    {
        Explode();
    }

    //chiamato al RunLevel()
    protected override void WakeUp()
    {
        timerText.text = timeBeforeExplosion.ToString();
        currentTime = timeBeforeExplosion;

        //inizializza il timer per l'esplosione
        SetTimer(timeBeforeExplosion);

        //permette di entrare nell'UpdateObstacle()
        SetActive(true);

        //avvia il timer
        StartTimer();
        
        //abilita collider e rigidbody
        SetCollidersActive(true); SetDynamicRigidbodyActive(rb, true);
    }

    //chiamato al RetryLevel()
    protected override void Sleep()
    {
        //rigenera l'immagine e il timer
        SetVisible(true);
        img.sprite = disabledBomb;
        canvas.enabled = true;
        timerText.text = "";

        //impedisce di entrare nell'UpdateObstacle()
        SetActive(false);

        //risetta la posizione iniziale
        ResetPosition();

        //rimette il timer a zero e lo blocca
        ResetTimer();

        //disabilita collider e rigidbody
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
                if (!Physics2D.Raycast(rb.worldCenterOfMass, direction, direction.magnitude, LayerMask.GetMask("Platform")))
                {
                    //applica una spinta all'oggetto pari a explosionThrust
                    rigidbodyHit.AddForce(direction.normalized * explosionThrust /10, ForceMode2D.Impulse);

                    //se il player è troppo vicino, uccidilo
                    if (objectHit.tag == "Player" && direction.magnitude <= explosionInnerRadius)
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



