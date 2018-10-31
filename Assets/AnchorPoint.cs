using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnchorPointPosition { Top, Side, Platform };

public class AnchorPoint : MonoBehaviour {
 
    [Header("Position can be: top, side o platform")]
    public AnchorPointPosition position;
    public bool edge;
    [SerializeField]
    private bool occupied;

    public void SetOccupied(bool occupied) {
        this.occupied = occupied;
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
