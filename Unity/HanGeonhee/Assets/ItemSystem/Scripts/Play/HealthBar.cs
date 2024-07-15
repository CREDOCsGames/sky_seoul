using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] protected Image healthGuage;

    [SerializeField] protected float maxHealth;

    private float health;

    protected float healthChangeTimer = 0.5f;

    public float Health
    {
        set { health = value; }
        get { return health; }
    }

    [SerializeField] List<Transform> shieldGuages;

    protected int shieldNum;

    protected float shieldValue;

    protected virtual void Start()
    {
        health = maxHealth;
        healthGuage.fillAmount = health;
        shieldNum = 4;
        shieldValue = 20f;

        for(int i = 0; i < shieldGuages.Count; i++)
        {
            shieldGuages[i].GetChild(0).gameObject.SetActive(true);
        }
    }

    public virtual IEnumerator ChangeHealthGuage(float damage)
    {
        if(shieldNum > 0)
        {
            damage -= shieldValue;
            if (damage < 0)
                yield break;

            shieldNum--;
            shieldGuages[shieldNum].GetChild(0).gameObject.SetActive(false);
        }

        float damagedHealth = health - damage;

        float timer = 0f;

        while (timer < healthChangeTimer)
        {
            timer += Time.deltaTime;

            health = Mathf.Lerp(health, damagedHealth, timer / healthChangeTimer);

            healthGuage.fillAmount = health / maxHealth;

            if(health == damagedHealth)
            {
                break;
            }

            yield return null;
        }
    }
}
