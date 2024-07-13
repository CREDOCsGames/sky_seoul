using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootIKSetUp : MonoBehaviour
{
    Transform objToPickUp;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        float reach = anim.GetFloat("Walk");
    }
}
