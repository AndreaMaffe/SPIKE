using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorRangeSpawner : MonoBehaviour {

    //lo starting point deve stare sotto o a sinistra 
    public Transform startingPoint;
    public Transform endingPoint;

    [Header("allineamento dei punti di ancoraggio")]
    public Alignment alignment;
    [Header("Top, Side o Platform")]
    public AnchorPointPosition position;

    // Use this for initialization
    void Start () {
        //disegna la linea al punto finale 
        startingPoint.GetComponent<LineRenderer>().SetPosition(1, endingPoint.localPosition);
        //adatta il collider 
        BoxCollider2D collider = startingPoint.GetComponent<BoxCollider2D>();
        if (alignment == Alignment.Vertical)
        {
            collider.size = new Vector2(0.8f, endingPoint.transform.localPosition.y);
            collider.offset = new Vector2(0, collider.size.y / 2);
        }
        else if (alignment == Alignment.Horizontal) {
            collider.size = new Vector2(endingPoint.transform.localPosition.x , 0.8f );
            collider.offset = new Vector2(collider.size.x / 2, 0);
        }
      
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
