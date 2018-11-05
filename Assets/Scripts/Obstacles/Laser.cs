using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : ObstacleWithTimer {

    private bool readyToMove; //true se il laser è pronto a muoversi, false se deve "riposarsi" dopo lo sparo precedente
    private GameObject objectToFollow;
    private Timer timerToRecover; 

    [Tooltip("Period of inactivity after shooting") ]
    public float timeToRecover;
    public float timeBeforeShooting;
    public float deceleration;
    public float maxVelocity;

    public Transform shootingPoint;

    public GameObject laserStartParticle;
    public GameObject laserEndParticle;
    //serve un piccolo offset per far in modo che siano allineate con la fine del laser
    public Vector3 laserEndParticleOffsetPosition;
    //quando colpisco il player il laser non deve fermarsi al sopra del player ma attraversarlo (fa un effetto migliore)
    public Vector3 laserLightHitPlayerOffsetPosition;
    public GameObject laserLight;

    //start apposito per gli ostacoli, usare questo anziché Start().
    protected override void StartObstacle()
    {
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
            if (deltaXPosition > maxVelocity)
                deltaXPosition = maxVelocity;
            if (deltaXPosition < -maxVelocity)
                deltaXPosition = -maxVelocity;

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
        RaycastHit2D hit = Physics2D.Raycast(shootingPoint.position, Vector2.down);

        Debug.Log(hit.collider.name);

        DrawLaser(hit.point, hit.collider.gameObject.tag);

        //if (hit.collider.gameObject.tag == "Player")
        //     Destroy(hit.collider.gameObject, 0.5f); 
        
        //fa partire un timer al termine del quale readyToMove è rimesso a true e il timer può muoversi di nuovo
        timerToRecover.Start();
    }

    //Metodo fa sparare il laser
    void DrawLaser(Vector3 hitPoint, string objectHit)
    {         
        //il laser arriva fino in fondo al player
        if (objectHit == "Player") {
            hitPoint += laserLightHitPlayerOffsetPosition;
        }
        ActivateLaserParticle(hitPoint);      
    }

    void ActivateLaserParticle(Vector3 hitPoint)
    {
        SpriteRenderer renderer = laserLight.GetComponent<SpriteRenderer>();
        renderer.size = new Vector2(renderer.size.x, laserLight.transform.position.y - hitPoint.y);
        //attiva tutte le particle
        laserStartParticle.SetActive(true);
        laserEndParticle.SetActive(true);
        laserStartParticle.GetComponent<ParticleSystem>().Play();
        laserLight.SetActive(true);
        laserEndParticle.transform.position = hitPoint + laserEndParticleOffsetPosition;
        laserEndParticle.GetComponent<ParticleSystem>().Play();
    }

    void DeactivateLaserParticle()
    {
        laserStartParticle.GetComponent<ParticleSystem>().Stop();
        laserEndParticle.GetComponent<ParticleSystem>().Stop();
        laserStartParticle.SetActive(false);
        laserEndParticle.SetActive(false);
        laserLight.SetActive(false);
    }

    //chiamato quando il laser si è "riposato" ed è pronto a sparare di nuovo
    void Restart()
    {
        DeactivateLaserParticle();   
        readyToMove = true;
    }

    //chiamato al RunLevel()
    protected override void WakeUp()
    {
        //permette di entrare nell'UpdateObstacle()
        SetActive(true);

        //permette di muoversi
        readyToMove = true;

        //risetta il timer
        SetTimer(timeBeforeShooting);
    }

    //chiamato al RetryLevel()
    protected override void Sleep()
    {
        //cancella la scia del laser in caso stesse sparando
        DeactivateLaserParticle();

        //impedisce di entrare nell'UpdateObstacle()
        SetActive(false);

        //risetta la posizione iniziale
        ResetPosition();

        //rimette il timer a zero e lo blocca
        ResetTimer();
    }

    private void OnDisable()
    {
        timerToRecover.triggeredEvent -= Restart;
        ResetTimer();
    }

    public override ObstacleType GetObstacleType()
    {
        return ObstacleType.Laser;
    }
}
