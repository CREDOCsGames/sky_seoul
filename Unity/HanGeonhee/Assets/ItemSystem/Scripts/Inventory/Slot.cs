using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler
{
    private Transform saveTransform;

    public void SetCountText(TextMeshProUGUI counter, int num)
    {
        if (num == -1)
        {
            counter.text = null;
            return;
        }

        counter.text = num.ToString();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (!Task())
            return;

        DragObject.dragGameObject.transform.SetParent(transform);

        DragObject.dragGameObject.transform.localPosition = Vector2.zero;
    }

    public virtual bool Task()
    {
        if (transform.childCount <= 0)
            return true;

        SwapItem(transform.childCount);

        return true;
    }

    public void SwapItem(int childCount)
    {
        saveTransform = transform.GetChild(childCount - 1);

        saveTransform.SetParent(DragObject.dragGameObject.GetComponent<DragObject>().startSlot);
        saveTransform.localPosition = Vector2.zero;
    }
}
