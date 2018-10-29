using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

    private Timer timer;

    public GameObject objectToFollow;
    public float deceleration;
    public float rateOfFire;

	// Use this for initialization
	void Start () {

        //crea il timer nel TimerManager
        timer = FindObjectOfType<TimerManager>().AddTimer(rateOfFire);

        //associa lo scadere del timer al metodo Shoot()
        timer.triggeredEvent += Shoot;
	}
	
	// Update is called once per frame
	void Update () {

        //calcola lo spostamento verso il player in base alla distanza da quest'ultimo
        float deltaXPosition = (objectToFollow.transform.position.x - this.transform.position.x) / deceleration;

        //setta lo spostamento massimo per ogni frame per evitare che il laser sembri muoversi a scatti
        if (deltaXPosition > 0.4f)
            deltaXPosition = 0.4f;
        if (deltaXPosition < -0.4f)
            deltaXPosition = -0.4f;

        this.transform.position = new Vector3(this.transform.position.x + deltaXPosition, this.transform.position.y, this.transform.position.z);

        //se il laser è allineato, avvia il conto alla rovescia per lo sparo
        if (Mathf.Abs(objectToFollow.transform.position.x - this.transform.position.x) < 0.05)
        {
            timer.Start();
        }
	}

    void Shoot() {

        //modifica animazione;


    }
}
