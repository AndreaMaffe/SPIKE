using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour {

    private bool triggered;

    [Tooltip("Intensity of the push")]
    public float push;

	// Use this for initialization
	void Start () {
        triggered = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!triggered)
        {
            //apply vertical force with intensity equal to "push"
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(0, push, 0));

            this.transform.position += new Vector3(0, 1, 0);

            triggered = true;
        }


    }
}
