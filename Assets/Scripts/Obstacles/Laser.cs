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
    public LineRenderer laser;

    //start apposito per gli ostacoli, usare questo anziché Start().
    protected override void StartObstacle() {

        SetTimer(rateOfFire);

        readyToMove = false;

        objectToFollow = GameObject.FindGameObjectWithTag("Player");
        timerToRecover = FindObjectOfType<TimerManager>().AddTimer(timeToRecover);

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

        RaycastHit2D objectHit = Physics2D.Raycast(this.transform.position, new Vector2(0, -1), 10);

        DrawLaser(objectHit.point);

        readyToMove = false;

        //fa partire un timer al termine del quale readyToMove è rimesso a true e il timer può muoversi di nuovo
        timerToRecover.triggeredEvent += Restart;
        timerToRecover.Start();

    }

    //Metodo che riempie i vertici del line renderer
    void DrawLaser(Vector3 hitPoint) {
        laser.positionCount = 2;
        laser.SetPosition(0, shootingPoint.position);
        laser.SetPosition(1, hitPoint);
    }

    //chiamato quando il laser si è "riposato" ed è pronto a sparare di nuovo
    void Restart()
    {
        readyToMove = true;
        laser.positionCount = 0;
        timerToRecover.triggeredEvent -= Restart; //per evitare bug

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
}
