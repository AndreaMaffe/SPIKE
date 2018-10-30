using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendolum : Obstacle {

    private Rigidbody2D rb;

    public float oscillation;

	// Use this for initialization
	void Start () {

        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector3(oscillation, 0, 0));
	}

    //update apposito per gli ostacoli, usare questo anziché Update().
    protected override void UpdateObstacle()
    {
    }

    public override void WakeUp()
    {
    }
}
