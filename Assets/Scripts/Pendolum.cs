using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendolum : Obstacle {

    private Rigidbody2D rb;

    public float oscillation;

    //start apposito per gli ostacoli, usare questo anziché Start().
    protected override void StartObstacle() {

	}

    //update apposito per gli ostacoli, usare questo anziché Update().
    protected override void UpdateObstacle()
    {
    }

    protected override void WakeUp()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector3(oscillation, 0, 0));
    }
}
