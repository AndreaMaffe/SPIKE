using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : Obstacle
{

    public Animator animator;

    //start apposito per gli ostacoli, usare questo anziché Start().
    protected override void StartObstacle()
    {
        CreateCircleDraggingCollider();

        DisablePhysics();
    }

    //update apposito per gli ostacoli, usare questo anziché Update().
    protected override void UpdateObstacle()
    {
        
    }

    //chiamato al RunLevel()
    protected override void WakeUp()
    {
        //permette di entrare nell'UpdateObstacle()
        SetActive(true);
        animator.SetBool("go", true);
    }

    //chiamato al RetryLevel()
    protected override void Sleep()
    {
        //impedisce di entrare nell'UpdateObstacle()
        SetActive(false);

        //risetta la posizione iniziale
        animator.SetBool("go", false);
        ResetPosition();
    }

    public override ObstacleType GetObstacleType()
    {
        return ObstacleType.Spring;
    }
}