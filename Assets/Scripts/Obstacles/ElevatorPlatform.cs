using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorPlatform : MonoBehaviour {

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Platform")
            transform.parent.gameObject.GetComponent<Elevator>().InvertDirection();

        if (collision.collider.gameObject.name == "Player(Clone)")
            Debug.Log("CIAONE");
    }

}
