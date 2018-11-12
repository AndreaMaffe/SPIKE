using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fallingspikes : ObstacleWithTimer
{
    private Rigidbody2D rb;
    [SerializeField]
    private bool goingUp;
    private Vector3 originalSpikesPosition;
    private GameObject spikes;

    private bool colliso;
    private SpriteRenderer ropeSprite;

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
        spikes = transform.Find("spikes").gameObject;
        ropeSprite = transform.Find("Rope").GetComponent<SpriteRenderer>();

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
                colliso = false;

            }
        }

        UpdateropeSprite();

    }

    //chiamato al RunLevel()
    protected override void WakeUp()
    {
        //permette di entrare nell'UpdateObstacle()
        SetActive(true);

        goingUp = false;
        colliso = false;

        rigidbodies[0].gravityScale = 5;
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

        goingUp = false;
        colliso = false;

        ResetTimer();
    }

    public override void OnObstacleDropped()
    {
        base.OnObstacleDropped();
        originalSpikesPosition = transform.Find("Spikes").transform.position;
    }

    protected override void ResetPosition()
    {
        base.ResetPosition();
        UpdateropeSprite();
        transform.Find("spikes").transform.position = originalSpikesPosition;
    }

    private void UpdateropeSprite()
    {
        ropeSprite.size = new Vector2(ropeSprite.size.x, -spikes.transform.localPosition.y - 1);
    }

    protected override void OnTimerEnd()
    {
        //inizia la risalita
        goingUp = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collided();
    }

    public void Collided()
    {
        //colliso viene settato a false nell'altro script
        if (!colliso && !goingUp)
        {
            rigidbodies[0].gravityScale = 0;
            SetTimer(timeOnGround);
            StartTimer();
        }
    }






}
