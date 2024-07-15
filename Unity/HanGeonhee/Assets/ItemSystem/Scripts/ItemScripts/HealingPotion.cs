using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingPotion : CoolDownItem
{
    protected override void SetDefault()
    {
        coolDown = 3.0f;
        canUse = true;
    }

    public override void UseEffect()
    {
        if (!canUse)
            return;

        Debug.Log("회복 했습니다.");
    }
}
