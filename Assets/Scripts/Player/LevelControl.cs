using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelControl : MonoBehaviour
{
    public int level = 0;
    public float exp = 0f;
    public float expBonus = 1f;
    private float expUpperBound = 1f;
    private Canvas canvas;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (expUpperBound <= exp) LevelUp();
    }

    public void ExpPromotion(int expGained)
    {
        exp += expGained * expBonus;
        Debug.Log(exp);
    }

    public void LevelUp()
    {
        while (expUpperBound <= exp)
        {
            exp -= expUpperBound;

            UpgradePanelManager upgradePanelManager = gameObject.GetComponent<UpgradePanelManager>();
            if (upgradePanelManager != null)
            {
                upgradePanelManager.ShowUpgradeOptions();
            }
            level++;
        }
    }
}
