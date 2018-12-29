using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Obstacle {

    public float speed;

    private Rigidbody2D rb;
    private float direction;    // 1 se sx-->dx , -1 se sx<--dx


    protected override void EnableDraggingSystem() { }
    //start apposito per gli ostacoli, usare questo anziché Update().
    protected override void StartObstacle()
    {
        SetActive(true);

        rb = GetComponent<Rigidbody2D>();

        direction = Mathf.Cos(transform.eulerAngles.y);
    }

    //update apposito per gli ostacoli, usare questo anziché Update().
    protected override void UpdateObstacle()
    {
        rb.velocity = new Vector3(speed * direction, 0, 0);

        if (Mathf.Abs(this.transform.position.x) > 20)
            Destroy(this.gameObject);
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
