using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : Obstacle {

    private bool triggered;

    [Tooltip("Intensity of the push")]
    public float push;

    //start apposito per gli ostacoli, usare questo anziché Start().
    protected override void StartObstacle ()
    {
        triggered = false;
	}
	
	//update apposito per gli ostacoli, usare questo anziché Update().
	protected override void UpdateObstacle () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!triggered)
        {
            //applica una forza verticale di intensità pari a "push"
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(0, push, 0));

            //muovi la sprite
            this.transform.position += new Vector3(0, 1, 0);

            triggered = true;
        }


    }

    //chiamato al RunLevel()
    protected override void WakeUp()
    {
    }

    //chiamato al RetryLevel()
    protected override void Sleep()
    {
        //torna nella posizione iniziale
        if (triggered == true)
            this.transform.position -= new Vector3(0, 1, 0);

        else triggered = false;
    }

    public override ObstacleType GetObstacleType()
    {
        return ObstacleType.Spring;
    }
}
