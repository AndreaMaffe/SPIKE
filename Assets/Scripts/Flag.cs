using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            LevelManager.EndLevel();
        }

        else if (collision.gameObject.tag == "PlayerSimulator") {
            collision.gameObject.GetComponent<PlayerSimulator>().StopMovement();
        }
    }
}
