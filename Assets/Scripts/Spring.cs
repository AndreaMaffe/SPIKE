using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : Obstacle {

    private bool triggered;

    [Tooltip("Intensity of the push")]
    public float push;

	void Start () {
        triggered = false;
	}
	
	//update apposito per gli ostacoli, usare questo anziché Update().
	void UpdateObstacle () {
		
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
