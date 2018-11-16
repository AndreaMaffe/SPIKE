using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : Obstacle {

    private bool triggered;
    private Transform spikes;
    private BoxCollider2D collider;

    [Tooltip("Intensity of the push")]
    public float push;
    [Tooltip("Vertical shift of the platform when triggered")]
    public float platformHeight;

    //start apposito per gli ostacoli, usare questo anziché Start().
    protected override void StartObstacle ()
    {
        triggered = false;
        spikes = transform.Find("Spikes");
        collider = GetComponent<BoxCollider2D>();
        DisablePhysics();
	}
	
	//update apposito per gli ostacoli, usare questo anziché Update().
	protected override void UpdateObstacle () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!triggered)
        {
            //applica una forza verticale di intensità pari a "push" all'oggetto soprastante
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            collision.gameObject.transform.position += new Vector3(0, platformHeight, 0);
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(0, push, 0));

            collider.isTrigger = false;

            //muovi la sprite
            this.transform.position += new Vector3(0, platformHeight, 0);
            //riaggiusta la posizione delle spine
            this.spikes.position -= new Vector3(0, platformHeight, 0);

            triggered = true;
        }
    }

    //chiamato al RunLevel()
    protected override void WakeUp()
    {
        EnablePhysics();
    }

    //chiamato al RetryLevel()
    protected override void Sleep()
    {
        //risetta la posizione iniziale
        ResetPosition();

        //riaggiusta la posizione delle spine
        if (triggered)
            this.spikes.position -= new Vector3(0, -platformHeight, 0);

        triggered = false;

        collider.isTrigger = true;

        DisablePhysics();
    }

    public override ObstacleType GetObstacleType()
    {
        return ObstacleType.Spring;
    }
}
