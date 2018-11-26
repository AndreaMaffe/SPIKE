using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DraggableObjectPositionUpdater : MonoBehaviour
{
    public SpriteRenderer[] spriteRenderer;
    [SerializeField]
    bool snapped;

    int layerMask;
    int obstacleLayerMask;

    public void OnEnable()
    {
        layerMask = LayerMask.GetMask("PuntoAncoraggio");
        obstacleLayerMask = LayerMask.GetMask("Bomb", "Default", "FallingSpikes", "Pendolum","Raptor","Spikes");
        spriteRenderer = GetComponentsInChildren<SpriteRenderer>();
        ChangeSpriteOpacity();
    }

    public bool CheckIfSnapped()
    {
        if (snapped)
            return true;
        else return false;
    }

    public bool CheckIfOverAnotherObstacle()
    {

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero, 10000, obstacleLayerMask);

        if (hit.collider != null && hit.collider.gameObject != this.gameObject) {
            Debug.Log(hit.collider.name);
            return true;

        }

        else return false;
    }

    public void UpdatePosition(Vector3 newPosition)
    {

        //spara un raycast nella posizione leggermente sopra al tocco del dito
        RaycastHit2D hit = Physics2D.Raycast(newPosition, Vector2.zero, 10000, layerMask);

        //se sono sopra un punto di ancoraggio
        if (hit.collider != null)
        {
            //prendo la posizione in cui l'ostacolo puo' andare
            AnchorPointPosition anchorPosition = GetComponent<Obstacle>().anchorPosition;

            if (anchorPosition == hit.collider.GetComponent<AnchorRangeSpawner>().GetPosition() )
            {
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
            foreach(SpriteRenderer spr in spriteRenderer )
                spr.color = new Color32(255, 255, 255, 255);
        else
            foreach (SpriteRenderer spr in spriteRenderer)
                spr.color = new Color32(255, 255, 255, 120);
    }

}
