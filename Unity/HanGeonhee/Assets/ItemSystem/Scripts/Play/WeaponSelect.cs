using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponSelect : MonoBehaviour
{
    private WeaponItem weapon;

    [SerializeField] QuickSlotUi slotUI;

    [SerializeField] int WeaponCode;

    [SerializeField] Transform bulletsParent;

    private List<Transform> bullts;

    [SerializeField] TextMeshProUGUI bulletLimit;

    public void StartSetting()
    {
        weapon = ItemManager.Instance.GetItemScript(WeaponCode) as WeaponItem;

        bullts = new List<Transform>();

        for(int i = 0; i < bulletsParent.childCount; i++)
        {
            bullts.Add(bulletsParent.GetChild(i));
        }

        if(weapon == null)
        {
            Destroy(gameObject);
        }

        SetSlotUI();

        SetBulletInfo();
    }

    private void OnEnable()
    {
        InputManager.Instance.keyDownAction += InputKey;
    }

    private void OnDisable()
    {

        InputManager.Instance.keyDownAction -= InputKey;
    }

    public void SetSlotUI()
    {
        PlayerManager.Instance.Inventory.QsItemCreate(WeaponCode, transform);

        slotUI.SetSlotInfo(WeaponCode);
    }

    public void Shot()
    {
        weapon.AttackEffect();

        if (weapon.currentAmmo >= weapon.maxAmmo)
        {
            SetBulletInfo();
            return;
        }


        bullts[weapon.currentAmmo].GetChild(0).gameObject.SetActive(false);
        

        bulletLimit.text = weapon.currentAmmo.ToString();
    }

    public void Reload()
    {
        var Reload = weapon as IWeaponReload;
        if(Reload == null)
            return;

        Reload.ReloadEffect();

        SetBulletInfo();
    }

    private void SetBulletInfo()
    {
        bulletLimit.text = weapon.currentAmmo.ToString();

        for (int i = 0; i < weapon.currentAmmo; i++)
        {
            bullts[i].GetChild(0).gameObject.SetActive(true);
        }
    }

    public void InputKey()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Shot();
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }
}
