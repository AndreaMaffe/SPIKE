using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSpikes : ObstacleWithTimer
{
    private Rigidbody2D rb;
    private bool goingUp;

    [Tooltip("Time on ground before starting rising up")]
    public float timeOnGround;
    [Tooltip("Speed of the vertical ascent")]
    public float liftSpeed;

    public override ObstacleType GetObstacleType()
    {
        return ObstacleType.FallingSpikes;
    }

    //start apposito per gli ostacoli, usare questo anziché Update().
    protected override void StartObstacle()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
    }

    //update apposito per gli ostacoli, usare questo anziché Update().
    protected override void UpdateObstacle()
    {
        if (!goingUp)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(-1, -10), 10, LayerMask.GetMask("Player"));
            if (hit)
            {
                rb.isKinematic = false;
            }

            hit = Physics2D.Raycast(transform.position, new Vector2(1, -10), 10, LayerMask.GetMask("Player"));
            if (hit)
            {
                rb.isKinematic = false;
            }
        }

        if (goingUp)
        {
            transform.position += new Vector3(0, liftSpeed, 0);
            if (transform.position.y >= originalPosition.y)
            {
                transform.position = originalPosition;
                rb.velocity = Vector2.zero;
                goingUp = false;
            }

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

        //impedisce di cadere
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;

        //risetta la posizione iniziale
        ResetPosition();

        ResetTimer();
    }

    protected override void OnTimerEnd()
    {
        //inizia la risalita
        goingUp = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        rb.isKinematic = true;

        if (!goingUp)
        {
            SetTimer(timeOnGround);
            StartTimer();
        }
    }






}
