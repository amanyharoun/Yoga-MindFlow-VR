using Core3lb;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class YogaAudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public Slider slider;
    public AudioClip[] clips;
    public AudioClip[] meditationclips;
    public Sprite[] images;
    public GameObject audioButtonPrefab;
    public Transform buttonParent;
    public AudioSource meditation;
    void Start()
    {
        slider.onValueChanged.AddListener(SetAudioVolume);
        for (int i = 0; i < clips.Length; i++)
        {
            int ind = i;
            GameObject sb = Instantiate(audioButtonPrefab, buttonParent);
            sb.GetComponent<Image>().sprite = images[ind];
            sb.GetComponent<Button>().onClick.AddListener(() => { SetClip(ind); });
            sb.GetComponentInChildren<TextMeshProUGUI>().text = clips[ind].name = "";
        }
    }

    public void SetAudioVolume(float val)
    {
        meditation.volume = audioSource.volume = slider.value;
    }
    public void SetMeditationClip()
    {
        audioSource.Pause();
        meditation.clip = meditationclips[Random.Range(0, meditationclips.Length)];
        meditation.Play();
    }

    public void SetClip(int cIndex)
    {
        audioSource.clip = clips[cIndex];
        audioSource.Play();
    }
}
