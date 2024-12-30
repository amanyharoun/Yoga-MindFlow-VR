using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTriggerHandle : MonoBehaviour
{
    public Transform maintransform;
    public Vector3 offset;
    public Transform centerEye;
    void Start()
    {
        
    }

    private void OnEnable()
    {
        Initialize();
    }
    void Update()
    {
        CheckOffset();
    }

    void Initialize()
    {
        transform.position = maintransform.position;
    }

    public void CheckOffset()
    {
        offset = maintransform.localPosition = centerEye.localPosition;
    }
}
