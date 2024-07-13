using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ActiveWeapon : MonoBehaviour
{
    public Animator rigController;
    public Animator parentAnim;
    [SerializeField] RigWeapon[] weapon;

    [SerializeField] MultiParentConstraint R_Attach;

    [SerializeField] Transform katanaSocket;
    // Start is called before the first frame update
    void Start()
    {
        parentAnim = GetComponentInParent<Animator>();  
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) // 맨손
        {
            Equip(weapon[0], 0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) // 카타나
        {
            Equip(weapon[1], 1);
        }
    }


    public void Equip(RigWeapon newWeapon, int num)
    {
        parentAnim.CrossFade(newWeapon.AnimationName, 0.5f);

        StartCoroutine(OnWeaponSwapComplete(num, newWeapon.WaitTime));
        //rigController.Play("equip_" + newWeapon.AnimationName);
    }

    public void Test()
    {
        var sourceObject = R_Attach.data.sourceObjects;
        sourceObject.SetWeight(0, 1);
        sourceObject.SetWeight(1, 0);
        R_Attach.data.sourceObjects = sourceObject;
    }

    IEnumerator OnWeaponSwapComplete(int index, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        var sourceObject = R_Attach.data.sourceObjects;
        for (int i = 0; i < sourceObject.Count; i++)
        {
            sourceObject.SetWeight(i, 0);
        }

        sourceObject.SetWeight(index, 1);
        R_Attach.data.sourceObjects = sourceObject;
    }
}

[System.Serializable]
public class RigWeapon
{
    public string AnimationName;
    public float WaitTime;
}