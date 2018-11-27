using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : Obstacle
{
    public float speed;

    private GameObject platform;
    private Animator animator;

    private Vector3 direction;

    //start apposito per gli ostacoli, usare questo anziché Start().
    protected override void StartObstacle()
    {
        platform = transform.Find("Platform").gameObject;
        animator = GetComponent<Animator>();

        direction = Vector3.down;

        DisablePhysics();
    }

    //update apposito per gli ostacoli, usare questo anziché Update().
    protected override void UpdateObstacle()
    {
        platform.transform.position += direction * speed;
    }

    //chiamato al RunLevel()
    protected override void WakeUp()
    {
        //permette di entrare nell'UpdateObstacle()
        SetActive(true);

        //attiva la motosega
        animator.SetBool("go", true);

        EnablePhysics();

    }

    //chiamato al RetryLevel()
    protected override void Sleep()
    {
        //impedisce di entrare nell'UpdateObstacle()
        SetActive(false);

        //risetta la posizione iniziale
        animator.SetBool("go", false);

        DisablePhysics();
    }

    public void InvertDirection()
    {
        if (direction == Vector3.up)
            direction = Vector3.down;
        else direction = Vector3.up;
    }

    public override ObstacleType GetObstacleType()
    {
        return ObstacleType.Spring;
    }


}