using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LipSync : MonoBehaviour
{
    public SkinnedMeshRenderer[] SkinnedMeshRenderers;
    public int blendShapeVal;
    public int changeVal;

    private void Awake()
    {
        changeVal = 2;
    }
    void Start()
    {

    }

    void Update()
    {

    }
    public void RestBlendShapes()
    {
        foreach (SkinnedMeshRenderer smr in SkinnedMeshRenderers)
        {
            smr.SetBlendShapeWeight(0, 0);
        }
    }

    public void StartLipSync()
    {
        DoLipSyncCoroutine = StartCoroutine(DoLipSync());
    }

    Coroutine DoLipSyncCoroutine;
    public IEnumerator DoLipSync()
    {
        while (true)
        {
            foreach (SkinnedMeshRenderer smr in SkinnedMeshRenderers)
            {
                smr.SetBlendShapeWeight(0, blendShapeVal);
            }
            yield return null;
            blendShapeVal += changeVal;
            if (blendShapeVal >= 100)
            {
                changeVal = -2;
            }
            if (blendShapeVal <= 1)
            {
                changeVal = 2;
            }
        }
    }

    public void StopLipSync()
    {
        if (DoLipSyncCoroutine != null)
        {
            StopCoroutine(DoLipSyncCoroutine);
        }
        CancelInvoke(nameof(DoLipSync));
        RestBlendShapes();
    }
}
