using Core3lb;
using Meta.WitAi.TTS.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadRotationYoga : MonoBehaviour
{
    public TextToSpeech instructionTTS;
    public ResettableObject resettableObject;
    public ResettableObject resettable2Object;
    public FollowTarget followTarget;
    public DoRotation doRotation;
    public DoMovement doMovement;
    public int currentRound;
    public int currentIndex;
    public int[] allRoundLimts;
    void Start()
    {

    }


    private void OnEnable()
    {
        Invoke(nameof(Initialize), 1f);
    }
    private void OnDisable()
    {
        doRotation.isRotating = false;
        resettableObject._ResetObject();
        resettable2Object._ResetObject();
        followTarget._StopFollow();
        doRotation.rotateSpeed = 10;
        instructionTTS.gameObject.SetActive(false);
    }

    public void Initialize()
    {
        doMovement.transform.position = Vector3.zero;
        instructionTTS.gameObject.SetActive(true);
        instructionTTS.GetComponent<TTSSpeaker>().Events.OnAudioClipPlaybackFinished.RemoveAllListeners();
        instructionTTS.GetComponent<TTSSpeaker>().Events.OnAudioClipPlaybackFinished.AddListener((o) =>
        {
            followTarget._StartFollow();
            doMovement._MoveToY(3f);
            doRotation.onCompleteRound.RemoveAllListeners();
            doRotation.onCompleteRound.AddListener(OnComplete);
            doMovement.tweenDone.AddListener(() =>
            {
                doRotation.isRotating = true;
                instructionTTS.gameObject.SetActive(false);
            });
        });
    }

    public void OnComplete()
    {
        currentRound++;
        if (currentRound > allRoundLimts[currentIndex])
        {
            currentRound = 0;
            if (currentIndex < allRoundLimts.Length - 1)
            {
                currentIndex++;
            }
            else
            {
                currentIndex = 0;
            }
            doRotation.rotateSpeed *= -1;
        }
    }
}
