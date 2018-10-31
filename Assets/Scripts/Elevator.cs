using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : Obstacle
{
    public float speed = 5f;
    public float height = 0.5f;


    //start apposito per gli ostacoli, usare questo anziché Start().
    protected override void StartObstacle()
    {

    }

    //update apposito per gli ostacoli, usare questo anziché Update().
    protected override void UpdateObstacle()
    {
        Vector3 pos = transform.position;
        float newY = Mathf.Sin(Time.time * speed);
        transform.position = new Vector3(pos.x, newY, pos.z) * height;
    }

    protected override void WakeUp()
    {
    }
}