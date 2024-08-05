using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpItem : MonoBehaviour
{
    public int expValue = 0;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ExpGain(Transform player)
    {
        LevelControl giveExp = player.gameObject.GetComponent<LevelControl>();
        if (giveExp != null)
        {
            giveExp.ExpPromotion(expValue);
        }
    }
}
