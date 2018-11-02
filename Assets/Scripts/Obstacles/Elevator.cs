using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : Obstacle
{
    public float speed = 2f;
    public float height = 0.5f;
    public float amplitude;




    //start apposito per gli ostacoli, usare questo anziché Start().
    protected override void StartObstacle()
    {

    }

    //update apposito per gli ostacoli, usare questo anziché Update().
    protected override void UpdateObstacle()
    {
        Vector3 pos = transform.localPosition;
        float newY = amplitude * Mathf.Sin(Time.time * speed);
        transform.localPosition = new Vector3(pos.x, newY, pos.z);
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

}