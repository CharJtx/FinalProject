using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectTreasure : MonoBehaviour
{

    private bool playerEnter = false;
    private bool Upgraded = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Upgraded)
        {
            MapGenerator.Instance.ShowPanel();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playerEnter == false)
        {
            Upgraded = UpgradePanelManager.instance.ShowUpgradeOptions();
            playerEnter = true;
            
            
            
        }
    }
}
