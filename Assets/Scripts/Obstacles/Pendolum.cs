﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendolum : Obstacle {

    private Rigidbody2D rb;
    public GameObject body;

    public float oscillation;

    //start apposito per gli ostacoli, usare questo anziché Start().
    protected override void StartObstacle()
    {
        rb = body.GetComponent<Rigidbody2D>();
        //DisablePhysics();
	}

    //update apposito per gli ostacoli, usare questo anziché Update().
    protected override void UpdateObstacle()
    {
    }

    //chiamato al RunLevel()
    protected override void WakeUp()
    {
        EnablePhysics();
        rb.AddForce(new Vector3(oscillation, 0, 0));
    }

    //chiamato al RetryLevel()
    protected override void Sleep()
    {
        //reimposta la posizione e la rotazione del pendolo a quella inziale
        body.transform.localPosition = new Vector3(0, -1.5f, 0);
        body.transform.localRotation = Quaternion.Euler(0, 0, 0);
        DisablePhysics();
    }

    public override ObstacleType GetObstacleType()
    {
        return ObstacleType.Pendolum;
    }
}
