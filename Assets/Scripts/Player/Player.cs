using System.Collections;
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

    [Header("Ragdoll")]
    public GameObject bloodParticle;
    public Rigidbody2D[] RagdollPieces;
    public Collider2D[] RagdollColliders;
    public GameObject[] RagdollArts;

    //variabile che contiene lo scriptable object del livello attuale chiesto al level manager
    private Level currentLevel;
    private LevelManager levelManager;
    private TimerManager timerManager;

    //Lista che contiene tutti i timer per i movimenti del giocatore
    private List<Timer> timers;
    private Timer timerBeforeExulting;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        mainCollider = GetComponent<BoxCollider2D>();
        body = transform.Find("Body");

        levelManager = FindObjectOfType<LevelManager>();
        timerManager = FindObjectOfType<TimerManager>();

        LevelManager.runLevelEvent += WakeUp;
        LevelManager.retryLevelEvent += Sleep;
        LevelManager.endLevelEvent += OnEndLevel;

        originalPosition = this.transform.position;
        currentLevel = levelManager.GetActualLevel();

        timers = new List<Timer>();
        timerBeforeExulting = timerManager.AddTimer(0.8f);
        timerBeforeExulting.triggeredEvent += Exult;
    }

    void FixedUpdate()
    {
        //se tocca una piattaforma
        if (Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y), Vector2.down, 0.1f , LayerMask.GetMask("Platform")))
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

        else
        {
            rb.drag = 0;
            onGround = false;
        }
            

        switch (state)
        {
            case PlayerState.Running:
                if (onGround)
                    rb.AddForce(new Vector2(speed, 0));
                break;

            case PlayerState.WaitingToJump:
                Invoke("Jump", jumpDelayTime);
                break;

            case PlayerState.Jumping:
                break;

            case PlayerState.Stopped:
                rb.velocity = new Vector2(0, rb.velocity.y);
                break;

            case PlayerState.Exulting:
                rb.velocity = Vector2.zero;
                break;
        }
    }

    //chiamato al RunLevel()
    void WakeUp()
    {
        Run();

        SetTimers();
        timerBeforeExulting = timerManager.AddTimer(0.8f);
        timerBeforeExulting.triggeredEvent += Exult;

        StartTimers();

        //distrugge lo spruzzo di sangue
        GetComponent<PlayerAppearence>().DestroyBloodFountainParticle();
        //riattiva l'ombra
        transform.Find("OmbraPlayer").gameObject.SetActive(true);

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
        timerBeforeExulting.Pause();
    }

    //chiamato al EndLevel()
    void OnEndLevel()
    {
        timerBeforeExulting.Start();
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
            rb.velocity = Vector2.zero;
            state = PlayerState.WaitingToJump;
            animator.SetTrigger("WaitingJump");
        }
    }

    void Jump()
    {
        if (state == PlayerState.WaitingToJump && onGround)
        {
            state = PlayerState.Jumping;
            animator.SetTrigger("Jump");
            rb.AddForce(new Vector2(jumpStrenght * Mathf.Cos(jumpAngle * Mathf.Deg2Rad), jumpStrenght * Mathf.Sin(jumpAngle * Mathf.Deg2Rad)), ForceMode2D.Impulse);
        }
    }

    void Exult()
    {
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
        if (collision.gameObject.tag == "Deadly")
        {
            //crea l'evento PlayerDeathEvent nel punto corrispondente al contatto e avvialo
            PlayerDeathEvent playerDeathEvent = collision.transform.root.GetComponent<Obstacle>().CreatePlayerDeathEvent(this, collision.GetContact(0).point);
            playerDeathEvent.StartDeath();

            FindObjectOfType<AudioManagerBR>().GetComponent<AudioManager>().PlayFailAudio();
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
        timerBeforeExulting.triggeredEvent -= Exult;
    } 

}