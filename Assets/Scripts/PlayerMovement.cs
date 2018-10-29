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

    private void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        timerManager = FindObjectOfType<TimerManager>();
        //currentLevel = levelManager.GetActualLevel();
        StartTimersForJumpingAndStopping();

        //StartMovement();

    }
    // Update is called once per frame
    void Update () {
        if (state == "run") {
            rb.AddForce(new Vector3(acceleration, 0, 0), ForceMode2D.Force);
            if (rb.velocity.x >= maxVelocity)
                rb.velocity = new Vector2(maxVelocity, rb.velocity.y);
        }
       
	}

    public void StartMovement()
    {
        rb.velocity = new Vector2(maxVelocity, rb.velocity.y);
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
        Timer jumpTimer = timerManager.AddTimer(1);
        jumpTimer.triggeredEvent+= Jump;
        jumpTimer.Start();
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
