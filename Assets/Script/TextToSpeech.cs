using System;
using System.Collections;
using UnityEngine;
using Meta.WitAi.TTS.Utilities;
using TMPro;

public class TextToSpeech : MonoBehaviour
{
    public bool playFromString;
    public string speechStr;
    [SerializeField] private TTSSpeaker _speaker;

    // Default input
    [SerializeField] private TextMeshProUGUI _input;

    // Queue button that will not stop previous clip
    // Async toggle that will play a clip on completion
    [SerializeField] private AudioClip _asyncClip;

    [SerializeField] private string _dateId = "[DATE]";
    [SerializeField] private string[] _queuedText;

    // States
    public bool queued;
    public bool async;
    public string _voice;
    public bool _loading;
    public bool _speaking;
    public bool _paused;

    public float delay;
    // Add delegates
    private void OnEnable()
    {
        RefreshStopButton();
        RefreshPauseButton();
        Invoke(nameof(SpeakTest), delay);
    }
    // Stop click
    private void StopClick() => _speaker.Stop();
    // Pause click
    private void PauseClick()
    {
        if (_speaker.IsPaused)
        {
            _speaker.Resume();
        }
        else
        {
            _speaker.Pause();
        }
    }

    public void SpeakTest()
    {
        string str = "";
        if (playFromString)
        {
            str = speechStr;
        }
        else { 
            str = _input.text;
        }
        _speaker.SpeakQueued(FormatText(str));
    }
    // Speak phrase click
    private void SpeakClick()
    {
        // Speak phrase
        string phrase = FormatText(_input.text);

        // Speak async
        if (async)
        {
            StartCoroutine(SpeakAsync(phrase, queued));
        }
        // Speak queued
        else if (queued)
        {
            _speaker.SpeakQueued(phrase);
        }
        // Speak
        else
        {
            _speaker.Speak(phrase);
        }

        // Queue additional phrases
        if (_queuedText != null && _queuedText.Length > 0 && queued)
        {
            foreach (var text in _queuedText)
            {
                _speaker.SpeakQueued(FormatText(text));
            }
        }
    }
    // Speak async
    private IEnumerator SpeakAsync(string phrase, bool queued)
    {
        // Queue
        if (queued)
        {
            yield return _speaker.SpeakQueuedAsync(new string[] { phrase });
        }
        // Default
        else
        {
            yield return _speaker.SpeakAsync(phrase);
        }

        // Play complete clip
        if (_asyncClip != null)
        {
            _speaker.AudioSource.PlayOneShot(_asyncClip);
        }
    }
    // Format text with current datetime
    private string FormatText(string text)
    {
        string result = text;
        if (result.Contains(_dateId))
        {
            DateTime now = DateTime.UtcNow;
            string dateString = $"{now.ToLongDateString()} at {now.ToLongTimeString()}";
            result = text.Replace(_dateId, dateString);
        }
        return result;
    }

    // Preset text fields
    private void Update()
    {
        // On preset voice id update
        if (!string.Equals(_voice, _speaker.VoiceID))
        {
            _voice = _speaker.VoiceID;
        }
        // On state changes
        if (_loading != _speaker.IsLoading)
        {
            RefreshStopButton();
        }
        if (_speaking != _speaker.IsSpeaking)
        {
            RefreshStopButton();
        }
        if (_paused != _speaker.IsPaused)
        {
            RefreshPauseButton();
        }
    }
    // Refresh interactable based on states
    private void RefreshStopButton()
    {
        _loading = _speaker.IsLoading;
        _speaking = _speaker.IsSpeaking;
    }
    // Refresh text based on pause state
    private void RefreshPauseButton()
    {
        _paused = _speaker.IsPaused;
    }
}
