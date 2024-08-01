using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelControl : MonoBehaviour
{
    public int level = 0;
    public float exp = 0f;
    public float expBonus = 1f;
    private float expUpperBound = 10f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ExpPromotion(int expGained)
    {
        exp += expGained * expBonus;
        Debug.Log(exp);
    }
}
