using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour {

    public Rigidbody2D rb;

    public float horizVelocity;
    public float maxVelocity;

	// Update is called once per frame
	void Update () {
        rb.AddForce(new Vector3(horizVelocity, 0, 0), ForceMode2D.Force);
        if (rb.velocity.x >= maxVelocity)
            rb.velocity = new Vector2(maxVelocity, rb.velocity.y);
	}
}
