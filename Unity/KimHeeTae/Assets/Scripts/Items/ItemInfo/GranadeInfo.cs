using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Granade", menuName = "Inventory/Granade", order = 202)]
public class GranadeInfo : ConsumableInfo
{
    public GranadeInfo()
    {
        consumableType = ConsumableType.Granade;
    }

    public override Item CreateItem(ItemInfo _itemInfo, int count)
    {
        return new Granade(_itemInfo, count);
    }
}

public class Granade : Item
{
    public GranadeInfo granadeInfo;

    public Granade(ItemInfo _itemInfo, int count) : base(_itemInfo, count)
    {
        granadeInfo = _itemInfo as GranadeInfo;
        if (granadeInfo == null)
        {
            throw new System.InvalidCastException("Granade �����ڿ� ���޵� ItemInfo�� GranadeInfo Ÿ���̾�� �մϴ�.");
        }
    }

    public void Set()
    {

    }

    public void Unset()
    {

    }
}