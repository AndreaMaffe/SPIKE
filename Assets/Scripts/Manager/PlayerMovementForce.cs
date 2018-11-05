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
    private Vector3 originalPosition;

    public bool onGround;
    public bool activateMovements = false;

    Timer timerInAria;

    PlayerDeath playerDeath;

    //variabile che contiene lo scriptable object del livello attuale chiesto al level manager
    private Level currentLevel;
    private LevelManager levelManager;
    private TimerManager timerManager;

    //Lista che contiene tutti i timer per i movimenti del giocatore
    private Timer[] movementTimers;

    Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerDeath = GetComponent<PlayerDeath>();
        levelManager = FindObjectOfType<LevelManager>();
        timerManager = FindObjectOfType<TimerManager>();
        animator = GetComponent<Animator>();
        LevelManager.runLevelEvent += WakeUp;
        LevelManager.retryLevelEvent += Sleep;
        state = MovementType.Stop;
        originalPosition = this.transform.position;
        currentLevel = levelManager.GetActualLevel();
        movementTimers = new Timer[currentLevel.movementDatas.Length];
    }


    private void OnDisable()
    {
        LevelManager.runLevelEvent -= WakeUp;
        LevelManager.retryLevelEvent -= Sleep;
    }

    //chiamato al RunLevel()
    void WakeUp()
    {
        activateMovements = true;
        StartTimersForJumpingAndStopping();
        ResetPlayerAnimationToDefault();
        
    }


    //chiamato al RetryLevel()
    void Sleep()
    {
        playerDeath.ActivateRagdoll(false);

        SetStop();
        gameObject.transform.position = originalPosition;
        activateMovements = false;
        foreach (Timer timer in movementTimers)
            timer.Pause(); //in questo modo i timer non arriveranno mai a zero
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

    //Riporta l'animator allo stato di stop
    void ResetPlayerAnimationToDefault (){
        animator.ResetTrigger("Jump");
        animator.ResetTrigger("Move");
        animator.ResetTrigger("WaitingJump");
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
            Debug.Log("Waiting to Jump at position: " + transform.position);
        }
        else
            SetMove();
    }

    void ApplyJumpImpulse() {

        if (onGround) {
            animator.SetTrigger("Jump");
            rb.AddForce(new Vector2(jumpStrenght * gridUnitDimension * Mathf.Cos(jumpAngle * Mathf.Deg2Rad), jumpStrenght * gridUnitDimension * Mathf.Sin(jumpAngle * Mathf.Deg2Rad)), ForceMode2D.Impulse);
            timerInAria = timerManager.AddTimer(2);
            timerInAria.Start();
            Debug.Log(rb.velocity.x);
            
        }
    }

    void SetMove() {
        state = MovementType.Move;
        animator.SetTrigger("Move");
    }

    void SetJump()
    {
        state = MovementType.WaitingForJump;
        animator.SetTrigger("WaitingJump");
    }

    void SetStop()
    {
        state = MovementType.Stop;
        animator.SetTrigger("Stop");
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
        if (collision.gameObject.tag == "Platform")
        {
            onGround = true;

            if (activateMovements)
            {    
                if (state!= MovementType.Move)
                    SetMove();
                if (timerInAria != null)
                    Debug.Log("Tempo In Aria: " + (2 - timerInAria.GetTime()));
            }      
        }

        if (collision.gameObject.tag == "Deadly")
            playerDeath.ActivateRagdoll(true);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
            onGround = false;
    }



}
