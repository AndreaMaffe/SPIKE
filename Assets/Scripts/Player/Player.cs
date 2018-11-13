using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerState
{
    Jumping,
    Stopped,
    Running,
    WaitingToJump
}

public class Player : MonoBehaviour {

    private Rigidbody2D rb;
    private Animator animator;

    public float maxVelocity;
    public float jumpAngle;
    public float jumpStrenght;
    public float jumpDelayTime;
    [SerializeField]
    private PlayerState state;
    private Vector3 originalPosition;

    public bool onGround;

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

    //variabile che contiene lo scriptable object del livello attuale chiesto al level manager
    private Level currentLevel;
    private LevelManager levelManager;
    private TimerManager timerManager;

    //Lista che contiene tutti i timer per i movimenti del giocatore
    private List<Timer> timers;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        levelManager = FindObjectOfType<LevelManager>();
        timerManager = FindObjectOfType<TimerManager>();

        LevelManager.runLevelEvent += WakeUp;
        LevelManager.retryLevelEvent += Sleep;
        state = PlayerState.Stopped;
        originalPosition = this.transform.position;
        currentLevel = levelManager.GetActualLevel();

        timers = new List<Timer>();
    }

    void FixedUpdate()
    {
        switch (state)
        {
            case PlayerState.Running:
                rb.velocity = new Vector2(maxVelocity, rb.velocity.y); ;
                break;

            case PlayerState.WaitingToJump:
                rb.velocity = new Vector2(0, rb.velocity.y);
                break;

            case PlayerState.Stopped:
                rb.velocity = new Vector2(0, rb.velocity.y);
                break;
        }
    }

    //chiamato al RunLevel()
    void WakeUp()
    {
        //ResetPlayerAnimationToDefault();    //<--- SICURI CHE SERVA?

        Run();

        SetTimers();

        StartTimers();
    }

    //chiamato al RetryLevel()
    void Sleep()
    {
        //riassesta il corpo in caso di morte
        SetActiveRagdoll(false);

        Stop();

        //risetta il Player immobile e alla posizione iniziale
        gameObject.transform.position = originalPosition;
        rb.velocity = Vector2.zero;

        //rimuovi tutti i timer (verranno risettati al successivo WakeUp())
        ResetTimers();
    }

    //Riporta l'animator allo stato di stop
    void ResetPlayerAnimationToDefault()
    {
        animator.Play("Stop", 0);
        animator.ResetTrigger("Jump");
        animator.ResetTrigger("Move");
        animator.ResetTrigger("WaitingJump");
    }

    void Run()
    {
        state = PlayerState.Running;
        animator.SetTrigger("Move");
    }

    void Stop()
    {
        state = PlayerState.Stopped;
        animator.SetTrigger("Stop");
    }

    void WaitAndJump()
    {
        if (onGround)
        {
            state = PlayerState.WaitingToJump;
            Invoke("Jump", jumpDelayTime);
            animator.SetTrigger("WaitingJump");
        }
    }

    void Jump()
    {
        if (state == PlayerState.WaitingToJump)
        {
            state = PlayerState.Jumping;
            animator.SetTrigger("Jump");
            rb.AddForce(new Vector2(jumpStrenght * Mathf.Cos(jumpAngle * Mathf.Deg2Rad), jumpStrenght * Mathf.Sin(jumpAngle * Mathf.Deg2Rad)), ForceMode2D.Impulse);
        }
    }

    //crea e setta i timer collegati ai movimenti
    void SetTimers()
    { 
        foreach (MovementData movementData in currentLevel.movementDatas)
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
                default:
                    Debug.Log("ERROR: action name not valid, check scriptable object of the level " + currentLevel + " or PlayerMovement.cs");
                    break;
            }

            timers.Add(timer);
        }
    }

    //blocca tutti i timer e li rimuove
    void ResetTimers()
    {
        foreach (Timer timer in timers)
            timer.Pause();

        timers.Clear();
    }

    //fa partire tutti i timer collegati ai movimenti
    void StartTimers()
    {
        foreach (Timer timer in timers)
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

            //se tocca terra dopo un salto, riprendi a muoverti
            if (state == PlayerState.Jumping)
                Run();
        }

        if (collision.gameObject.tag == "Deadly")
        {
            //crea l'evento PlayerDeathEvent nel punto corrispondente al contatto e avvialo
            PlayerDeathEvent playerDeathEvent = collision.transform.root.GetComponent<Obstacle>().CreatePlayerDeathEvent(this, collision.GetContact(0).point);
            playerDeathEvent.StartDeath();
        }           
    }

    public void SetActiveRagdoll(bool active)
    {
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

        if (active)
        {
            bodyRenderer.sprite = bodySprite[0];
            faceRenderer.sprite = faceSprite[0];
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
    }

}