using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] List<Transform> weaponSelects;

    private List<Vector3> tempLocate;

    private List<Vector3> tempScale;

    private readonly float changeSpeed = 0.5f;

    private bool nowSwap;

    private void Start()
    {
        weaponSelects[0].GetComponent<WeaponSelect>().StartSetting();
        weaponSelects[1].GetComponent<WeaponSelect>().StartSetting();
        weaponSelects[1].GetComponent<WeaponSelect>().enabled = false;

        tempLocate = new List<Vector3>();
        tempScale = new List<Vector3>();

        for(int i = 0; i< weaponSelects.Count; i++)
        {
            tempLocate.Add(new Vector3(weaponSelects[i].localPosition.x, weaponSelects[i].localPosition.y, weaponSelects[i].localPosition.z));
            tempScale.Add(new Vector3(weaponSelects[i].localScale.x, weaponSelects[i].localScale.y, weaponSelects[i].localScale.z));
        }

        InputManager.Instance.keyDownAction += InputSwap;
    }

    private void OnDisable()
    {
        InputManager.Instance.keyDownAction -= InputSwap;
    }

    public IEnumerator SwapWeapon()
    {
        nowSwap = true;

        weaponSelects[0].GetComponent<WeaponSelect>().enabled = false;
        weaponSelects[1].GetComponent<WeaponSelect>().enabled = false;

        float timer = 0f;

        bool changeFirst = false;

        while (timer < changeSpeed)
        {
            timer += Time.deltaTime;

            weaponSelects[0].localPosition = Vector3.Lerp(weaponSelects[0].localPosition, tempLocate[1],timer / changeSpeed);
            weaponSelects[0].localScale = Vector3.Lerp(weaponSelects[0].localScale, tempScale[1], timer / changeSpeed);

            weaponSelects[1].localPosition = Vector3.Lerp(weaponSelects[1].localPosition, tempLocate[0], timer / changeSpeed);
            weaponSelects[1].localScale = Vector3.Lerp(weaponSelects[1].localScale, tempScale[0], timer / changeSpeed);

            if(timer > 0.05f && !changeFirst)
            {
                weaponSelects[0].SetAsFirstSibling();
                changeFirst = true;
            }

            yield return null;
        }


        weaponSelects[1].GetComponent<WeaponSelect>().enabled = true;

        nowSwap = false;

        weaponSelects.Reverse();
    }

    public void InputSwap()
    {
        if (nowSwap)
            return;

        if(Input.GetKeyDown(KeyCode.F))
        {

            StartCoroutine(SwapWeapon());
        }
    }
}
