using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class IrohaParent : MonoBehaviour
{
    public GameObject mParentCon;              // 리그 컨트롤러 연결
    public GameObject Sword;                   // 아이템 주우면 활성화 될 검
    public GameObject groundSword;             // 비활성화할 검

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private enum Mode                         // 플레이어의 상태 분류
    {
        Idle,
        Ground,
        Hand,
        Back
    }

    private Mode m_Mode;                     // 플레이어의 상태를 저장하는 변수 저장

    private void Update()
    {
        SetStateChange();

        if (m_Mode != Mode.Idle)
        {
            var constraint = mParentCon.GetComponent<MultiParentConstraint>();
            var sourceObjects = constraint.data.sourceObjects;

            sourceObjects.SetWeight(0, m_Mode == Mode.Ground ? 1f : 0f);
            sourceObjects.SetWeight(1, m_Mode == Mode.Hand ? 1f : 0f);
            sourceObjects.SetWeight(2, m_Mode == Mode.Back ? 1f : 0f);
            constraint.data.sourceObjects = sourceObjects;

            m_Mode = Mode.Idle;
        }
    }

    private void Start()
    {
        m_Mode = Mode.Ground;
       
    }

    void SetStateChange()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            hand();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            back();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            TakeOutSword();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            PlayAnimation("DualBlade");
        }
    }

    private void TakeOutSword()
    {
        animator.CrossFadeInFixedTime("TakeOutSword", 0.2f);
    }

    public void hand()
    {
        animator.CrossFadeInFixedTime("DrawSword", 0.2f);
    }

    public void SetHand()
    {
        m_Mode = Mode.Hand;
        Sword.SetActive(true);
        groundSword.SetActive(false);
    }

    public void back()
    {
        animator.CrossFadeInFixedTime("putSword", 0.2f);
    }

    public void SetBack()
    {
        m_Mode = Mode.Back;
    }

    public void PlayAnimation(string animName)
    {
        animator.CrossFadeInFixedTime(animName, 0.2f);
    }
}
