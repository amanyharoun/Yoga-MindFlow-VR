using Core3lb;
using DG.Tweening;
using Meta.WitAi.TTS.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeExersizeScript : MonoBehaviour
{
    public float timeToCompleteRotation;
    public Vector3 rotation;
    public int currentRound;
    public int currentIndex;
    public int max;
    public int[] allRoundLimts;
    public TextToSpeech instructionTTS;
    public DoRotation doRotation;
    public ResettableObject resettableObject;

    private void OnEnable()
    {
        Invoke(nameof(Initialize), 1f);

    }
    public void StartExersize()
    {
        StartRotation();
    }
    public void Initialize()
    {
        instructionTTS.gameObject.SetActive(true);
        instructionTTS.GetComponent<TTSSpeaker>().Events.OnAudioClipPlaybackFinished.RemoveAllListeners();
        instructionTTS.GetComponent<TTSSpeaker>().Events.OnAudioClipPlaybackFinished.AddListener((o) =>
        {
            currentRound = 0;
            currentIndex = 0;
            timeToCompleteRotation = 10;
            StartExersize();
        });
    }
    private void OnDisable()
    {
        doRotation.isRotating = false;
        instructionTTS.gameObject.SetActive(false);
        resettableObject._ResetObject();
    }
    public void StartRotation()
    {
        Vector3 direction = doRotation.rotateDirection;
        direction.z = 3;
        doRotation._SetRotationDirection(direction);
        doRotation.isRotating = true;
        doRotation.onCompleteRound.RemoveAllListeners();
        doRotation.onCompleteRound.AddListener(OnComplete);
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
                Vector3 dir = doRotation.rotateDirection;
                dir.z = 3;
                doRotation._SetRotationDirection(dir);
                doRotation.isRotating = true;
            }
            Vector3 direction = doRotation.rotateDirection;
            if (direction.z < max)
            {
                direction.z++;
            }
            doRotation._SetRotationDirection(direction);
        }
    }
}
