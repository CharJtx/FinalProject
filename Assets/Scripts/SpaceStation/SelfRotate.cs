using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfRotate : MonoBehaviour
{
    public Vector3 rotationAxis = new Vector3 (0f, 0f, 1f);
    public float rotationSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate (rotationAxis, rotationSpeed*Time.deltaTime);
    }
}
