using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New HealSelf", menuName = "Inventory/HealSelf", order = 200)]
public class HealSelfInfo : ConsumableInfo
{
    public HealSelfInfo()
    {
        consumableType = ConsumableType.HealSelf;
    }

    public override Item CreateItem(ItemInfo _itemInfo, int count)
    {
        return new HealSelf(_itemInfo, count);
    }
}

public class HealSelf : Item
{
    public HealSelfInfo healSelfInfo;

    public HealSelf(ItemInfo _itemInfo, int count) : base(_itemInfo, count)
    {
        healSelfInfo = _itemInfo as HealSelfInfo;
        if (healSelfInfo == null)
        {
            throw new System.InvalidCastException("HealSelf �����ڿ� ���޵� ItemInfo�� HealSelfInfo Ÿ���̾�� �մϴ�.");
        }
    }

    public void Set()
    {

    }

    public void Unset()
    {

    }
}