using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorPlatform : MonoBehaviour {

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.name == this.gameObject.name)
            transform.parent.gameObject.GetComponent<Elevator>().InvertDirection();
    }
}
