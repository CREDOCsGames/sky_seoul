using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;
using System;
using System.Linq;
using TMPro;
using UnityEditorInternal.Profiling.Memory.Experimental;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] GameObject invObj;

    [SerializeField] Transform slotParent;

    [SerializeField]List<QuickSlotUi> invQuickSlots;

    private List<Slot> slots;

    [SerializeField] GameObject itemPrefab;

    [SerializeField] Transform invMain;

    [SerializeField] ViewInfo viewInfo;

    [SerializeField] Animator animator;

    private enum InventoryState
    {
        None,
        PlayMenu,
        Inventory
    }

    private InventoryState state;

    public Transform InvMain
    {
        get { return invMain; }
    }
     
    CultureInfo koreanCulture;

    //[SerializeField] TMP_InputField searchField;

    private Transform hideSlot;

    private void Start()
    {
        PlayerManager.Instance.Inventory = this;

        koreanCulture = new CultureInfo("ko-KR");

        slots = new List<Slot>();

        slots.Capacity = slotParent.childCount;

        for(int i = 0; i < slotParent.childCount; i++)
        {
            slots.Add(slotParent.GetChild(i).GetComponent<Slot>());
        }

        hideSlot = slots[slots.Count - 1].transform;

        slots.RemoveAt(slots.Count - 1);

        InputManager.Instance.keyDownAction += OpenInventory;

        ItemAddInventory(1);
        ItemAddInventory(1);
        ItemAddInventory(1);
        ItemAddInventory(2);
        ItemAddInventory(2);
        ItemAddInventory(3);
        ItemAddInventory(4);
        ItemAddInventory(4);
        ItemAddInventory(5);
        ItemAddInventory(6);

        invObj.SetActive(false);

        state = InventoryState.None;
    }

    private void OnDisable()
    {
        InputManager.Instance.keyDownAction -= OpenInventory;
    }

    public void SetItemInfomation(int code)
    {
        viewInfo.ViewInfomation(code);
    }

    public void ItemAddInventory(int code, int count = 1)
    {
        int i = HoldTask(code);

        if(i > -1)
        {
            var obj = ItemManager.Instance.GetItemScript(code) as StackConsumeItem;

            if (obj != null)
            {
                obj.AddHold(count);
                slots[i].SetCountText(slots[i].gameObject.GetComponentInChildren<TextMeshProUGUI>(), obj.GetHoldNum());
                return;
            }
        }

        int j = EmptySlot();

        if (j == -1)
            return;

        GameObject newItem = Instantiate(itemPrefab, slots[j].transform);

        newItem.GetComponent<DragObject>().ItemCode = code;

        newItem.GetComponent<Image>().sprite = ResourcesManager.Instance.Load<Sprite>(ItemManager.Instance.GetItemInfo(code).name);
    }

    private int HoldTask(int code)
    {
        for(int i = 0; i < slots.Count; i++)
        {
            if (slots[i].transform.childCount <= 0)
                continue;

            if (slots[i].gameObject.GetComponentInChildren<DragObject>().ItemCode == code)
                return i;
        }


        return -1;
    }

    private int EmptySlot()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].transform.childCount <= 0)
                return i;
        }
        return -1;
    }

    public void QsItemCreate(int code,Transform parent)
    {
        GameObject obj = Instantiate(itemPrefab, parent);

        obj.GetComponent<DragObject>().ItemCode = code;

        obj.GetComponent<Image>().sprite = ResourcesManager.Instance.Load<Sprite>(ItemManager.Instance.GetItemInfo(code).name);
    }

    public void OpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (state == InventoryState.None)
            {
                animator.SetTrigger("PlayMenu");
                state = InventoryState.PlayMenu; 
            }
            else if(state == InventoryState.PlayMenu)
            {
                animator.SetTrigger("Inventory");
                state = InventoryState.Inventory;
            }
        }
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(state == InventoryState.PlayMenu)
            {
                animator.SetTrigger("Close");
                state = InventoryState.None;
            }
            else if(state == InventoryState.Inventory)
            {
                animator.SetTrigger("Close");
                state = InventoryState.PlayMenu;
            }
        }
    }

    public void SetInventory()
    {
        for (int i = 0; i < invQuickSlots.Count; i++)
        {
            invQuickSlots[i].SetQuickSlot(QuickSlot.slotItems[invQuickSlots[i].slotNum].code);
        }
    }

    private void SetHide(Slot item)
    {
        Transform trans = item.transform.GetChild(0);

        trans.SetParent(hideSlot);
        trans.localPosition = Vector2.zero;
    }

    public void EquipSort()
    {
        List<DragObject> Items = new List<DragObject>();

        string type = "";

        for (int i = 0; i < hideSlot.childCount; i++)
        {
            type = ItemManager.Instance.GetItemInfo(hideSlot.GetChild(i).GetComponent<DragObject>().ItemCode).type;

            if ((type == ItemType.Equip.ToString()) || (type == ItemType.Weapon.ToString()) || (type == ItemType.CoolDown.ToString()))
                Items.Add(hideSlot.GetChild(i).GetComponent<DragObject>());
        }

        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].transform.childCount <= 0)
                continue;

            type = ItemManager.Instance.GetItemInfo(slots[i].GetComponentInChildren<DragObject>().ItemCode).type;

            if ((type == ItemType.Equip.ToString()) || (type == ItemType.Weapon.ToString()) || (type == ItemType.CoolDown.ToString()))
            {
                Items.Add(slots[i].GetComponentInChildren<DragObject>());
            }
            else
            {
                SetHide(slots[i]);
            }
        }

        QuickSort(Items, 0, Items.Count - 1);

        for (int i = 0; i < Items.Count; i++)
        {
            Items[i].transform.SetParent(slots[i].transform);
            Items[i].transform.localPosition = Vector2.zero;
        }
    }

    public void ConsumeSort()
    {
        List<DragObject> Items = new List<DragObject>();

        string type = "";

        for (int i = 0; i < hideSlot.childCount; i++)
        {
            type = ItemManager.Instance.GetItemInfo(hideSlot.GetChild(i).GetComponent<DragObject>().ItemCode).type;

            if (type == ItemType.Consume.ToString())
                Items.Add(hideSlot.GetChild(i).GetComponent<DragObject>());
        }

        for (int i = 0; i < slots.Count - 1; i++)
        {
            if (slots[i].transform.childCount <= 0)
                continue;

            type = ItemManager.Instance.GetItemInfo(slots[i].GetComponentInChildren<DragObject>().ItemCode).type;

            if (type == ItemType.Consume.ToString())
            {
                Items.Add(slots[i].GetComponentInChildren<DragObject>());
            }
            else
            {
                SetHide(slots[i]);
            }
        }

        QuickSort(Items, 0, Items.Count - 1);

        for (int i = 0; i < Items.Count; i++)
        {
            Items[i].transform.SetParent(slots[i].transform);
            Items[i].transform.localPosition = Vector2.zero;
        }
    }

    public void ImportSort()
    {
        List<DragObject> Items = new List<DragObject>();

        string type = "";

        for (int i = 0; i < hideSlot.childCount; i++)
        {
            type = ItemManager.Instance.GetItemInfo(hideSlot.GetChild(i).GetComponent<DragObject>().ItemCode).type;

            if (type == ItemType.Import.ToString())
                Items.Add(hideSlot.GetChild(i).GetComponent<DragObject>());
        }

        for (int i = 0; i < slots.Count - 1; i++)
        {
            if (slots[i].transform.childCount <= 0)
                continue;

            type = ItemManager.Instance.GetItemInfo(slots[i].GetComponentInChildren<DragObject>().ItemCode).type;

            if (type == ItemType.Import.ToString())
            {
                Items.Add(slots[i].GetComponentInChildren<DragObject>());
            }
            else
            {
                SetHide(slots[i]);
            }
        }

        QuickSort(Items, 0, Items.Count - 1);

        for (int i = 0; i < Items.Count; i++)
        {
            Items[i].transform.SetParent(slots[i].transform);
            Items[i].transform.localPosition = Vector2.zero;
        }
    }

    public void AllSort()
    {
        List<DragObject> Items = new List<DragObject>();

        for (int i = 0; i < hideSlot.childCount; i++)
        {
           Items.Add(hideSlot.GetChild(i).GetComponent<DragObject>());
        }

        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].transform.childCount <= 0)
                continue;

            slots[i].transform.GetChild(0).gameObject.SetActive(true);

            Items.Add(slots[i].GetComponentInChildren<DragObject>());
        }

        QuickSort(Items, 0, Items.Count - 1);

        for (int i = 0; i < Items.Count; i++)
        {
            Items[i].transform.SetParent(slots[i].transform);
            Items[i].transform.localPosition = Vector2.zero;
        }
    }

    /*
    public void InputValueChanged()
    {
        SearchItem(searchField.text);
    }

    private void SearchItem(string str)
    {

        for(int i = 0; i < slots.Count; i++)
        {
            if (slots[i].transform.childCount <= 0)
                continue;

            if (!TextManager.Instance.LoadString("ItemLanguage", slots[i].GetComponentInChildren<DragObject>().ItemCode).Contains(str, StringComparison.OrdinalIgnoreCase))
            {
                slots[i].transform.GetChild(0).GetComponent<Image>().material = PlayerManager.Instance.GrayShader;
            }
            else
            {
                slots[i].transform.GetChild(0).GetComponent<Image>().material = null;
            }
        }
    }

    public void NameSort()
    {
        List<DragObject> nameSort = new List<DragObject>();

        for(int i = 0; i< slots.Count; i++)
        {
            if (slots[i].transform.childCount <= 0)
                continue;

            nameSort.Add(slots[i].GetComponentInChildren<DragObject>());
        }

        QuickSort(nameSort, 0, nameSort.Count - 1);

        for(int i = 0; i < nameSort.Count; i++)
        {
            nameSort[i].transform.SetParent(slots[i].transform);
            nameSort[i].transform.localPosition = Vector2.zero;
        }
    }

    public void TypeSort()
    {
        List<List<DragObject>> sort = new List<List<DragObject>>();

        for (int i = 0; i < (int)ItemType.Count; i++)
        {
            sort.Add(new List<DragObject>());
        }

        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].transform.childCount <= 0)
                continue;

            for(int j = 0; j < (int)ItemType.Count; j++)
            {
                if (ItemManager.Instance.GetItemInfo(slots[i].GetComponentInChildren<DragObject>().ItemCode).type == ((ItemType)j).ToString())
                {
                    sort[j].Add(slots[i].GetComponentInChildren<DragObject>());
                    break;
                }
            }

        }

        for (int i = 0; i < sort.Count; i++)
        {
            if (sort[i].Count <= 1)
                continue;

            QuickSort(sort[i], 0, sort[i].Count - 1);
        }

        List<DragObject> finish = new List<DragObject>();

        for (int i = 0; i < sort.Count; i++)
        {
            for (int j = 0; j < sort[i].Count; j++)
            {
                finish.Add(sort[i][j]);
            }
        }

        for (int i = 0; i < finish.Count; i++)
        {
            finish[i].transform.SetParent(slots[i].transform);
            finish[i].transform.localPosition = Vector2.zero;
        }
    }
    */

    private string GetName(int itemCode)
    {
        return TextManager.Instance.LoadString("ItemNameLanguage",itemCode);
    }

    private void Swap(List<DragObject> list, int first, int second)
    {
        DragObject temp = list[first];

        list[first] = list[second];

        list[second] = temp;
    }

    private bool PartitionCheck(string lower, string higher)
    {
        int compareResult = 0;

        if(DataManager.Instance.Data.Language == "Korean")
        {
            compareResult = String.Compare(lower, higher, false, koreanCulture);
        }
        else if(DataManager.Instance.Data.Language == "English")
        {
            compareResult = String.Compare(lower, higher, false);
        }

        if(compareResult < 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private int Partition(List<DragObject> sort, int left, int right)
    {
        string pivot = GetName(sort[left].ItemCode);

        int low = left;
        int high = right + 1;

        do
        {
            do
            {
                low++;
            } while (low <= right && PartitionCheck(GetName(sort[low].ItemCode), pivot));

            do
            {
                high--;
            } while (high >= left && PartitionCheck(pivot, GetName(sort[high].ItemCode)));

            if (low < high)
            {
                Swap(sort, low, high);
            }

        } while (low < high);

        Swap(sort, left, high);

        return high;
    }

    private void QuickSort(List<DragObject> sort, int left, int right)
    {
        if(left < right)
        {
            int i = Partition(sort, left, right);

            QuickSort(sort, left, i - 1);
            QuickSort(sort, i + 1, right);
        }
    }
}
