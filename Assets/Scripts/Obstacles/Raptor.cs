using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raptor : Obstacle {
	
	private GameObject objectToFollow;
	private Rigidbody2D rb;
	public float deceleration;
	public float speed; // velocità del raptor
	public float positionOffset;

    //start apposito per gli ostacoli, usare questo anziché Start().
    protected override void StartObstacle()
    {
	    objectToFollow = GameObject.FindGameObjectWithTag("Player");
	    //rb = GetComponent<Rigidbody2D>();

	    DisablePhysics();

        rb = rigidbodies[0];
	}

    //update apposito per gli ostacoli, usare questo anziché Update().
    protected override void UpdateObstacle ()
    {
        //calcola lo spostamento verso il player in base alla distanza da quest'ultimo
        float deltaXPosition = (objectToFollow.transform.position.x - this.transform.position.x) / deceleration;

        // evita jitter quando deltaXPosition è vicino a 0
        if (deltaXPosition < positionOffset && deltaXPosition > -positionOffset)
            deltaXPosition = 0;

        //spostamento usando rigidbody2d
        if (deltaXPosition > 0)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            this.transform.rotation = new Quaternion(0, 180, 0, 1);
        }
        else if (deltaXPosition < 0)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            this.transform.rotation = Quaternion.identity;
        }

        else
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
    }

	//chiamato al RunLevel()
	protected override void WakeUp()
	{

        GetComponent<Animator>().enabled = true;
        transform.Find("RaptorDavanti").transform.Find("Fumo").GetComponent<ParticleSystem>().Play();
		//permette di entrare nell'UpdateObstacle()
		SetActive(true);

        EnablePhysics();
		
	}

	//chiamato al RetryLevel()
	protected override void Sleep()
	{

        GetComponent<Animator>().enabled = false;
        transform.Find("RaptorDavanti").transform.Find("Fumo").GetComponent<ParticleSystem>().Stop();
        
        //impedisce di entrare nell'UpdateObstacle()
        SetActive(false);

		//risetta la posizione iniziale
		ResetPosition();

        DisablePhysics();

	}

	public override ObstacleType GetObstacleType()
	{
		return ObstacleType.Raptor;
	}
}
