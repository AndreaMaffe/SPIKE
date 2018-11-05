using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour {

    public Animator animator;
    public Rigidbody2D[] RagdollPieces;
    public Collider2D[] RagdollColliders;

    [Header("Il rigidbody e il collider principali")]
    public Rigidbody2D mainRigidbody;
    public BoxCollider2D mainCollider;
	
    public void ActivateRagdoll()
    {
        animator.enabled = false;
        mainRigidbody.simulated = false;
        mainCollider.enabled = false;
        foreach (Rigidbody2D rb in RagdollPieces)
        {
            rb.simulated = true;
        }
        foreach (Collider2D col in RagdollColliders)
        {
            col.enabled = true;
        }
    }
}
