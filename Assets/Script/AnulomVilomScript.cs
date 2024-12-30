using Core3lb;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnulomVilomScript : MonoBehaviour
{
    public TextToSpeech instructionSpeech;
    public TextToSpeech inhaleFromLeft;
    public TextToSpeech inhaleFromRight;
    public TextToSpeech exhaleFromLeft;
    public TextToSpeech exhaleFromRight;
    public TextToSpeech[] TTS;
    public GameObject[] smokes;
    public int currentIndex;
    public GameObject ball;
    public Color inColor;
    public Color OutColor;
    public float time;
    void Start()
    {
    }

    private void OnEnable()
    {
        Invoke(nameof(Initialize), 1);
    }
    private void OnDisable()
    {
        foreach (var item in TTS)
        {
            item.gameObject.SetActive(false);
        }
        instructionSpeech.gameObject.SetActive(false);
        foreach (var item in smokes)
        {
            item.gameObject.SetActive(false);
        }
        ball.GetComponent<ResettableObject>()._ResetObject();
        ball.GetComponent<Renderer>().material.DOColor(Color.white, 0);
    }
    public void Initialize()
    {
        StartAnulomVilom();
    }

    public void StartAnulomVilom()
    {
        foreach (var item in TTS)
        {
            item.gameObject.SetActive(false);
        }
        instructionSpeech.gameObject.SetActive(true);
    }
    public void BreatheIn(Transform tr)
    {
        ball.GetComponent<Renderer>().material.DOColor(inColor, time);
        ball.transform.DOMove(tr.position, 2f);
    }
    public void BreatheOut(Transform tr)
    {
        ball.GetComponent<Renderer>().material.DOColor(OutColor, time);
        ball.transform.DOMove(tr.position, 2f);
    }
}
