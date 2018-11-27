using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : Obstacle
{
    private bool triggered;
    private Transform spikes;
    private PolygonCollider2D coll;
    private BoxCollider2D trigg;

    [Tooltip("Intensity of the push")]
    public float push;
    [Tooltip("Vertical shift of the platform when triggered")]
    public float platformHeight;

    //start apposito per gli ostacoli, usare questo anziché Start().
    protected override void StartObstacle()
    {
        triggered = false;
        spikes = transform.Find("Spikes");

        coll = GetComponent<PolygonCollider2D>();
        coll.enabled = false;
    }

    public override void OnObstacleDropped()
    {
        base.OnObstacleDropped();


        //sposta le spine sotto alla prima piattaforma disponibile
        if (Physics2D.Raycast(this.transform.position + Vector3.up, Vector2.up, 10, LayerMask.GetMask("Platform")))
            spikes.transform.position = new Vector3(spikes.transform.position.x, Physics2D.Raycast(this.transform.position + Vector3.up, Vector2.up, 10, LayerMask.GetMask("Platform")).collider.gameObject.transform.position.y - 0.5f, 0);
        else
            spikes.transform.position = new Vector3(spikes.transform.position.x, GameObject.Find("AstaMetallo").transform.position.y - 1.25f, 0);
    }

    //update apposito per gli ostacoli, usare questo anziché Update().
    protected override void UpdateObstacle()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!triggered && active)
        {
            //alza la piattaforma e applica una forza verticale di intensità pari a "push" all'oggetto soprastante
            collision.gameObject.transform.position += new Vector3(0, platformHeight, 0);
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(0, push, 0));

            coll.enabled = true;

            //muovi la sprite
            this.transform.position += new Vector3(0, platformHeight, 0);
            //riaggiusta la posizione delle spine
            spikes.position -= new Vector3(0, platformHeight, 0);

            triggered = true;

            FindObjectOfType<AudioManagerBR>().Play("spring");
        }
    }

    //chiamato al RunLevel()
    protected override void WakeUp()
    {
        SetActive(true);
    }

    //chiamato al RetryLevel()
    protected override void Sleep()
    {
        //risetta la posizione iniziale
        ResetPosition();

        //riaggiusta la posizione delle spine
        if (triggered)
            this.spikes.position -= new Vector3(0, -platformHeight, 0);

        //impedisce di entrare in OnTrigger
        SetActive(false);

        //disabilita il collider fisico della piattaforma
        coll.enabled = false;

        triggered = false;
    }

    public override ObstacleType GetObstacleType()
    {
        return ObstacleType.Spring;
    }
}