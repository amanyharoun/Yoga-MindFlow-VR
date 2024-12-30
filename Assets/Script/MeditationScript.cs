using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeditationScript : MonoBehaviour
{
    public TextToSpeech instructionTTS;
    public TextToSpeech instruction2TTS;
    public TextToSpeech instruction3TTS;
    public AudioSource meditationAudioSource;
    public AudioClip[] clips;
    public YogaAudioManager audioManager;
    void Start()
    {
        
    }

    private void OnEnable()
    {
        Initialize();
    }

    private void OnDisable()
    {
        audioManager.audioSource.Play();
        instructionTTS.gameObject.SetActive(false);
        instruction2TTS.gameObject.SetActive(false);
        instruction3TTS.gameObject.SetActive(false);
    }

    public void PlayMeditation()
    {
        audioManager.SetMeditationClip();
    }
    public void Initialize()
    {
        instructionTTS.gameObject.SetActive(true);
        instruction2TTS.gameObject.SetActive(false);
        instruction3TTS.gameObject.SetActive(false);
    }
}
