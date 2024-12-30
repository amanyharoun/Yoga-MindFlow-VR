using Core3lb;
using DG.Tweening;
using Meta.WitAi.TTS.Utilities;
using System.Collections;
using UnityEngine;

public class ChairYogaManager : MonoBehaviour
{
    public GameObject[] chairYogas;
    public TextToSpeech instructionTTS;
    public TextToSpeech handsForwardTTS;
    public TextToSpeech handsUpTTS;
    public TextToSpeech handsSideTTS;
    public TextToSpeech handsdownTTS;
    public GameObject[] TTSs;
    public Transform leftShoulder;
    public Transform rightShoulder;
    public Transform leftArm;
    public Transform leftForeArm;
    public Transform rightArm;
    public Transform rightForeArm;
    public Transform spine;
    public Transform head;

    public Transform leftHandTarget;
    public Transform leftTargetForwardVal;
    public Transform leftTargetUpVal;
    public Transform leftTargetSideVal;
    public Transform leftTargetOriginalVal;
    public Transform rightHandTarget;
    public Transform rightTargetForwardVal;
    public Transform rightTargetUpVal;
    public Transform rightTargetSideVal;
    public Transform rightTargetOriginalVal;
    void Start()
    {

    }

    private void OnEnable()
    {
        Invoke(nameof(Initialize), 1f);
    }

    private void OnDisable()
    {
        instructionTTS.gameObject.SetActive(false);
        foreach (var t in TTSs)
        {
            t.SetActive(false);
        }
    }

    Coroutine cr;
    public void DoHandExerSize()
    {
        if (cr != null)
        {
            StopCoroutine(cr);
        }
        cr = StartCoroutine(IEDoHandExersize());
    }

    public IEnumerator IEDoHandExersize()
    {
        yield return new WaitForSeconds(3);
        PlayTTS(handsForwardTTS);
        DoHandPositionAndRotation(leftHandTarget, leftTargetForwardVal, 2, 2);
        DoHandPositionAndRotation(rightHandTarget, rightTargetForwardVal, 2, 2);

        yield return new WaitForSeconds(3);
        PlayTTS(handsUpTTS);
        DoHandPositionAndRotation(leftHandTarget, leftTargetUpVal, 2, 2);
        DoHandPositionAndRotation(rightHandTarget, rightTargetUpVal, 2, 2);
        yield return new WaitForSeconds(3);
        PlayTTS(handsSideTTS);
        DoHandPositionAndRotation(leftHandTarget, leftTargetSideVal, 2, 2);
        DoHandPositionAndRotation(rightHandTarget, rightTargetSideVal, 2, 2);
        yield return new WaitForSeconds(3);
        PlayTTS(handsdownTTS);
        DoHandPositionAndRotation(leftHandTarget, leftTargetOriginalVal, 2, 2);
        DoHandPositionAndRotation(rightHandTarget, rightTargetOriginalVal, 2, 2);
        DoHandExerSize();
    }

    public void PlayTTS(TextToSpeech tts)
    {
        foreach (var t in TTSs)
        {
            t.SetActive(false);
        }
        tts.gameObject.SetActive(true);
    }

    public void DoHandPositionAndRotation(Transform hand, Transform desire, float timeforPos, float timeforRot)
    {
        hand.DOMove(desire.transform.position, timeforPos);
        hand.DORotateQuaternion(desire.rotation, timeforRot);
    }

    public void Initialize()
    {
        DoHandPositionAndRotation(leftHandTarget, leftTargetOriginalVal, 2, 2);
        DoHandPositionAndRotation(rightHandTarget, rightTargetOriginalVal, 2, 2);
        instructionTTS.gameObject.SetActive(true);
        instructionTTS.GetComponent<TTSSpeaker>().Events.OnAudioClipPlaybackFinished.RemoveAllListeners();
        instructionTTS.GetComponent<TTSSpeaker>().Events.OnAudioClipPlaybackFinished.AddListener((o) =>
        {
            DoHandExerSize();
        });
    }
}
