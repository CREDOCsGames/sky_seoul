using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] BossBar bossBar;

    public void TakedDamage(float damage)
    {
        StartCoroutine(bossBar.ChangeHealthGuage(damage));
    }
}
