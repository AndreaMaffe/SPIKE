using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DraggableObjectPositionUpdater : MonoBehaviour
{
    public SpriteRenderer[] spriteRenderer;
    [SerializeField]
    bool snapped;
    bool overAnotherObstacle;

    int layerMask;
    int[] obstacleLayerMask;

    private void Start()
    {
        FillObstacleLayerMask();
    }

    public void OnEnable()
    {
        layerMask = LayerMask.GetMask("PuntoAncoraggio");
        spriteRenderer = GetComponentsInChildren<SpriteRenderer>();
        ChangeSpriteOpacity();
    }

    void FillObstacleLayerMask()
    {
        obstacleLayerMask = new int[6];
        obstacleLayerMask[0] = LayerMask.GetMask("Bomb");
        obstacleLayerMask[1] = LayerMask.GetMask("Default");
        obstacleLayerMask[2] = LayerMask.GetMask("FallingSpikes");
        obstacleLayerMask[3] = LayerMask.GetMask("Pendolum");
        obstacleLayerMask[4] = LayerMask.GetMask("Raptor");
        obstacleLayerMask[5] = LayerMask.GetMask("Spikes");
    }

    public bool CheckIfSnapped()
    {
        if (snapped)
            return true;
        else return false;
    }

    public bool CheckIfOverAnotherObstacle() {
        return overAnotherObstacle;
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

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 0.7f);
        bool objectFound = false;

        for (int i = 0; i < hitColliders.Length; i++)
        {
            GameObject objectHit = hitColliders[i].gameObject;

            if (objectHit != gameObject && CheckIfObjectInLayerMask(LayerMask.GetMask(LayerMask.LayerToName(objectHit.layer))))
            {
                overAnotherObstacle = true;
                objectFound = true;
                ChangeSpriteOpacity();
            }
        }

        if (!objectFound) {
            overAnotherObstacle = false;
            ChangeSpriteOpacity();
        }

    }

    void ChangeSpriteOpacity()
    {
        if (snapped)
        {
            if (!overAnotherObstacle)
            {
                foreach (SpriteRenderer spr in spriteRenderer)
                    spr.color = new Color32(255, 255, 255, 255);
            }

            else
            {
                foreach (SpriteRenderer spr in spriteRenderer)
                    spr.color = new Color32(255, 255, 255, 120);
            }       
        }
            
        else if (!snapped)
            foreach (SpriteRenderer spr in spriteRenderer)
                spr.color = new Color32(255, 255, 255, 120);
    }

    bool CheckIfObjectInLayerMask(int layerMask)
    {
        try
        {
            for (int i = 0; i < obstacleLayerMask.Length; i++)
            {
                if (layerMask == obstacleLayerMask[i])
                    return true;
            }
            return false;
        }
        catch (NullReferenceException e){ return false; }
     
    }

}
