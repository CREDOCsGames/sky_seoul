using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGrenade : StackConsumeItem
{
    public FireGrenade()
    {
        maxHold = 3;
    }

    public override void UseEffect()
    {
        if (hold == 0)
            return;

        hold--;

        Debug.Log("화염 수류탄을 던졌습니다." + hold + "개 남았습니다.");

        if (hold == 0)
        {
            base.UseEffect();
        }
    }
}
