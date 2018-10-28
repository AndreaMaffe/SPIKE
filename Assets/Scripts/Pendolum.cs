using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendolum : MonoBehaviour {

    private Rigidbody2D rb;

    public float oscillation;

	// Use this for initialization
	void Start () {

        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector3(oscillation, 0, 0));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
