using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : Obstacle
{

    public Animator animator;

    //start apposito per gli ostacoli, usare questo anziché Start().
    protected override void StartObstacle()
    {

    }

    //update apposito per gli ostacoli, usare questo anziché Update().
    protected override void UpdateObstacle()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 10, LayerMask.GetMask("Player"));
        if (hit.collider != null) {
            animator.SetTrigger("Down");
        }
    }

    //chiamato al RunLevel()
    protected override void WakeUp()
    {
        //permette di entrare nell'UpdateObstacle()
        SetActive(true);
    }

    //chiamato al RetryLevel()
    protected override void Sleep()
    {
        //impedisce di entrare nell'UpdateObstacle()
        SetActive(false);

        //risetta la posizione iniziale
        ResetPosition();
    }

    public override ObstacleType GetObstacleType()
    {
        return ObstacleType.Spring;
    }
}