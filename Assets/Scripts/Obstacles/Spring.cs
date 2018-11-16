using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : Obstacle {

    private bool triggered;
    private Transform spikes;
    private PolygonCollider2D coll;
    private BoxCollider2D trigg;

    [Tooltip("Intensity of the push")]
    public float push;
    [Tooltip("Vertical shift of the platform when triggered")]
    public float platformHeight;

    //start apposito per gli ostacoli, usare questo anziché Start().
    protected override void StartObstacle ()
    {
        triggered = false;
        spikes = transform.Find("Spikes");

        coll = GetComponent<PolygonCollider2D>();
        coll.enabled = false;
    }
	
	//update apposito per gli ostacoli, usare questo anziché Update().
	protected override void UpdateObstacle () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!triggered && active)
        {
            //applica una forza verticale di intensità pari a "push" all'oggetto soprastante
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            collision.gameObject.transform.position += new Vector3(0, platformHeight, 0);
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(0, push, 0));

            coll.enabled = true;

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
        SetActive(true);
    }

    //chiamato al RetryLevel()
    protected override void Sleep()
    {
        //risetta la posizione iniziale
        ResetPosition();

        //riaggiusta la posizione delle spine
        if (triggered)
            this.spikes.position -= new Vector3(0, -platformHeight, 0);

        //impedisce di entrare in OnTrigger
        SetActive(false);

        //disabilita il collider fisico della piattaforma
        coll.enabled = false;

        triggered = false;
    }

    public override ObstacleType GetObstacleType()
    {
        return ObstacleType.Spring;
    }
}
