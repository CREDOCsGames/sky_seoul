using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler , IPointerEnterHandler
{
    public static GameObject dragGameObject;

    public Transform startSlot;

    private Transform moveTransform;

    private int itemCode;

    public int ItemCode
    {
        set { itemCode = value; }
        get { return itemCode; }
    }

    private void Start()
    {
        startSlot = transform.parent;

        moveTransform = PlayerManager.Instance.Inventory.InvMain;

        gameObject.GetComponent<AspectRatioFitter>().aspectMode = AspectRatioFitter.AspectMode.None;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragGameObject = gameObject;

        dragGameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;

        startSlot = transform.parent;

        dragGameObject.transform.SetParent(moveTransform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        dragGameObject.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragGameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;

        dragGameObject = null;

        if(transform.parent == moveTransform)
        {
            transform.SetParent(startSlot);

            transform.localPosition = Vector2.zero;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("아이템 이름 : " + TextManager.Instance.LoadString("ItemLanguage",itemCode));
    }
}
