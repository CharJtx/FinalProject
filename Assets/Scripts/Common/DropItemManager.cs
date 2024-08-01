using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class DropItemManager : MonoBehaviour
{
    public GameObject[] Exps;
    int expOutputIncrement = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // 0 is small enemy, 
    public void DropItem(Transform dropTransform, int enemyType)
    {
        List<int> expList = new List<int>();

        if (dropTransform == null) return;
        switch (enemyType)
        {
            case 0:
                for (int i = 0; i < 1 + expOutputIncrement; i++)
                {
                    expList.Add(0);
                }
                break;
        }
        Vector3 dp = dropTransform.position;
        for (int i = 0; i < expList.Count; i++)
        {
            GameObject item = Instantiate(Exps[expList[i]], dp, Quaternion.identity);
            ItemBase itemBase = item.GetComponent<ItemBase>();
            if (itemBase != null)
            {
                switch (expList[i])
                {
                    case 0:
                        itemBase.itemType = 1001;
                        break;
                    case 1:
                        itemBase.itemType = 1002;
                        break;
                    case 2:
                        itemBase.itemType = 1003;
                        break;
                }
            }
        }

    }
}
