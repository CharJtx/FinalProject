using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemManager : MonoBehaviour
{
    public GameObject[] Exps;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DropItem (Transform dropTransform) 
    {
        if (dropTransform == null) return;
        Vector3 dp = dropTransform.position;
        Instantiate(Exps[0],dp, Quaternion.identity);
    }
}
