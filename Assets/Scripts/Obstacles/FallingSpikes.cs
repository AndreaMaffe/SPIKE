using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSpikes : ObstacleWithTimer
{
    private Rigidbody2D rb;
    [SerializeField]
    private bool goingUp;
    public bool colliso;

    public float raycastOffset;
    public SpriteRenderer gancio;

    Vector3 originalPernoPosition;

    public float offsetGancioFromSpikes = 1;

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
        //rb = GetComponent<Rigidbody2D>();
        DisablePhysics();
        originalPosition = deadlyGameObject.transform.position;
        originalPernoPosition = transform.position;
    }

    //update apposito per gli ostacoli, usare questo anziché Update().
    protected override void UpdateObstacle()
    {
        if (!goingUp)
        {
            //lancia 2 raycast dx e sx
            RaycastHit2D hit1 = Physics2D.Raycast(transform.position + new Vector3(-raycastOffset,0,0), Vector2.down, 10, LayerMask.GetMask("Player"));
            RaycastHit2D hit2 = Physics2D.Raycast(transform.position + new Vector3(raycastOffset, 0, 0), Vector2.down, 10, LayerMask.GetMask("Player"));

            if (hit1 || hit2)           
                EnablePhysics();
        }

        if (goingUp)
        {
            Debug.Log(deadlyGameObject.transform.position.y);

            if (deadlyGameObject.transform.position.y <= originalPosition.y)
            {
                Debug.Log("Sono nel check");
                rigidbodies[0].velocity = new Vector2(0, liftSpeed);

            }
            else {
                rigidbodies[0].velocity = new Vector2(0, 0);
                rigidbodies[0].gravityScale = 5;
                goingUp = false;
                colliso = false;

            }
        }

        UpdateSprite();

    }
    private void UpdateSprite()
    {
        gancio.size = new Vector2(gancio.size.x, -deadlyGameObject.transform.localPosition.y - offsetGancioFromSpikes);
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

        ResetTimer();
    }

    protected override void ResetPosition() {
        this.transform.position = originalPernoPosition;
        deadlyGameObject.transform.position = originalPosition;
        this.transform.rotation = Quaternion.Euler(0,0,0);
        UpdateSprite();
        goingUp = false;
        colliso = false;
    }

    protected override void OnTimerEnd()
    {
        //inizia la risalita
        goingUp = true;
        Debug.Log("TimerScaduto");
    }

    public void Collided()
    {
        //colliso viene settato a false nell'altro script
        if (!colliso && !goingUp) {
            rigidbodies[0].gravityScale = 0;
            SetTimer(timeOnGround);
            StartTimer();
        }
    }






}
