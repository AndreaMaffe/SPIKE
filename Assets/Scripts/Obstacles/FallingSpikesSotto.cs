using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSpikesSotto : MonoBehaviour {

    public FallingSpikes fallingSpikes;

    //mi serve perche' lo script non e' attaccato direttamente al game object con le spine ma solo al perno sopra
    // che non puo' rilevare le collisioni perche' non e' un rigidbody
    private void OnCollisionEnter2D(Collision2D collision)
    {
        fallingSpikes.Collided();
        fallingSpikes.colliso = true;
    }
}
