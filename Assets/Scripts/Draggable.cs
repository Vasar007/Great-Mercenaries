using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public Transform parentToReturnTo;
    public Transform placeholderParent;

    public bool isDragable;

    public DropZone.PanelState CardPanelState { get; private set; }
    private Vector3 _startPoosition;
    private GameObject _placeholder;
    private bool _notBegin = true;
    private GameObject _itemBeingHandled;

    [SerializeField]
    private GameManager _gameManager;

    
    #region IBeginDragHandler implementation

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isDragable) return;

        _startPoosition  = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x,
                                                                      eventData.position.y, 0.0f))
                           - transform.position;

        _placeholder = new GameObject();
        _placeholder.transform.SetParent(transform.parent);

        var layoutElement = _placeholder.AddComponent<LayoutElement>();
        layoutElement.preferredWidth  = GetComponent<LayoutElement>().preferredWidth;
        layoutElement.preferredHeight = GetComponent<LayoutElement>().preferredHeight;
        layoutElement.flexibleWidth   = 0.0f;
        layoutElement.flexibleHeight  = 0.0f;

        _placeholder.transform.SetSiblingIndex(transform.GetSiblingIndex());

        parentToReturnTo  = transform.parent;
        placeholderParent = parentToReturnTo;
        CardPanelState   = parentToReturnTo.GetComponent<DropZone>().panelIndex;
        transform.SetParent(transform.parent.parent);

        GetComponent<CanvasGroup>().blocksRaycasts = false;

        _notBegin = false;

        _itemBeingHandled = gameObject;
    }
    
    #endregion

    #region IDragHandler implementation

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragable || _notBegin) return;

        //this.transform.position = Camera.main.ScreenToWorldPoint(eventData.position);
        transform.position = SavingDrag(eventData);

        if (_placeholder.transform.parent != placeholderParent)
        {
            _placeholder.transform.SetParent(placeholderParent);
        }

        int newSiblingIndex = placeholderParent.childCount;
        for (int i = 0; i < placeholderParent.childCount; ++i)
        {
            if (transform.position.x < placeholderParent.GetChild(i).position.x)
            {
                newSiblingIndex = i;
                if (_placeholder.transform.GetSiblingIndex() < newSiblingIndex)
                {
                    --newSiblingIndex;
                }

                break;
            }
        }

        _placeholder.transform.SetSiblingIndex(newSiblingIndex);
    }
    
    #endregion

    #region IEndDragHandler implementation

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDragable || _notBegin) return;

        transform.SetParent(parentToReturnTo);
        transform.SetSiblingIndex(_placeholder.transform.GetSiblingIndex());
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        Destroy(_placeholder);

        var dropZone = parentToReturnTo.GetComponent<DropZone>();
        if (dropZone != null && CardPanelState != dropZone.panelIndex
                             && dropZone.IsAvailableZone(dropZone.panelIndex))
        {
            isDragable = false;
            CardPanelState = dropZone.panelIndex;
            _gameManager.battleManager.SetCardForBattle(_itemBeingHandled, true);
            _gameManager.urgentNotify = true;
        }

        _notBegin = true;
        _itemBeingHandled = null;
    }

    #endregion

    private Vector3 SavingDrag(PointerEventData eventData)
    {
        var distanceToScreen = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        var posMove = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x,
                                                                 eventData.position.y,
                                                                 distanceToScreen.z));
        return new Vector3(posMove.x - _startPoosition.x, posMove.y - _startPoosition.y, posMove.z);
    }
}
