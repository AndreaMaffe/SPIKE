﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private Rigidbody2D rb;
    private Animator animator;

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

    public GameObject bloodParticle;
    public Rigidbody2D[] RagdollPieces;
    public Collider2D[] RagdollColliders;

    [Header("Il rigidbody e il collider principali")]
    public Rigidbody2D mainRigidbody;
    public BoxCollider2D mainCollider;

    public Sprite[] bodySprite;
    public Sprite[] faceSprite;

    public SpriteRenderer bodyRenderer;
    public SpriteRenderer faceRenderer;

    bool injured = false;

    //variabile che contiene lo scriptable object del livello attuale chiesto al level manager
    private Level currentLevel;
    private LevelManager levelManager;
    private TimerManager timerManager;

    //Lista che contiene tutti i timer per i movimenti del giocatore
    private Timer[] movementTimers;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        levelManager = FindObjectOfType<LevelManager>();
        timerManager = FindObjectOfType<TimerManager>();

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

        rb.velocity = new Vector2(0, 0);
    }

    //chiamato al RetryLevel()
    void Sleep()
    {
        SetActiveRagdoll(false);

        gameObject.transform.position = originalPosition;
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        activateMovements = false;

        foreach (Timer timer in movementTimers)
            timerManager.RemoveTimer(timer);

        //setta l'animazione di Stop
        SetStop();
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
    void ResetPlayerAnimationToDefault() {
        animator.Play("Stop",0);
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
        }
        else
            SetMove();
    }

    void ApplyJumpImpulse() {

        if (onGround && activateMovements) {
            animator.SetTrigger("Jump");
            rb.AddForce(new Vector2(jumpStrenght * gridUnitDimension * Mathf.Cos(jumpAngle * Mathf.Deg2Rad), 
                jumpStrenght * gridUnitDimension * Mathf.Sin(jumpAngle * Mathf.Deg2Rad)), ForceMode2D.Impulse);
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
            originalPosition = this.transform.position;


            if (activateMovements)
            {
                if (state != MovementType.Move)
                    SetMove();
            }
        }

        if (collision.gameObject.tag == "Deadly")
        {
            //crea l'evento PlayerDeathEvent nel punto corrispondente al contatto (N.B: CONTROLLARE SE L'INDICE 0 E' CORRETTO!!)
            PlayerDeathEvent playerDeathEvent = collision.transform.root.GetComponent<Obstacle>().CreatePlayerDeathEvent(this, collision.GetContact(0).point);

            playerDeathEvent.StartDeath();
        }           
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
            onGround = false;
    }

    public void SetActiveRagdoll(bool active)
    {
        if (active)
        {
            injured = true;           
        }

        animator.enabled = !active;
        mainRigidbody.simulated = !active;
        mainCollider.enabled = !active;
        foreach (Rigidbody2D rb in RagdollPieces)
        {
            rb.simulated = active;
        }
        foreach (Collider2D col in RagdollColliders)
        {
            col.enabled = active;
        }
        UpdateSprite();
    }

    //metodo che applica al corpo della ragdoll una forza in una certa direzione 
    public void ApplyRagdollImpulse(float amount, Vector2 direction)
    {
        if (activateMovements)
            RagdollPieces[0].AddForce(direction * amount);
    }

    void UpdateSprite()
    {
        if (injured)
        {
            bodyRenderer.sprite = bodySprite[0];
            faceRenderer.sprite = faceSprite[0];
        }
    }

}