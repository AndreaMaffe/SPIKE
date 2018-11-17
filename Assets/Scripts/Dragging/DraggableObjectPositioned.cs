using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//classe che serve per un obstacle gia' posizionato per essere ancora draggable
public class DraggableObjectPositioned : DraggableObject
{
    public delegate void OnReaddObstacle(ObstacleType type);
    public static event OnReaddObstacle onReaddObstacle;

    protected override void Start()
    {
        draggableObstacleOffsetFromFinger = new Vector3(0, 0.5f, 0);
    }


    public override void OnBeginDrag(PointerEventData eventData)
    {
        obstacleDragged = this.GetComponent<DraggableObjectPositionUpdater>();
        //obstacleDragged.Activate();
        HideObstacleButtons();    
    }

    protected override void UpdateObstacleNumber(int amount)
    {
        if (amount == 1) 
            onReaddObstacle(GetComponent<Obstacle>().GetObstacleType());
    }
}
