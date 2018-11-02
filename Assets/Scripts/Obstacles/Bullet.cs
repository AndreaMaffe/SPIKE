using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float speed;

    private Rigidbody2D rb;
    private float direction;

    // Use this for initialization
    void Start ()
    { 
        rb = GetComponent<Rigidbody2D>();

        // 1 se sx-->dx , -1 se sx<--dx
        direction = Mathf.Cos(transform.eulerAngles.y);

	}
	
	// Update is called once per frame
	void Update ()
    {
        rb.velocity = new Vector3(speed*direction, 0, 0);
	}
}
