﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Jumping,
    Stopped,
    Running,
    WaitingToJump,
    Exulting
}

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private BoxCollider2D mainCollider;
    private Transform body;

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
    private bool outOfTheLevel;
    private bool invincible;

    [Header("Ragdoll")]
    public GameObject bloodParticle;
    public Rigidbody2D[] RagdollPieces;
    public Collider2D[] RagdollColliders;
    public GameObject[] RagdollArts;

    private TimerManager timerManager;

    //Lista che contiene tutti i timer per i movimenti del giocatore
    private List<Timer> movementTimers;
    private Timer timerBeforeExulting;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        mainCollider = GetComponent<BoxCollider2D>();
        body = transform.Find("Body");

        timerManager = FindObjectOfType<TimerManager>();

        LevelManager.runLevelEvent += WakeUp;
        LevelManager.retryLevelEvent += Sleep;
        LevelManager.endLevelEvent += OnEndLevel;

        originalPosition = this.transform.position;

        movementTimers = new List<Timer>();
    }

    void FixedUpdate()
    {
        //se tocca una piattaforma
        if (Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y), Vector2.down, 0.1f , LayerMask.GetMask("Platform", "ElevatorPlatform","SpringPlatform"))  || Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y), new Vector2(-2, -1), 0.1f, LayerMask.GetMask("Platform", "ElevatorPlatform", "SpringPlatform")))
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
                    rb.AddForce(new Vector2(speed/100, 0), ForceMode2D.Impulse);
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

        if (Mathf.Abs(transform.position.x) > 12 && !outOfTheLevel &&!invincible)
        {
            LevelManager.StartFailureEvent("OUT OF THE LEVEL!");
            outOfTheLevel = true;
        }

    }

    //chiamato al RunLevel()
    void WakeUp()
    {
        Run();

        SetTimers();

        StartMovementTimers();

        //distrugge lo spruzzo di sangue
        GetComponent<PlayerAppearence>().DestroyBloodFountainParticle();
        //riattiva l'ombra
        transform.Find("OmbraPlayer").gameObject.SetActive(true);
        Time.timeScale = 1;

        invincible = false;

    }

    //chiamato al RetryLevel()
    void Sleep()
    {
        //riassesta il corpo in caso di morte
        SetActiveRagdoll(false);

        ResetAnimatorTriggers();

        Stop();

        //risetta il Player immobile e alla posizione iniziale
        gameObject.transform.position = originalPosition;
        rb.velocity = Vector2.zero;

        //rimuovi tutti i timer (verranno risettati al successivo WakeUp())
        ResetTimers();

        outOfTheLevel = false;
    }

    //chiamato al EndLevel()
    void OnEndLevel()
    {
        timerBeforeExulting.Start();
        invincible = true;
    }

    void Run()
    {
        ResetAnimatorTriggers();
        state = PlayerState.Running;
        animator.SetTrigger("Move");
    }

    void Stop()
    {
        if (onGround)
        {
            ResetAnimatorTriggers();
            rb.velocity = Vector2.zero;
            state = PlayerState.Stopped;
            animator.SetTrigger("Stop");
        }

        else Invoke("Stop", 0.1f);

    }

    void WaitAndJump()
    {
        if (onGround)
        {
            ResetAnimatorTriggers();
            rb.velocity = Vector2.zero;
            state = PlayerState.WaitingToJump;
            animator.SetTrigger("WaitingJump");
        }
    }

    void Jump()
    {
        if (state == PlayerState.WaitingToJump && onGround)
        {
            ResetAnimatorTriggers();
            state = PlayerState.Jumping;
            animator.SetTrigger("Jump");
            rb.AddForce(new Vector2(jumpStrenght * Mathf.Cos(jumpAngle * Mathf.Deg2Rad), jumpStrenght * Mathf.Sin(jumpAngle * Mathf.Deg2Rad)), ForceMode2D.Impulse);
        }
    }

    void Exult()
    {
        ResetAnimatorTriggers();
        state = PlayerState.Exulting;
        animator.SetTrigger("Exult");
    }

    void ResetAnimatorTriggers()
    {
        animator.ResetTrigger("Jump");
        animator.ResetTrigger("Move");
        animator.ResetTrigger("WaitingJump");
        animator.ResetTrigger("Stop");
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

        timerBeforeExulting = timerManager.AddTimer(0.7f);
        timerBeforeExulting.triggeredEvent += Exult;
    }

    //blocca tutti i timer e li rimuove
    void ResetTimers()
    {
        foreach (Timer timer in movementTimers)
            timer.Pause();

        movementTimers.Clear();

        if(timerBeforeExulting != null)
            timerBeforeExulting.Pause();
    }

    //fa partire tutti i timer collegati ai movimenti
    void StartMovementTimers()
    {
        foreach (Timer timer in movementTimers)
        {
            timer.Start();
        }
    }

    //da cambiare e da fare con raycast per evitare collisioni laterali ma per ora va bene anche cosi'
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Deadly" && !invincible)
        {
            //crea l'evento PlayerDeathEvent nel punto corrispondente al contatto e avvialo
            PlayerDeathEvent playerDeathEvent = collision.transform.root.GetComponent<Obstacle>().CreatePlayerDeathEvent(this, collision.GetContact(0).point);
            playerDeathEvent.StartDeath();
        }
    }

    public void SetActiveRagdoll(bool value)
    {
        transform.Find("OmbraPlayer").gameObject.SetActive(false);
        animator.enabled = !value;
        rb.simulated = !value;
        mainCollider.enabled = !value;

        foreach (Rigidbody2D rb in RagdollPieces)
        {
            rb.simulated = value;
        }

        foreach (Collider2D col in RagdollColliders)
        {
            col.enabled = value;
        }

        foreach (GameObject art in RagdollArts)
        {
            if (value)
                art.transform.parent = null;
            else art.transform.parent = body;
        }
    }

    //metodo che applica al corpo della ragdoll una forza in una certa direzione 
    public void ApplyRagdollImpulse(float amount, Vector2 direction)
    {
        RagdollPieces[0].AddForce(direction * amount);
    }


    private void OnDisable()
    {
        LevelManager.runLevelEvent -= WakeUp;
        LevelManager.retryLevelEvent -= Sleep;
        LevelManager.endLevelEvent -= OnEndLevel;
    } 

}