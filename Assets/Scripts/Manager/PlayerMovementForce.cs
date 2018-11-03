using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementForce : MonoBehaviour {

    public Rigidbody2D rb;
    public float gridUnitDimension;

    public float maxVelocity;
    public float jumpAngle;
    public float jumpStrenght;
    public float jumpDelayTime;
    [SerializeField]
    private MovementType state;

    public bool onGround;

    //variabile che contiene lo scriptable object del livello attuale chiesto al level manager
    private Level currentLevel;
    private LevelManager levelManager;
    private TimerManager timerManager;
    //Lista che contiene tutti i timer per i movimenti del giocatore
    Timer[] movementTimers;

    Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        levelManager = FindObjectOfType<LevelManager>();
        timerManager = FindObjectOfType<TimerManager>();
        StartMovement();
    }

    public void StartMovement()
     {
         currentLevel = levelManager.GetActualLevel();
         movementTimers = new Timer[currentLevel.movementDatas.Length];
         StartTimersForJumpingAndStopping();
     }

    private void FixedUpdate()
    {
        if (state == MovementType.Move)
            Move();
        else if (state == MovementType.WaitingForJump) {
            WaitForJump();
        }
        else if (state == MovementType.Stop)
            Stop();

    }

    public void Move()
    {
        rb.velocity = new Vector2(maxVelocity, rb.velocity.y);
    }

    public void Stop()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    public void WaitForJump()
    {
        if (onGround) {
            Invoke("ApplyJumpImpulse", jumpDelayTime);
            state = MovementType.Jump;
            Stop();
        }
        else
            SetMove();
    }

    void ApplyJumpImpulse() {
        if (onGround)
            rb.AddForce(new Vector2(jumpStrenght * gridUnitDimension * Mathf.Cos(jumpAngle * Mathf.Deg2Rad), jumpStrenght * gridUnitDimension * Mathf.Sin(jumpAngle * Mathf.Deg2Rad)), ForceMode2D.Impulse);
    }

    void SetMove() {
        state = MovementType.Move;
        Debug.Log("Move:" + transform.position);
    }

    void SetJump()
    {
        state = MovementType.WaitingForJump;
        Debug.Log("Jump:" + transform.position);
    }

    void SetStop() {
        state = MovementType.Stop;
        Debug.Log("Stop:" + transform.position);

    }


    void StartTimersForJumpingAndStopping()
    {
        //per ogni timer contenuto nello scriptable Object del livello corrente    
        // per ora crea un solo timer che salta dopo un secondo

        int i = 0;
        foreach (MovementData movementData in currentLevel.movementDatas)
        {
            Timer timer = timerManager.AddTimer(movementData.startTime);
            switch (movementData.type.ToString())
            {
                case "Jump":
                    timer.triggeredEvent += SetJump;
                    break;
                case "Move":
                    timer.triggeredEvent += SetMove;
                    break;
                case "Stop":
                    timer.triggeredEvent += SetStop;
                    break;
                default:
                    Debug.Log("ERROR: action name not valid, check scriptable object of the level " + currentLevel + " or PlayerMovement.cs");
                    break;
            }
            movementTimers[i] = timer;
            i += 1;
        }
        //Da spostare all'interno di un metodo che viene chiamato nel momento in cui si preme il tasto play
        foreach (Timer timer in movementTimers)
        {
            timer.Start();
        }

    }

    //da cambiare e da fare con raycast per evitare collisioni laterali ma per ora va bene anche cosi'
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform") {
            onGround = true;
            SetMove();
        }

        if (collision.gameObject.tag == "Deadly")
            Destroy(this.gameObject);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
            onGround = false;
    }



}
