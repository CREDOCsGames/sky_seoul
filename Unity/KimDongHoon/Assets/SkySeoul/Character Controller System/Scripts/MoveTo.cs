using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveTo : MonoBehaviour
{
    private Vector3 moveDir = Vector3.zero;
    private float moveSpeed = 5.0f;


    public void MoveToDir(Vector3 dir)
    {
        moveDir = dir;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }
}
