using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSpikes : ObstacleWithTimer
{
    private Vector3 originalSpikesPosition;
    private GameObject spikes;
    private Rigidbody2D rbSpikes;
    private Rigidbody2D rbPlatform;

    private SpriteRenderer ropeSprite;
    private Vector2 originalRopeSize;

    private bool goingUp;

    [Tooltip("Time on ground before starting rising up")]
    public float timeOnGround;
    [Tooltip("Speed of the vertical ascent")]
    public float liftSpeed;


    bool audioon = false;

    //start apposito per gli ostacoli, usare questo anziché Update().
    protected override void StartObstacle()
    {
        spikes = transform.Find("Spikes").gameObject;
        rbSpikes = spikes.GetComponent<Rigidbody2D>();
        rbPlatform = spikes.transform.Find("Platform").GetComponent<Rigidbody2D>();

        ropeSprite = transform.Find("Pivot").GetComponent<SpriteRenderer>();

        originalRopeSize = ropeSprite.size;

        SetCollidersActive(false); SetDynamicRigidbodyActive(rbSpikes, false); SetDynamicRigidbodyActive(rbPlatform, false);
    }

    //update apposito per gli ostacoli, usare questo anziché Update().
    protected override void UpdateObstacle()
    {
        if (!goingUp)
        {
            //lancia 2 raycast dx e sx
            RaycastHit2D hit1 = Physics2D.Raycast(transform.position + new Vector3(-0.5f, -1, 0), Vector2.down, 10, LayerMask.GetMask("Player", "Raptor"));
            RaycastHit2D hit2 = Physics2D.Raycast(transform.position + new Vector3(0.5f, -1, 0), Vector2.down, 10, LayerMask.GetMask("Player", "Raptor"));

            if (hit1 || hit2)
            {
                SetCollidersActive(true);
                SetDynamicRigidbodyActive(rbSpikes, true);
                SetDynamicRigidbodyActive(rbPlatform, true);

                if (audioon == false)
                {
                    FindObjectOfType<AudioManager>().Play("falling");
                    audioon = true;
                }
            }                              
        }

        if (goingUp)
        {
            audioon = false;

            if (spikes.transform.position.y <= originalSpikesPosition.y)
                spikes.GetComponent<Rigidbody2D>().velocity = new Vector2(0, liftSpeed);

            else
            {
                spikes.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                spikes.GetComponent<Rigidbody2D>().gravityScale = 5;
                goingUp = false;
                SetCollidersActive(false);
                SetDynamicRigidbodyActive(rbSpikes, false);
                SetDynamicRigidbodyActive(rbPlatform, false);
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
        SetCollidersActive(false); SetDynamicRigidbodyActive(rbSpikes, false); SetDynamicRigidbodyActive(rbPlatform, false);

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

    public override ObstacleType GetObstacleType()
    {
        return ObstacleType.FallingSpikes;
    }
}
