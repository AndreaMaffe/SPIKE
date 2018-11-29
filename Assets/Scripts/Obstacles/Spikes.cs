using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : Obstacle
{
    public override ObstacleType GetObstacleType()
    {
        return ObstacleType.Spikes;
    }

    protected override void StartObstacle()
    {
        SetCollidersActive(false); SetDynamicRigidbodyActive(false);;
    }

    protected override void UpdateObstacle()
    {

    }

    protected override void WakeUp()
    {
        SetCollidersActive(true); SetDynamicRigidbodyActive(true);;
    }

    protected override void Sleep()
    {
        SetCollidersActive(false); SetDynamicRigidbodyActive(false);;

        ResetPosition();
    }





}
