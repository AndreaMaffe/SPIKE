using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendolum : Obstacle {

    private Rigidbody2D rb;

    public float oscillation;

    //start apposito per gli ostacoli, usare questo anziché Start().
    protected override void StartObstacle()
    {
	}

    //update apposito per gli ostacoli, usare questo anziché Update().
    protected override void UpdateObstacle()
    {
    }

    //chiamato al RunLevel()
    protected override void WakeUp()
    {
        rb = transform.Find("Body").GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector3(oscillation, 0, 0));
    }

    //chiamato al RetryLevel()
    protected override void Sleep()
    {
        ResetPosition();
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        rb.rotation = 0;
    }

    public override ObstacleType GetObstacleType()
    {
        return ObstacleType.Pendolum;
    }
}
