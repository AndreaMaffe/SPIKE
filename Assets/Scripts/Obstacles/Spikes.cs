using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : Obstacle
{
    private Rigidbody2D rb; 

    public override ObstacleType GetObstacleType()
    {
        return ObstacleType.Spikes;
    }

    protected override void StartObstacle()
    {
        rb = GetComponent<Rigidbody2D>(); 
        SetCollidersActive(false); SetDynamicRigidbodyActive(rb, false);
    }

    protected override void UpdateObstacle()
    {

    }

    protected override void WakeUp()
    {
        SetCollidersActive(true); SetDynamicRigidbodyActive(rb, true);;
    }

    protected override void Sleep()
    {
        SetCollidersActive(false); SetDynamicRigidbodyActive(rb, false);;

        ResetPosition();
    }





}
