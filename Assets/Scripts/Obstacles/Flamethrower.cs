using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : Obstacle {

    public ParticleSystem flameParticles;

    public float flameCloseRange;
    public float flameFarRange;
    public float push;

    private float direction;
    private int layerMaskHit;


    public override ObstacleType GetObstacleType()
    {
        return ObstacleType.Flamethrower;
    }

    protected override void StartObstacle()
    {
        layerMaskHit = LayerMask.GetMask("Player", "Raptor", "Bomb", "Spike", "Pendolum");
    }

    protected override void UpdateObstacle()
    {
        try
        {
            Rigidbody2D rigidbodyHit = Physics2D.Raycast(this.transform.position, new Vector2(direction, 0), flameFarRange, layerMaskHit).rigidbody;

            rigidbodyHit.AddForce(push/100 * new Vector2(direction, 0), ForceMode2D.Impulse);

            if (rigidbodyHit.gameObject.tag == "Player")
            {
                float distance = (rigidbodyHit.position - new Vector2(this.transform.position.x, this.transform.position.y)).magnitude;

                if (distance < flameCloseRange)
                    CreatePlayerDeathEvent(rigidbodyHit.gameObject.GetComponent<Player>(), rigidbodyHit.position).StartDeath();
            }
        }
        catch (System.NullReferenceException e) { }

            
    }

    protected override void WakeUp()
    {
        //permette di entrare nell'UpdateObstacle()
        SetActive(true);

        flameParticles.Play();
    }

    protected override void Sleep()
    {
        //impedisce di entrare nell'UpdateObstacle()
        SetActive(false);

        flameParticles.Stop();
    }

    public override void OnObstacleDropped()
    {
        //1 se sx-->dx , -1 se sx
        direction = -this.transform.position.x / Mathf.Abs(this.transform.position.x);

        //inverti specularmente in base alla direzione
        this.transform.localScale = new Vector3(direction,1,1);
    }

}
