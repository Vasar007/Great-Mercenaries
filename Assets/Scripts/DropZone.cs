using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public enum PanelState
    {
        PlayerHand = 0,
        PlayerTabletop = 1,
        EnemyTabletop = 2,
        EnemyHand = 3
    }

    public PanelState panelIndex = PanelState.PlayerHand;


    #region IDragHandler implementation

    public void OnDrop(PointerEventData eventData)
    {
        var draggableComp = eventData.pointerDrag.GetComponent<Draggable>();

        if (draggableComp == null) return;
        if (!draggableComp.isDragable || !IsAvailableZone(panelIndex)) return;

        draggableComp.parentToReturnTo = transform;
    }

    #endregion

    #region IPointerEnterHandler implementation

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;

        var draggableComp = eventData.pointerDrag.GetComponent<Draggable>();

        if (draggableComp == null) return;
        if (!draggableComp.isDragable || !IsAvailableZone(panelIndex)) return;

        draggableComp.placeholderParent = transform;
    }

    #endregion

    #region IPointerExitHandler implementation

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;

        var draggableComp = eventData.pointerDrag.GetComponent<Draggable>();

        if (draggableComp == null) return;
        if (!draggableComp.isDragable || !IsAvailableZone(panelIndex)) return;

        if (draggableComp.placeholderParent == transform)
        {
            draggableComp.placeholderParent = draggableComp.parentToReturnTo;
        }
    }

    #endregion

    public bool IsAvailableZone(PanelState panelState)
    {
        switch (panelState)
        {
            case PanelState.PlayerTabletop:
                return true;

            case PanelState.PlayerHand:
            case PanelState.EnemyTabletop:
            case PanelState.EnemyHand:
                return false;

            default:
                throw new ArgumentException("Do not handle case with " + panelIndex);
        }
    }
}
