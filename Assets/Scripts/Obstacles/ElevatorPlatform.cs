using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorPlatform : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Platform")
            transform.parent.gameObject.GetComponent<Elevator>().InvertDirection();
    }

}
