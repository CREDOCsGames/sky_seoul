using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotSelect : MonoBehaviour
{
    [SerializeField] List<Transform> slots;

    private int selectedSlot;

    private bool nowChange;

    private List<Vector2> tempLocate;
    private List<Vector2> tempScale;

    private float changeSpeed = 0.5f;

    private void Start()
    {
        selectedSlot = 0;

        tempLocate = new List<Vector2>();
        tempScale = new List<Vector2>();

        for(int i = 0; i < slots.Count; i++)
        {
            tempLocate.Add(new Vector2(slots[i].localPosition.x, slots[i].localPosition.y));
            tempScale.Add(new Vector2(slots[i].localScale.x, slots[i].localScale.y));
        }
    }

    private void OnEnable()
    {
        InputManager.Instance.keyDownAction += ChangeSelect;
    }

    private void OnDisable()
    {
        InputManager.Instance.keyDownAction -= ChangeSelect;
    }

    //private static List<GameObject> robotList = new List<GameObject>();
    //
    //public static void AddRobotList(int code)
    //{
    //    
    //}

    public void ChangeSelect()
    {
        if (nowChange)
            return;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            selectedSlot--;
            if (selectedSlot < 0)
                selectedSlot = 2;
            StartCoroutine(SetTransform());
        }
        else if(Input.GetKeyDown(KeyCode.E))
        {
            selectedSlot++;
            if (selectedSlot > 2)
                selectedSlot = 0;
            StartCoroutine(SetTransform());
        }
    }

    private IEnumerator SetTransform()
    {
        if (nowChange)
            yield break;

        nowChange = true;

        float timer = 0f;

        bool changeFirst = false;

        while (timer < changeSpeed)
        {
            timer += Time.deltaTime;

            for(int i = 0; i < slots.Count; i++)
            {
                slots[i % 3].localPosition = Vector2.Lerp(slots[i % 3].localPosition, tempLocate[((selectedSlot * 2) + i) % 3], timer / changeSpeed);
                slots[i % 3].localScale = Vector2.Lerp(slots[i % 3].localScale, tempScale[((selectedSlot * 2) + i) % 3], timer / changeSpeed);
            }

            if (timer > 0.05f && !changeFirst)
            {
                slots[selectedSlot].SetAsLastSibling();
                changeFirst = true;
            }

            yield return null;
        }

        nowChange = false;
    }

}
