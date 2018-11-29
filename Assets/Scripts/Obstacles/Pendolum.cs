using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendolum : Obstacle {

    private Rigidbody2D rbBody;

    private BoxCollider2D collBody;
    private BoxCollider2D collBlade;

    public float oscillation;

    //start apposito per gli ostacoli, usare questo anziché Start().
    protected override void StartObstacle()
    {
        rbBody = transform.Find("Body").GetComponent<Rigidbody2D>();

        SetCollidersActive(false); SetDynamicRigidbodyActive(false);
	}

    //update apposito per gli ostacoli, usare questo anziché Update().
    protected override void UpdateObstacle()
    {
    }

    //chiamato al RunLevel()
    protected override void WakeUp()
    {
        SetCollidersActive(true); SetDynamicRigidbodyActive(true);;
        rbBody.AddForce(new Vector3(oscillation, 0, 0));
    }

    //chiamato al RetryLevel()
    protected override void Sleep()
    {
        SetCollidersActive(false); SetDynamicRigidbodyActive(false);;

        //reimposta la posizione e la rotazione del pendolo a quella inziale
        rbBody.transform.localPosition = new Vector3(0, -1.5f, 0);
        rbBody.transform.localRotation = Quaternion.Euler(0, 0, 0);
        transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    public override ObstacleType GetObstacleType()
    {
        return ObstacleType.Pendolum;
    }

    protected override void SetDynamicRigidbodyActive(bool value)
    {
        base.SetDynamicRigidbodyActive(value);

        if (value)
            rbBody.isKinematic = false;
        else
        {
            rbBody.isKinematic = true;
            rbBody.velocity = Vector2.zero;
            rbBody.angularVelocity = 0;
        }
    }
}
