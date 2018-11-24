using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSpikes : ObstacleWithTimer
{
    private Rigidbody2D rb;
    private Vector3 originalSpikesPosition;
    private GameObject spikes;
    private SpriteRenderer ropeSprite;

    private Vector2 originalRopeSize;

    private bool goingUp;

    [Tooltip("Range inside which the Player is detected")]
    public float raycastOffset;
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
        spikes = transform.Find("Spikes").gameObject;
        ropeSprite = transform.Find("Rope").GetComponent<SpriteRenderer>();

        originalRopeSize = ropeSprite.size;

        DisablePhysics();
    }

    //update apposito per gli ostacoli, usare questo anziché Update().
    protected override void UpdateObstacle()
    {
        if (!goingUp)
        {
            //lancia 2 raycast dx e sx
            RaycastHit2D hit1 = Physics2D.Raycast(transform.position + new Vector3(-raycastOffset, 0, 0), Vector2.down, 10, LayerMask.GetMask("Player"));
            RaycastHit2D hit2 = Physics2D.Raycast(transform.position + new Vector3(raycastOffset, 0, 0), Vector2.down, 10, LayerMask.GetMask("Player"));

            if (hit1 || hit2)
                EnablePhysics();
        }

        if (goingUp)
        {
            if (spikes.transform.position.y <= originalSpikesPosition.y)
                spikes.GetComponent<Rigidbody2D>().velocity = new Vector2(0, liftSpeed);

            else
            {
                rigidbodies[0].velocity = new Vector2(0, 0);
                rigidbodies[0].gravityScale = 5;
                goingUp = false;
                DisablePhysics();
            }
        }

        //allunga la corda
        ropeSprite.size = new Vector2(ropeSprite.size.x, -spikes.transform.localPosition.y - 1);

    }

    //chiamato al RunLevel()
    protected override void WakeUp()
    {
        //permette di entrare nell'UpdateObstacle()
        SetActive(true);

        goingUp = false;

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

        //rimette a posto corda e spine
        spikes.transform.position = originalSpikesPosition;
        ropeSprite.size = originalRopeSize;

        goingUp = false;

        ResetTimer();
    }

    public override void OnObstacleDropped()
    {
        base.OnObstacleDropped();
        originalSpikesPosition = spikes.transform.position;
    }

    protected override void OnEndLevel()
    {
        //inizia la risalita
        goingUp = true;
    }

    protected override void OnTimerEnd()
    {
        //inizia la risalita
        goingUp = true;
    }

    //chiamato quando le spikes in caduta collidono con il primo oggetto sottostante
    public void OnSpikesCollision()
    {
        SetTimer(timeOnGround);
        StartTimer();
    }
}
