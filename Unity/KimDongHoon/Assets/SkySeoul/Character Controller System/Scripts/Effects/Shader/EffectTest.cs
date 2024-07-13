using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectTest : MonoBehaviour
{
    [HideInInspector] public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            anim.CrossFade("DualOne", 0.2f);
        }

        if (Input.GetMouseButton(0))
        {
            anim.SetTrigger("doCombo");
        }
    }
}
