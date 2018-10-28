using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour {

    public Rigidbody2D rb;

    public float horizVelocity;
    public float maxVelocity;
    public float jumpStrenght;

    public bool onGround;

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
    }
    // Update is called once per frame
    void Update () {
        rb.AddForce(new Vector3(horizVelocity, 0, 0), ForceMode2D.Force);
        if (rb.velocity.x >= maxVelocity)
            rb.velocity = new Vector2(maxVelocity, rb.velocity.y);
	}

    public void Jump() {
        if (onGround)
            rb.AddForce(Vector2.up * jumpStrenght, ForceMode2D.Impulse);
    }


    void StartTimersForJumpingAndStopping() {
        //per ogni timer contenuto nello scriptable Object del livello corrente    
        // per ora crea un solo timer che salta dopo un secondo
        Timer jumpTimer = new Timer(1, "jump");
        jumpTimer.TimerEnded += Jump;
        timerManager.AddTimer(jumpTimer);
        jumpTimer.StartTimer();
    }

    //da cambiare e da fare con raycast per evitare collisioni laterali ma per ora va bene anche cosi'
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
            onGround = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
            onGround = false;
    }


}
