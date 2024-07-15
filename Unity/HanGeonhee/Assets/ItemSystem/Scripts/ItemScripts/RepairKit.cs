using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairKit : CoolDownItem
{
    protected override void SetDefault()
    {
        coolDown = 5f;
        canUse = true;
    }

    public override void UseEffect()
    {
        if (!canUse)
            return;

        Debug.Log("로봇을 수리 했습니다.");
    }
}
