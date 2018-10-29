using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

    private Timer timer;
    private bool readyToFire;

    public GameObject objectToFollow;
    public float deceleration;
    public float rateOfFire;
    [Header("Period of inactivity after shooting") ]
    public float timeToRecover;

	// Use this for initialization
	void Start () {

        readyToFire = true;

        //crea il timer nel TimerManager
        timer = FindObjectOfType<TimerManager>().AddTimer(rateOfFire);

        //associa lo scadere del timer al metodo Shoot()
        timer.triggeredEvent += Shoot;
	}
	
	// Update is called once per frame
	void Update () {

        if (readyToFire)
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
                timer.Start();
            }
        }
	}

    void Shoot() {

        //modifica animazione;

        RaycastHit2D objectHit = Physics2D.Raycast(this.transform.position, new Vector2(0, -1), 10);

        Debug.DrawLine(this.transform.position, objectHit.transform.position, Color.yellow, 1);
        Debug.Log(objectHit.collider.gameObject.name);

        //if (objectHit.collider.gameObject.tag == "Player")
            //Destroy(objectHit.collider.gameObject);

        readyToFire = false;

        //fa partire un timer al termine del quale readyToFire è rimesso a true e il timer può muoversi di nuovo
        Timer timerToRecover = FindObjectOfType<TimerManager>().AddTimer(timeToRecover);
        timerToRecover.triggeredEvent += Restart;
        timerToRecover.Start();

    }

    void Restart() {

        readyToFire = true;
    }
}
