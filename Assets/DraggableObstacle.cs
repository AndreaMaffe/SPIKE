using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableObstacle : MonoBehaviour {

    public SpriteRenderer spriteRenderer;
    public List<GameObject> anchorPointsCollider;
    public AnchorPointPosition position;
    bool snapped;
    public float removeSnapDistance;

    Vector3 positionBeforeSnapping;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public bool CheckIfSnapped() {
        if (snapped)
            return true;
        else return false;
    }

    

    public void UpdatePosition(Vector3 newPosition) {
        if (!snapped)
            transform.position = newPosition;
        else if (snapped)
            if (Vector3.Distance(newPosition, positionBeforeSnapping) >= removeSnapDistance) {
                snapped = false;
                transform.position = newPosition;
            }
    }

    /*void ChangeSpriteOpacity() {
        if (snapped)
            spriteRenderer
    }*/


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PuntoAncoraggio") {
            if (position == collision.GetComponent<AnchorPoint>().GetPosition()) {
                Debug.Log("Ho colliso col punto di ancoraggio giusto");
                anchorPointsCollider.Add(collision.gameObject);
                positionBeforeSnapping = transform.position;
                snapped = true;
                //ChangeSpriteOpacity();
                transform.position = collision.gameObject.transform.position;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PuntoAncoraggio") {
            if (position == collision.GetComponent<AnchorPoint>().GetPosition())
                anchorPointsCollider.Remove(collision.gameObject);
        }
    }
}
