using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raptor : Obstacle {
	
	private GameObject objectToFollow;
	public float deceleration;
	public float maxVelocity;

    //start apposito per gli ostacoli, usare questo anziché Start().
    protected override void StartObstacle()
    {
	    objectToFollow = GameObject.FindGameObjectWithTag("Player");
	}

    //update apposito per gli ostacoli, usare questo anziché Update().
    protected override void UpdateObstacle () {
	    if (objectToFollow)  //controllo se il Player esiste ancora
	    {
		    //calcola lo spostamento verso il player in base alla distanza da quest'ultimo
		    float deltaXPosition = (objectToFollow.transform.position.x - this.transform.position.x) / deceleration;

		    //setta lo spostamento massimo per ogni frame per evitare che il laser sembri muoversi a scatti
		    if (deltaXPosition > maxVelocity)
			    deltaXPosition = maxVelocity;
		    if (deltaXPosition < -maxVelocity)
			    deltaXPosition = -maxVelocity;

		    this.transform.position = new Vector3(this.transform.position.x + deltaXPosition, this.transform.position.y, this.transform.position.z);
	    }
	}

	//chiamato al RunLevel()
	protected override void WakeUp()
	{
		//permette di entrare nell'UpdateObstacle()
		SetActive(true);
		
	}

	//chiamato al RetryLevel()
	protected override void Sleep()
	{

		//impedisce di entrare nell'UpdateObstacle()
		SetActive(false);

		//risetta la posizione iniziale
		ResetPosition();
	}

	public override ObstacleType GetObstacleType()
	{
		return ObstacleType.Bomb;
	}
}
