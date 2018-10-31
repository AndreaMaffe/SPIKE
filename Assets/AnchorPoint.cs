﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnchorPointPosition { Top, Side, Platform };

public class AnchorPoint : MonoBehaviour {
 
    [Header("Position can be: top, side o platform")]
    public AnchorPointPosition position;
    public bool edge;
    [SerializeField]
    private bool occupied;

    public SpriteRenderer spriteRenderer;


    private void Start()
    {
        //sottoscrizione all'evento lanciato dai bottoni per aggiornare la visibilita' degli anchor points
        ObstacleButton.onUpdateAnchorPoint += UpdateAnchorPointSprite;
        spriteRenderer.enabled = false;
    }

    private void OnDisable()
    {
        ObstacleButton.onUpdateAnchorPoint -= UpdateAnchorPointSprite;
    }

    void UpdateAnchorPointSprite(AnchorPointPosition pos)
    {
        if (pos == position && !occupied)
            spriteRenderer.enabled = true;
        else
            spriteRenderer.enabled = false;
    }

    public void SetOccupied(bool occupied) {
        this.occupied = occupied;
        if (occupied)
            spriteRenderer.enabled = false;
    }

    public bool GetOccupied() {
        return occupied;
    }

    public void SetPosition(AnchorPointPosition position) {
        this.position = position; 
    }

    public AnchorPointPosition GetPosition() {
        return position;
    }

    public void SetEdge(bool edge) {
        this.edge = edge;
    }
   

    
}
