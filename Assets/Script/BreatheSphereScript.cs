using Core3lb;
using DG.Tweening;
using Meta.WitAi.TTS.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreatheSphereScript : MonoBehaviour
{
    Renderer renderer;
    public bool breathIn = true;
    public Color inColor;
    public Color outColor;
    [SerializeField] private float time;
    public TextToSpeech inhaleTTS;
    public TextToSpeech instructionTTS;
    public TextToSpeech exhaleTTS;
    public int currentRound;
    public int currentIndex;
    public int[] allRoundLimts;
    public TimedEventBase timedEventBase;
    public ResettableObject resettableObject;
    void Start()
    {

    }

    private void OnEnable()
    {
        Invoke(nameof(Initialize), 1);
    }

    private void OnDisable()
    {
        resettableObject._ResetObject();
        inhaleTTS.gameObject.SetActive(false);
        exhaleTTS.gameObject.SetActive(false);
        instructionTTS.gameObject.SetActive(false);
        renderer.material.DOColor(Color.white,0 );
    }
    public void StartExersize()
    {
        StartCoroutine(IEStartExersize());
    }
    public IEnumerator IEStartExersize()
    {
        Debug.Log("StartExersize");
        timedEventBase._RestartTimer();
        yield return new WaitForSeconds(.5f);
        timedEventBase._Start();
        if (breathIn)
        {
            BreathIn();
            breathIn = false;
        }
        else
        {
            BreatheOut();
            breathIn = true;
        }
    }
    public void BreathIn()
    {
        Debug.Log("BreathIn");
        exhaleTTS.gameObject.SetActive(false);
        inhaleTTS.gameObject.SetActive(false);
        inhaleTTS.gameObject.SetActive(true);
        renderer.material.DOColor(inColor, time);
        transform.DOScale(Vector3.one * 5, time);
    }

    public void OnComplete()
    {
        Debug.Log("OnComplete");
        currentRound++;
        if (currentRound > allRoundLimts[currentIndex])
        {
            currentRound = 0;
            if (currentIndex < (allRoundLimts.Length - 1))
            {
                currentIndex++;
            }
            else
            {
                currentIndex = 0;
                time = 10;
            }
            time--;
            timedEventBase.currentInterval = time;
        }
        StartExersize();
    }
    public void BreatheOut()
    {
        Debug.Log("BreatheOut");
        inhaleTTS.gameObject.SetActive(false);
        exhaleTTS.gameObject.SetActive(false);
        exhaleTTS.gameObject.SetActive(true);
        renderer.material.DOColor(outColor, time);
        transform.DOScale(Vector3.one * 2, time);
    }

    public void Initialize()
    {
        transform.localScale = Vector3.one * 2;
        renderer = GetComponent<Renderer>();
        time = 10;
        inhaleTTS.gameObject.SetActive(false);
        exhaleTTS.gameObject.SetActive(false);
        instructionTTS.gameObject.SetActive(true);
        currentRound = 0;
        currentIndex = 0;
        instructionTTS.GetComponent<TTSSpeaker>().Events.OnAudioClipPlaybackFinished.RemoveAllListeners();
        instructionTTS.GetComponent<TTSSpeaker>().Events.OnAudioClipPlaybackFinished.AddListener((o) =>
        {
            StartExersize();
            instructionTTS.gameObject.SetActive(false);
        });
    }
}
