using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableObstacleRange : MonoBehaviour
{

    public SpriteRenderer spriteRenderer;
    public GameObject anchorPointSnapped;
    public AnchorPointPosition position;
    bool snapped = false;
    public float removeSnapDistance;

    Vector3 positionBeforeSnapping;

    int layerMask;

    private void Start()
    {
        layerMask = LayerMask.GetMask("PuntoAncoraggio"); ;
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
        Debug.Log("Ho colpito");
        Debug.Log(position);

        //spara un raycast nella posizione leggermente sopra al tocco del dito
        RaycastHit2D hit = Physics2D.Raycast(newPosition, Vector2.zero, 10000, layerMask);

        //se sono sopra un punto di ancoraggio
        if (hit.collider != null)
        {
            if (position == hit.collider.GetComponent<AnchorRangeSpawner>().GetPosition())
            {
                //aggiungi l'ancor point alla reference
                //anchorPointSnapped = hit.collider.gameObject;
                snapped = true;
                //cambia l'opacita' della sprite
                ChangeSpriteOpacity();
                //snappa alla posizione del agmeobject colpito
                if (hit.collider.GetComponent<AnchorRangeSpawner>().GetAlignment() == Alignment.Horizontal)
                    transform.position = new Vector3(newPosition.x, hit.collider.gameObject.transform.position.y, 0);
                else
                    transform.position = new Vector3(hit.collider.gameObject.transform.position.x, newPosition.y, 0);

            }
        }

        else
        {
            snapped = false;
            ChangeSpriteOpacity();
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

}
