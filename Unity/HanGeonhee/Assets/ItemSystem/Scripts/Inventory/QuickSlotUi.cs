using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotUi : Slot
{
    public int slotNum;

    public void SetSlotInfo(int code)
    {
        if (code < 0)
        {
            Destroy(transform.GetChild(0).gameObject);
            return;
        }

        var consume = ItemManager.Instance.GetItemScript(code) as StackConsumeItem;

        if(consume != null)
        {
            SetCountText(transform.GetComponentInChildren<TextMeshProUGUI>() , consume.GetHoldNum());
        }
    }

    public void SetQuickSlot(int code)
    {
        if (transform.childCount > 0)
            return;

        QuickSlot.slotItems[slotNum] = ItemManager.Instance.GetItemScript(code);

        PlayerManager.Instance.Inventory.QsItemCreate(code, transform);

        SetSlotInfo(code);
    }

    public void ClearSlot()
    {
        QuickSlot.slotItems[slotNum] = null;
        SetSlotInfo(-1);
    }

    public void CoolDownStart(CoolDownItem obj)
    {
        transform.GetChild(0).GetComponentInChildren<Image>().material = PlayerManager.Instance.GrayShader;

        StartCoroutine(CoolDown(obj));
    }

    private IEnumerator CoolDown(CoolDownItem obj)
    {
        float coolTime = obj.coolDown;

        while (coolTime > 0f)
        {
            coolTime -= Time.deltaTime;

            SetCountText(transform.GetComponentInChildren<TextMeshProUGUI>(), (int)coolTime + 1);

            yield return null;

        }

        SetCountText(transform.GetComponentInChildren<TextMeshProUGUI>(), - 1);

        obj.canUse = true;

        transform.GetChild(0).GetComponentInChildren<Image>().material = null;
    }

    public override bool Task()
    {
        int i = DragObject.dragGameObject.GetComponent<DragObject>().ItemCode;

        string task = ItemManager.Instance.GetItemInfo(i).type;

        if (task != ItemType.Consume.ToString())
            return false;

        if (transform.childCount > 0)
        {
            SwapItem(transform.childCount);
        }

        PlayerManager.Instance.QuickSlotPC.SetQuickSlot(i, slotNum);

        return true;
    }

}
