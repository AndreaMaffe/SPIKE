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
        CreateCircleDraggingCollider();
	    objectToFollow = GameObject.FindGameObjectWithTag("Player");
	    rb = GetComponent<Rigidbody2D>();
	}

    //update apposito per gli ostacoli, usare questo anziché Update().
    protected override void UpdateObstacle () {
	    if (objectToFollow)  //controllo se il Player esiste ancora
	    {
		    //calcola lo spostamento verso il player in base alla distanza da quest'ultimo
		    float deltaXPosition = (objectToFollow.transform.position.x - this.transform.position.x) / deceleration;
		    
		    // evita jitter quando deltaXPosition è vicino a 0
		    if (deltaXPosition < positionOffset && deltaXPosition > -positionOffset)
			    deltaXPosition = 0;

		    //spostamento usando rigidbody2d
		    if (deltaXPosition > 0)
			    rb.velocity = new Vector2(speed, rb.velocity.y);
		    else if (deltaXPosition < 0)
			    rb.velocity = new Vector2(-speed, rb.velocity.y);
		    else
			    rb.velocity = new Vector2(0.0f, rb.velocity.y);
	    }
	}

	//chiamato al RunLevel()
	protected override void WakeUp()
	{
		//permette di entrare nell'UpdateObstacle()
		SetActive(true);

        EnablePhysics();
		
	}

	//chiamato al RetryLevel()
	protected override void Sleep()
	{

		//impedisce di entrare nell'UpdateObstacle()
		SetActive(false);

		//risetta la posizione iniziale
		ResetPosition();

        DisablePhysics();

	}

	public override ObstacleType GetObstacleType()
	{
		return ObstacleType.Bomb;
	}
}
