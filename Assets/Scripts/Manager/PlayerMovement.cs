﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{


    public Rigidbody2D rb;
    public float gridUnitDimension;

    public float maxVelocity;
    public float jumpAngle;
    [SerializeField]
    private PlayerState state;

    public bool onGround;

    //variabile che contiene lo scriptable object del livello attuale chiesto al level manager
    private Level currentLevel;
    private LevelManager levelManager;
    private TimerManager timerManager;
    //Lista che contiene tutti i timer per i movimenti del giocatore
    //Timer[] movementTimers;

    public List<PlayerPatternMovement> allMovementsOfLevel;
    [SerializeField]
    PlayerPatternMovement currentMovement; 
    Vector2 positionWhenMovementStarted;
    int movementIndex = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        levelManager = FindObjectOfType<LevelManager>();
        timerManager = FindObjectOfType<TimerManager>();
        PickNextMovement();
    }

    void PickNextMovement() {
        if (movementIndex < allMovementsOfLevel.Count) {
            currentMovement = allMovementsOfLevel[movementIndex];
            state = currentMovement.movement;
        }
    }


    /* public void StartMovement()
     {
         Debug.Log("Movement started");
         currentLevel = levelManager.GetActualLevel();
         movementTimers = new Timer[currentLevel.movementDatas.Length];
         StartTimersForJumpingAndStopping();
     }*/

    private void Update()
    {
        // finche' la distanza da percorrere e' ancora piu' piccola della distanza che deve essere fatta vai avanti col movimento 
        if (currentMovement.lengthInUnit <= Vector2.Distance(positionWhenMovementStarted, transform.position))
        {
            if (state == PlayerState.Run)
                if (rb.velocity.x <= maxVelocity)
                    Move();

                else if (state == PlayerState.Jump)
                {
                    Jump();
                    state = PlayerState.Run;
                }
                else if (state == PlayerState.Stop)
                    Stop();
        }
        else {
            movementIndex++;
            PickNextMovement();
        }
       
    }

    public void Move()
    {
        rb.velocity = new Vector2(maxVelocity, rb.velocity.y);
    }

    public void Stop()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    public void Jump()
    {
        if (onGround)
            rb.velocity = new Vector2(maxVelocity * Mathf.Cos(jumpAngle), maxVelocity * Mathf.Sin(jumpAngle));
    }


    /*
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
        foreach (Timer timer in movementTimers)
        {
            timer.Start();
        }

    }*/

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

public enum PlayerState { Stop, Run, Jump };

[System.Serializable]
public struct PlayerPatternMovement {
    public PlayerState movement;
    public int lengthInUnit;
}

