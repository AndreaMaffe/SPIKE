using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//classe che serve per un obstacle gia' posizionato per essere ancora draggable
public class DraggableObjectPositioned : DraggableObject
{
    public override void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Dragged");
        obstacleDragged = this.GetComponent<DraggableObjectPositionUpdater>();
        //obstacleDragged.Activate();
        HideObstacleButtons();    
    }

    protected override void UpdateObstacleNumber(int amount)
    {
        if (amount == 1) { }
            //AddToObstacleAmount(amount);
    }
}
