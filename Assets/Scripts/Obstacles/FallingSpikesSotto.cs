using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSpikesSotto : MonoBehaviour {

    private void OnCollisionEnter2D(Collision2D collision)
    {
        transform.parent.gameObject.GetComponent<FallingSpikes>().OnSpikesCollision();
    }

}
