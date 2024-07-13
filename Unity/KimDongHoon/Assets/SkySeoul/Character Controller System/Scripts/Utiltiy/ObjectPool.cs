using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;

[System.Serializable]
class PoolItem
{
    public bool isActive;
    public GameObject gameObject;
}

public class ObjectPool
{
    // ������ƮǮ ����ؾ� �ϴ� ����?
    // Object�� ���� - �޸� ���, 
    // Object �ı� - �޸� GC  
    // ���� �ݺ� �� �ı��� GC�� ���� ������ ����� �߱��Ѵ�.

    // ���� ������Ʈ�� �̸� �����ص� �� �ʿ��� �� ���� Ȱ��ȭ�ϰ�, �ı��Ǹ� �ٽ� ��Ȱ��ȭ �ϴ� ����� �ǹ��Ѵ�.

    // �ʿ��� ���..  ������ �Ҵ�, Pop, Push, Dispose(���ִ�)

    public int maxCount = int.MaxValue;  // �ִ� ���� ���� ��
    private int allocationCount;         // ���� ��� ������ ����
    private int activeCount;             // ���� ��� ���� ������Ʈ ��
    private int increaseCount = 5;           // Pool�� ������Ʈ�� ������ �� �߰��� �����Ǵ� ��

    private GameObject poolObject;
    private List<PoolItem> poolItemList; // �÷����� �Ѱ��� ����Ѵ� -> Item�� Pool�� �� ���� �ش� �÷��ǿ��� ��Ȱ��ȭ�� Object�� Ž���ϴ� ����� �����.
                                         // �÷����� �ΰ��� ����ϸ� ���?

    private static Transform Container;

    public ObjectPool(GameObject obj)
    {
        allocationCount = 0;
        activeCount = 0;
        this.poolObject = obj;
        poolItemList = new List<PoolItem>();

        if (Container == null)
        {
            GameObject container = new GameObject();
            container.name = "Container";

            Container = container.transform;
        }

        InstantiateObjects();
    }

    public void InstantiateObjects()
    {
        allocationCount += increaseCount;

        for (int i = 0; i < allocationCount; i++)
        {
            PoolItem pool = new PoolItem();

            pool.isActive = false;
            pool.gameObject = GameObject.Instantiate(poolObject);
            pool.gameObject.SetActive(false);

            poolItemList.Add(pool);

            pool.gameObject.transform.parent = Container;
        }
    }

    public void DestroyObjects()
    {
        if (poolItemList == null) return;

        int count = poolItemList.Count;
        for(int i = 0; i < count; i++)
        {
            GameObject.Destroy(poolItemList[i].gameObject);
        }

        poolItemList.Clear();
    }

    public GameObject ActivatePoolItem()
    {
        if (poolItemList == null) return null;

        if(allocationCount == activeCount)
        {
            InstantiateObjects();
        }

        int count = poolItemList.Count;

        for (int i = 0; i < count; i++)
        {
            PoolItem poolItem = poolItemList[i];

            if(poolItem.isActive == false)
            {
                activeCount++;

                poolItem.isActive = true;
                poolItem.gameObject.SetActive(true);

                return poolItem.gameObject;
            }
        }

        return null;
    }

    public void DeactivatePoolItem(GameObject removeObject)
    {
        if (poolItemList == null || removeObject == null) return;

        int count = poolItemList.Count;

        for (int i = 0; i < count; i++)
        {
            PoolItem poolItem = poolItemList[i];

            if(poolItem.gameObject == removeObject)
            {
                activeCount--;

                poolItem.isActive = false;
                poolItem.gameObject.SetActive(false);

                return;
            }
        }
    }

    public void DeactiveateAllPoolItems()
    {
        if (poolItemList == null) return;

        int count = poolItemList.Count;

        for(int i=0; i < count; i++)
        {
            PoolItem poolItem = poolItemList[i];

            if(poolItem.gameObject != null && poolItem.isActive == true)
            {
                poolItem.isActive = false;
                poolItem.gameObject.SetActive(false);
            }
        }


        activeCount = 0;
    }

}
