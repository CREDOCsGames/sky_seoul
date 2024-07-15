using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcussionGrenade : StackConsumeItem
{
    public ConcussionGrenade()
    {
        maxHold = 4;
        hold = 4;
    }

    public override void UseEffect()
    {
        if (hold == 0)
            return;

        hold--;

        Debug.Log("충격 수류탄을 던졌습니다." + hold + "개 남았습니다.");

        if(hold == 0)
        {
            base.UseEffect();
        }
    }
}
