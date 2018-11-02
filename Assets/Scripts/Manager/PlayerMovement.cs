using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour {

    public enum PlayerState { Stop, Run, Jump };

    public Rigidbody2D rb;

    public float acceleration;
    public float maxVelocity;
    public float jumpStrenght;
    [SerializeField]
    private PlayerState state;

    public bool onGround;
    
    //variabile che contiene lo scriptable object del livello attuale chiesto al level manager
    private Level currentLevel;
    private LevelManager levelManager;
    private TimerManager timerManager;
    //Lista che contiene tutti i timer per i movimenti del giocatore
    Timer[] movementTimers;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        levelManager = FindObjectOfType<LevelManager>();
        timerManager = FindObjectOfType<TimerManager>();
       
        LevelManager.runLevelEvent += StartMovement;
    }
  

    public void StartMovement()
    {
        Debug.Log("Movement started");
        currentLevel = levelManager.GetActualLevel();
        movementTimers = new Timer[currentLevel.movementDatas.Length];
        StartTimersForJumpingAndStopping();
    }

    private void Update()
    {
        if (state == PlayerState.Run) {
            if (rb.velocity.x <= maxVelocity)
                rb.AddForce(Vector2.left * acceleration);
            else
                rb.velocity = new Vector2(maxVelocity, rb.velocity.y);
        }
        if (state == PlayerState.Jump || onGround) {
            Jump();
            state = PlayerState.Run;
        }
    }

    public void Move()
    {
        rb.velocity = new Vector2(maxVelocity, 0);
    }

    public void StopMovement() {
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    public void Jump() {
        if (onGround)
            rb.AddForce(Vector2.up * jumpStrenght, ForceMode2D.Impulse);
    }


    void StartTimersForJumpingAndStopping() {
        //per ogni timer contenuto nello scriptable Object del livello corrente    
        // per ora crea un solo timer che salta dopo un secondo

        int i = 0;
        foreach(MovementData movementData in currentLevel.movementDatas)
        {
            Timer timer = timerManager.AddTimer(movementData.startTime);
            switch (movementData.type.ToString())
            {
                case "Jump":
                    timer.triggeredEvent += Jump;
                    break;
                case "Move":
                    timer.triggeredEvent += Move;
                    break;
                case "Stop":
                    timer.triggeredEvent += StopMovement;
                    break;
                default:
                    Debug.Log("ERROR: action name not valid, check scriptable object of the level " + currentLevel + " or PlayerMovement.cs");
                    break;
            }
            movementTimers[i] = timer;
            i += 1;
        }
        //Da spostare all'interno di un metodo che viene chiamato nel momento in cui si preme il tasto play
        foreach(Timer timer in movementTimers)
        {
            timer.Start();
        }

    }

    //da cambiare e da fare con raycast per evitare collisioni laterali ma per ora va bene anche cosi'
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
            onGround = true;

        if (collision.gameObject.tag == "Deadly")
            Destroy(this.gameObject);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
            onGround = false;
    }


}
