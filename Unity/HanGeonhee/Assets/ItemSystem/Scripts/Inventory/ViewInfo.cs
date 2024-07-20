using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ViewInfo : MonoBehaviour
{
    [SerializeField] Image itemImage;
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI itemInfo;

    public void ViewInfomation(int code)
    {
        itemImage.sprite = ResourcesManager.Instance.Load<Sprite>(ItemManager.Instance.GetItemInfo(code).name);

        itemName.text = TextManager.Instance.LoadString("ItemNameLanguage", code);

        InfoTextSetting(code);
    }

    private void InfoTextSetting(int code)
    {
        string infoText = TextManager.Instance.LoadString("ItemData", code);

        if(infoText.Contains("<AC>"))
        {
            infoText = infoText.Replace("<AC>", (ItemManager.Instance.GetItemScript(code) as WeaponItem).maxAmmo.ToString());
            infoText = infoText.Replace("<AS>", (ItemManager.Instance.GetItemScript(code) as WeaponItem).attackSpeed.ToString());
            infoText = infoText.Replace("<AP>", (ItemManager.Instance.GetItemScript(code) as WeaponItem).attackDamage.ToString());
        }

        if(infoText.Contains("<CD>"))
        {
            infoText = infoText.Replace("<CD>", (ItemManager.Instance.GetItemScript(code) as CoolDownItem).coolDown.ToString());
        }

        if (infoText.Contains("<MP>"))
        {
            infoText = infoText.Replace("<MP>", (ItemManager.Instance.GetItemScript(code) as StackConsumeItem).maxHold.ToString());
        }

        if (infoText.Contains("<DF>"))
        {
            infoText = infoText.Replace("<DF>", (ItemManager.Instance.GetItemScript(code) as EquipItem).defense.ToString());
        }

        itemInfo.text = infoText;
    }
}
