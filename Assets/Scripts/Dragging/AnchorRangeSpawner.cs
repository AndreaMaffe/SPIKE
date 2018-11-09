using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Alignment { Horizontal, Vertical };
public enum AnchorPointPosition { Top, Side, Platform };

public class AnchorRangeSpawner : MonoBehaviour {

    //lo starting point deve stare sotto o a sinistra 
    public Transform startingPoint;
    public Transform endingPoint;

    [Header("allineamento dei punti di ancoraggio")]
    public Alignment alignment;
    [Header("Top, Side o Platform")]
    public AnchorPointPosition position;

    BoxCollider2D boxCollider;
    LineRenderer line;

    // Use this for initialization
    void Start () {
        DraggableObject.onUpdateAnchorPoint += UpdateAnchorRangeVisual;
        LevelManager.runLevelEvent += HideAnchorRange;
        DraggableObject.onEndDraggingObstacle += HideAnchorRange;       

        boxCollider = startingPoint.GetComponent<BoxCollider2D>();
        line = startingPoint.GetComponent<LineRenderer>();
        HideAnchorRange();

    }

    void UpdateAnchorRangeVisual(AnchorPointPosition pos) {
        if (pos == position)
            DrawAnchorRange();
        else
            HideAnchorRange();
    }

    void HideAnchorRange() {
        line.enabled = false;
        startingPoint.GetComponent<SpriteRenderer>().enabled = false;
        endingPoint.GetComponent<SpriteRenderer>().enabled = false;
    }

    void DrawAnchorRange()
    {
        line.enabled = true;
        startingPoint.GetComponent<SpriteRenderer>().enabled = true;
        endingPoint.GetComponent<SpriteRenderer>().enabled = true;
        line.SetPosition(1, endingPoint.localPosition);
        //adatta il collider 
        boxCollider.enabled = true;
        if (alignment == Alignment.Vertical)
        {
            boxCollider.size = new Vector2(0.8f, endingPoint.transform.localPosition.y);
            boxCollider.offset = new Vector2(0, boxCollider.size.y / 2);
        }
        else if (alignment == Alignment.Horizontal)
        {
            boxCollider.size = new Vector2(endingPoint.transform.localPosition.x, 0.8f);
            boxCollider.offset = new Vector2(boxCollider.size.x / 2, 0);
        }
    }

    public AnchorPointPosition GetPosition()  {
        return position;
    }

    public Alignment GetAlignment() {
        return alignment;
    }

    private void OnDisable()
    {
        DraggableObject.onUpdateAnchorPoint -= UpdateAnchorRangeVisual;
        LevelManager.runLevelEvent -= HideAnchorRange;
        DraggableObject.onEndDraggingObstacle -= HideAnchorRange;
    }


}
