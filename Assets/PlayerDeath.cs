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

    public Sprite[] bodySprite;
    public Sprite[] faceSprite;

    public SpriteRenderer bodyRenderer;
    public SpriteRenderer faceRenderer;

    public void ActivateRagdoll(bool active)
    {
        animator.enabled = !active;
        mainRigidbody.simulated = !active;
        mainCollider.enabled = !active;
        foreach (Rigidbody2D rb in RagdollPieces)
        {
            rb.simulated = active;
        }
        foreach (Collider2D col in RagdollColliders)
        {
            col.enabled = active;
        }

        UpdateSprite();
    }

    void UpdateSprite() {
        bodyRenderer.sprite = bodySprite[0];
        faceRenderer.sprite = faceSprite[1];
    }
}
