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

    }

    protected override void UpdateObstacle()
    {

    }

    protected override void WakeUp()
    {
        EnablePhysics();
    }

    protected override void Sleep()
    {
        DisablePhysics();

        ResetPosition();
    }





}
