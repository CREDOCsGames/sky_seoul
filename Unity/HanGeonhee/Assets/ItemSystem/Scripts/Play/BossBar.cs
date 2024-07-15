using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossBar : HealthBar
{
    [SerializeField] Image damagedGuage;

    private bool chainStart;

    private float chainTimer = 0f;

    private float chainTime = 1.25f;

    private float damagedHealth;

    protected override void Start()
    {
        base.Start();

        damagedHealth = Health;

        damagedGuage.fillAmount = damagedHealth;
    }

    public override IEnumerator ChangeHealthGuage(float damage)
    {
        StartCoroutine(base.ChangeHealthGuage(damage));

        if(!chainStart)
        {
            StartCoroutine(ChangeDamagedGuage());
            chainStart = true;
            yield break;
        }
        else
        {
            chainTimer = chainTime;
        }
    }

    private IEnumerator ChangeDamagedGuage()
    {
        chainTimer = chainTime;

        while (true)
        {
            chainTimer -= Time.deltaTime;

            if (chainTimer <= 0f)
            {
                StartCoroutine(DeleteDamagedGuage());
                break;
            }

            yield return null;
        }
    }

    private IEnumerator DeleteDamagedGuage()
    {
        float timer = 0f;

        while (timer < healthChangeTimer)
        {
            timer += Time.deltaTime;

            damagedHealth = Mathf.Lerp(damagedHealth, Health, timer / healthChangeTimer);

            damagedGuage.fillAmount = damagedHealth / maxHealth;

            if (damagedHealth == Health)
            {
                break;
            }

            yield return null;
        }

        chainStart = false;
    }
}
