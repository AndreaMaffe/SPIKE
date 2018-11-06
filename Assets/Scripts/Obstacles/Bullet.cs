using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Obstacle {

    public float speed;

    private Rigidbody2D rb;
    private float direction;

    //start apposito per gli ostacoli, usare questo anziché Update().
    protected override void StartObstacle()
    {
        SetActive(true);

        rb = GetComponent<Rigidbody2D>();

        // 1 se sx-->dx , -1 se sx<--dx
        direction = Mathf.Cos(transform.eulerAngles.y);
    }

    //update apposito per gli ostacoli, usare questo anziché Update().
    protected override void UpdateObstacle()
    {
        rb.velocity = new Vector3(speed * direction, 0, 0);
    }

    //chiamato al RunLevel()
    protected override void WakeUp()
    {

    }

    //chiamato al RetryLevel()
    protected override void Sleep()
    {
        Destroy(this.gameObject);
    }

    public override ObstacleType GetObstacleType()
    {
        return ObstacleType.Bullet;
    }

}
