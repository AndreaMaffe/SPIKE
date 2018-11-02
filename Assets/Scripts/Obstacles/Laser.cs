using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : ObstacleWithTimer {

    private bool readyToMove; //true se il laser è pronto a muoversi, false se deve "riposarsi" dopo lo sparo precedente
    private GameObject objectToFollow;
    private Timer timerToRecover; 

    [Tooltip("Period of inactivity after shooting") ]
    public float timeToRecover;
    public float rateOfFire;
    public float deceleration;

    public Transform shootingPoint;
    public GameObject laserStartParticle;
    public GameObject laserLight;

    public LayerMask layer;

    //start apposito per gli ostacoli, usare questo anziché Start().
    protected override void StartObstacle() {

        SetTimer(rateOfFire);

        readyToMove = false;

        objectToFollow = GameObject.FindGameObjectWithTag("Player");
        timerToRecover = FindObjectOfType<TimerManager>().AddTimer(timeToRecover);
        timerToRecover.triggeredEvent += Restart;
    }

    //update apposito per gli ostacoli, usare questo anziché Update().
    protected override void UpdateObstacle ()
    {
        if (readyToMove && objectToFollow)  //controllo se il Player esiste ancora
        {
            //calcola lo spostamento verso il player in base alla distanza da quest'ultimo
            float deltaXPosition = (objectToFollow.transform.position.x - this.transform.position.x) / deceleration;

            //setta lo spostamento massimo per ogni frame per evitare che il laser sembri muoversi a scatti
            if (deltaXPosition > 0.4f)
                deltaXPosition = 0.4f;
            if (deltaXPosition < -0.4f)
                deltaXPosition = -0.4f;

            this.transform.position = new Vector3(this.transform.position.x + deltaXPosition, this.transform.position.y, this.transform.position.z);

            //se il laser è allineato, avvia il conto alla rovescia per lo sparo
            if (Mathf.Abs(objectToFollow.transform.position.x - this.transform.position.x) < 0.1)
            {

                StartTimer();
            }
        }
	}

    protected override void OnTimerEnd()
    {
        Shoot();
    }

    void Shoot() {

        readyToMove = false;
        RaycastHit2D hit = Physics2D.Raycast(shootingPoint.position, Vector2.down, layer);
        Debug.Log(hit.collider.name);

        DrawLaser(hit.point);

        if (hit.collider.gameObject.tag == "Player")
            //Destroy(hit.collider.gameObject, 0.5f); 
        
        //fa partire un timer al termine del quale readyToMove è rimesso a true e il timer può muoversi di nuovo
        timerToRecover.Start();
    }

    //Metodo fa sparare il laser
    void DrawLaser(Vector3 hitPoint) {
        laserStartParticle.GetComponent<ParticleSystem>().Play();
        laserLight.SetActive(true);
        SpriteRenderer renderer = laserLight.GetComponent<SpriteRenderer>();
        renderer.size = new Vector2(renderer.size.x, Vector3.Distance(laserLight.transform.position, hitPoint));
        Debug.Log(hitPoint);
        
    }

    //chiamato quando il laser si è "riposato" ed è pronto a sparare di nuovo
    void Restart()
    {
        laserStartParticle.GetComponent<ParticleSystem>().Stop();
        laserLight.SetActive(false);
        readyToMove = true;
    }

    //chiamato al RunLevel()
    protected override void WakeUp()
    {
        //permette di entrare nell'UpdateObstacle()
        SetActive(true);

        //permette di muoversi
        readyToMove = true;
    }

    //chiamato al RetryLevel()
    protected override void Sleep()
    {
        //impedisce di entrare nell'UpdateObstacle()
        SetActive(false);

        //risetta la posizione iniziale
        ResetPosition();
    }

    private void OnDisable()
    {
        timerToRecover.triggeredEvent -= Restart;
    }
}
