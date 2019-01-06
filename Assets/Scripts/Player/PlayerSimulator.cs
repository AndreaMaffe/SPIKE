using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSimulator : MonoBehaviour {

    private Rigidbody2D rb;
    private BoxCollider2D mainCollider;

    [Header("Movements")]
    public float speed;
    public float jumpAngle;
    public float jumpStrenght;
    public float jumpDelayTime;
    [SerializeField]
    private PlayerState state;
    private Vector3 originalPosition;
    [SerializeField]
    private bool onGround;
    private TimerManager timerManager;

    //Lista che contiene tutti i timer per i movimenti del giocatore
    private List<Timer> movementTimers;

    public delegate void OnFlagReached();
    public static event OnFlagReached playerSimulatorFlagReached;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCollider = GetComponent<BoxCollider2D>();

        Time.timeScale = 2;

        timerManager = FindObjectOfType<TimerManager>();

        LevelManager.runLevelEvent += StopMovement;
        LevelManager.retryLevelEvent += Sleep;

        originalPosition = this.transform.position;

        movementTimers = new List<Timer>();

        Invoke("WakeUp", 0.5f);

    }

    void FixedUpdate()
    {
        //se tocca una piattaforma
        if (Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y), Vector2.down, 0.1f, LayerMask.GetMask("Platform")))
        {
            //al primo contatto, annulla la velocità e fai reiniziare il Player a correre
            if (!onGround)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                rb.drag = 4;
                if (state == PlayerState.Jumping)
                    Run();
            }
            onGround = true;
        }

        //se è in aria
        else
        {
            rb.drag = 0;
            onGround = false;
        }


        switch (state)
        {
            case PlayerState.Running:
                if (onGround)
                    rb.AddForce(new Vector2(speed / 100, 0), ForceMode2D.Impulse);
                break;

            case PlayerState.WaitingToJump:
                Invoke("Jump", jumpDelayTime);
                break;

            case PlayerState.Jumping:
                break;

            case PlayerState.Stopped:
                break;

            case PlayerState.Exulting:
                rb.velocity = Vector2.zero;
                break;
        }

        if (transform.position.x > 7)
            playerSimulatorFlagReached();
    }

    //chiamato al RunLevel()
    void WakeUp()
    {
        rb = GetComponent<Rigidbody2D>();
        Run();

        SetTimers();

        StartMovementTimers();
    }

    //chiamato al RetryLevel()
    void Sleep()
    {
        gameObject.SetActive(false);

        ResetTimers();
    }

    void Run()
    {
        state = PlayerState.Running;
    }

    void Stop()
    {
        if (onGround)
        {
            rb.velocity = Vector2.zero;
            state = PlayerState.Stopped;
        }

        else Invoke("Stop", 0.1f);

    }

    void WaitAndJump()
    {
        if (onGround)
        {
            rb.velocity = Vector2.zero;
            state = PlayerState.WaitingToJump;
        }
    }

    void Jump()
    {
        if (state == PlayerState.WaitingToJump && onGround)
        {
            state = PlayerState.Jumping;
            rb.AddForce(new Vector2(jumpStrenght * Mathf.Cos(jumpAngle * Mathf.Deg2Rad), jumpStrenght * Mathf.Sin(jumpAngle * Mathf.Deg2Rad)), ForceMode2D.Impulse);
        }
    }

    void Exult()
    {
        state = PlayerState.Exulting;
    }

    //crea e setta i timer collegati ai movimenti
    void SetTimers()
    {
        foreach (MovementData movementData in LevelManager.CurrentLevel.movementDatas)
        {
            Timer timer = timerManager.AddTimer(movementData.startTime);

            switch (movementData.movement.ToString())
            {
                case "Jump":
                    timer.triggeredEvent += WaitAndJump;
                    break;
                case "Run":
                    timer.triggeredEvent += Run;
                    break;
                case "Stop":
                    timer.triggeredEvent += Stop;
                    break;
            }

            movementTimers.Add(timer);
        }
    }

    //blocca tutti i timer e li rimuove
    void ResetTimers()
    {
        foreach (Timer timer in movementTimers)
            timer.Pause();

        movementTimers.Clear();
    }

    //fa partire tutti i timer collegati ai movimenti
    void StartMovementTimers()
    {
        foreach (Timer timer in movementTimers)
        {
            timer.Start();
        }
    }

    private void OnDisable()
    {
        LevelManager.retryLevelEvent -= Sleep;
        LevelManager.runLevelEvent -= StopMovement;
    }

    public void StopMovement()
    {
        Stop();
        rb.velocity = Vector2.zero;
        playerSimulatorFlagReached();
    }

    void SetInvisible()
    {
        this.gameObject.SetActive(false);
    }

    void SetVisible()
    {
        this.gameObject.SetActive(true);
    }

}
