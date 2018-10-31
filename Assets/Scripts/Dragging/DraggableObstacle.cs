using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableObstacle : MonoBehaviour
{

    public SpriteRenderer spriteRenderer;
    public GameObject anchorPointSnapped;
    public AnchorPointPosition position;
    bool snapped = false;
    public float removeSnapDistance;

    Vector3 positionBeforeSnapping;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ChangeSpriteOpacity();
    }

    public void SetSnappingDesiredPosition(AnchorPointPosition pos)
    {
        position = pos;
    }

    public bool CheckIfSnapped()
    {
        if (snapped)
            return true;
        else return false;
    }

    public void UpdatePosition(Vector3 newPosition)
    {
        if (!snapped)
            transform.position = newPosition;
        else if (snapped)
            if (Vector3.Distance(newPosition, positionBeforeSnapping) >= removeSnapDistance)
            {
                ChangeSpriteOpacity();
                snapped = false;
                transform.position = newPosition;
            }
    }

    void ChangeSpriteOpacity()
    {
        if (snapped)
            spriteRenderer.color = new Color32(255, 255, 255, 255);
        else
            spriteRenderer.color = new Color32(255, 255, 255, 120);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PuntoAncoraggio")
        {
            if (position == collision.GetComponent<AnchorPoint>().GetPosition() && !collision.GetComponent<AnchorPoint>().GetOccupied())
            {
                //aggiungi l'oggetto alla lista di oggetti
                anchorPointSnapped = collision.gameObject;
                //salva la posizione di quando hai snappato
                positionBeforeSnapping = transform.position;
                snapped = true;
                //cambia l'opacita' della sprite
                ChangeSpriteOpacity();
                transform.position = collision.gameObject.transform.position;
            }
        }
    }

}
