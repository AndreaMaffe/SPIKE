using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSpikes : ObstacleWithTimer
{
    private Rigidbody2D rb;
    private bool goingUp;

    public float raycastOffset;
    public SpriteRenderer gancio;
    public GameObject spikes;

    float offsetGancioFromSpikes;

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
        DisablePhysics();
        offsetGancioFromSpikes = transform.position.y - spikes.transform.position.y;
        Debug.Log(offsetGancioFromSpikes);
    }

    //update apposito per gli ostacoli, usare questo anziché Update().
    protected override void UpdateObstacle()
    {
        if (!goingUp)
        {
            //lancia 2 raycast dx e sx
            RaycastHit2D hit1 = Physics2D.Raycast(transform.position - new Vector3(-raycastOffset,0,0), Vector2.down, 10, LayerMask.GetMask("Player"));
            RaycastHit2D hit2 = Physics2D.Raycast(transform.position + new Vector3(raycastOffset, 0, 0), Vector2.down, 10, LayerMask.GetMask("Player"));

            if (hit1 || hit2)           
                EnablePhysics();

            UpdateSprite();
        }

        if (goingUp)
        {
            DisablePhysics();
            transform.position += new Vector3(0, liftSpeed, 0);
            if (transform.position.y >= originalPosition.y)
            {
                transform.position = originalPosition;
                goingUp = false;
            }
        }
    }
    private void UpdateSprite()
    {
        //gancio.size = new Vector2(gancio.size.x, )
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
        DisablePhysics();

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

        if (!goingUp)
        {
            SetTimer(timeOnGround);
            StartTimer();
        }
    }






}
