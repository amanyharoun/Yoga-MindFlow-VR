using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace Core3lb
{


    public class QuickLoopedAudioPlayer : MonoBehaviour
    {
        private AudioSource audioSource1; // First audio source
        private AudioSource audioSource2; // Second audio source
        [Tooltip("Audio To Play On Start")]
        public AudioClip startAudio; // Single audio clip to store music
        public float crossfadeDuration = 1f; // Duration for crossfade
        


        public AudioSource audioSourceTemplate;
        public AudioMixerGroup mixerGroup;
        public bool is2D;

        void Start()
        {
            // Create audio sources
            audioSource1 = gameObject.AddComponent<AudioSource>();
            audioSource2 = gameObject.AddComponent<AudioSource>();

            if(audioSourceTemplate)
            {
                audioSource1.CopyFrom(audioSourceTemplate);
                audioSource2.CopyFrom(audioSourceTemplate);
            }
            if(mixerGroup)
            {
                audioSource1.outputAudioMixerGroup = mixerGroup;
                audioSource2.outputAudioMixerGroup = mixerGroup;
            }
            if(is2D)
            {
                audioSource1.spatialBlend = 0;
                audioSource2.spatialBlend = 0;
            }
            audioSource1.loop = true;
            audioSource2.loop = true;
            // Play the music clip
            PlayMusic();
        }

        public void _FadeToOff()
        {
            // Find source playing and fade it out
            if (audioSource1.isPlaying)
            {
                StartCoroutine(FadeOut(audioSource1));
            }
            else if (audioSource2.isPlaying)
            {
                StartCoroutine(FadeOut(audioSource2));
            }
        }

        private IEnumerator FadeOut(AudioSource audioSource)
        {
            float startVolume = audioSource.volume;

            while (audioSource.volume > 0)
            {
                audioSource.volume -= startVolume * Time.deltaTime / crossfadeDuration;
                yield return null;
            }

            audioSource.Stop();
            audioSource.volume = startVolume; // Reset volume after fading out
        }

        public void _ChangeClip(AudioClip newClip)
        {
            _FadeToOff();
            StartCoroutine(CrossFade(newClip));
        }

        // Decide who is playing and pick which source
        private IEnumerator CrossFade(AudioClip newClip)
        {
            AudioSource activeSource = audioSource1.isPlaying ? audioSource1 : audioSource2;
            AudioSource newSource = audioSource1.isPlaying ? audioSource2 : audioSource1;

            newSource.clip = newClip;
            newSource.Play();
            newSource.volume = 0;

            float time = 0;
            while (time < crossfadeDuration)
            {
                time += Time.deltaTime;
                float t = time / crossfadeDuration;
                activeSource.volume = 1 - t;
                newSource.volume = t;
                yield return null;
            }

            activeSource.Stop();
            activeSource.volume = 1; // Reset volume after fading
            newSource.volume = 1; // Make sure the new track is full volume
        }

        private void PlayMusic()
        {
            audioSource1.clip = startAudio;
            audioSource1.Play();
        }
    }
}