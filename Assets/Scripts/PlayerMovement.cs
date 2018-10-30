using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour {

    public Rigidbody2D rb;

    public float acceleration;
    public float maxVelocity;
    public float jumpStrenght;

    public bool onGround;
    //state puo' essere run, jump o stop e adatta animazioni e velocita' di conseguenza
    public string state;

    //variabile che contiene lo scriptable object del livello attuale chiesto al level manager
    private Level currentLevel;
    private LevelManager levelManager;
    private TimerManager timerManager;
    //Lista che contiene tutti i timer per i movimenti del giocatore
    Timer[] movementTimers;

    private void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        timerManager = FindObjectOfType<TimerManager>();
       
        LevelManager.triggeredEvent += StartMovement;
    }
  

    public void StartMovement()
    {
        Debug.Log("Movement started");
        currentLevel = levelManager.GetActualLevel();
        //!!!!!! Attualmente questo metodo viene chiamato troppo presto: prima di LevelManager
        movementTimers = new Timer[currentLevel.movementDatas.Length];
        StartTimersForJumpingAndStopping();

        rb.velocity = new Vector2(maxVelocity, 0);
    }

    public void StopMovement() {
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    public void Jump() {
        if (onGround)
            rb.AddForce(new Vector2(1,1).normalized * jumpStrenght, ForceMode2D.Impulse);
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
                    timer.triggeredEvent += StartMovement;
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
