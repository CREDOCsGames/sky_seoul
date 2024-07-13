using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;


[System.Serializable]
public class SlashEffect
{
    public GameObject slashVFX;
    public float effectScale;
    public float effectRate;
    public GameObject spawnPosition;
}

public class SpawnEffects : MonoBehaviour
{
    public SlashEffect leftEffect;
    public SlashEffect rightEffect;

    public void SpawnLeftEffect()
    {

        var effectObj = Instantiate(leftEffect.slashVFX, leftEffect.spawnPosition.transform.position, Quaternion.identity);
        effectObj.transform.parent = leftEffect.spawnPosition.transform;
        effectObj.transform.localScale = Vector3.one * leftEffect.effectScale;
        effectObj.GetComponentInChildren<VisualEffect>().playRate = leftEffect.effectRate;
        effectObj.GetComponentInChildren<VisualEffect>().Play();
    }

    public void SpawnRightEffect()
    {

        var effectObj = Instantiate(rightEffect.slashVFX, rightEffect.spawnPosition.transform.position, Quaternion.identity);
        effectObj.transform.parent = rightEffect.spawnPosition.transform;
        effectObj.transform.localScale = Vector3.one * rightEffect.effectScale;
        effectObj.GetComponentInChildren<VisualEffect>().playRate = rightEffect.effectRate;
        effectObj.GetComponentInChildren<VisualEffect>().Play();
        AudioManager.instance.PlaySwordSfx();

    }
}
