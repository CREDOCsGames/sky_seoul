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
    // 오브젝트풀 사용해야 하는 이유?
    // Object를 생성 - 메모리 사용, 
    // Object 파괴 - 메모리 GC  
    // 잩은 반복 및 파괴는 GC에 의한 프레임 드랍을 야기한다.

    // 따라서 오브젝트를 미리 생성해둔 후 필요할 때 마다 활성화하고, 파괴되면 다시 비활성화 하는 방식을 의미한다.

    // 필요한 기능..  데이터 할당, Pop, Push, Dispose(없애다)

    public int maxCount = int.MaxValue;  // 최대 생성 가능 수
    private int allocationCount;         // 현재 허용 가능한 숫자
    private int activeCount;             // 현재 사용 중인 오브젝트 수
    private int increaseCount = 5;           // Pool할 오브젝트가 부족할 때 추가로 생성되는 수

    private GameObject poolObject;
    private List<PoolItem> poolItemList; // 컬렉션을 한개만 사용한다 -> Item을 Pool할 때 마다 해당 컬렉션에서 비활성화된 Object를 탐색하는 비용이 생긴다.
                                         // 컬렉션을 두개를 사용하면 어떨까?

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
