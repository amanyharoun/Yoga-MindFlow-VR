using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Core3lb
{
    //This script is very inefficent for advanced control please use the GrokitAudioSystem
    public class QuickSFX : MonoBehaviour
    {
        [Tooltip("This will pick one at random")]
        public List<AudioClip> audioClips;
        public bool is2D = true;

        [CoreHeader("Advanced")]
        [CoreEmphasize]
        public AudioSource audioSourceTemplate;
        public AudioMixerGroup mixerOverride;
        [CoreMinMax(-3, 3)]
        public Vector2 pitchRange = new Vector2(1f, 1f);
        [CoreMinMax(0, 1)]
        public Vector2 volumeRange = new Vector2(1f, 1f);
        public GameObject cachedObject;
        public AudioSource cachedSource;

        [CoreButton]
        public void _PlayOneShot()
        {
            if (audioClips.Count == 0) { Debug.Log("No Clips to Play"); return; }

            int randomIndex = Random.Range(0, audioClips.Count);
            _PlayOneShot(audioClips[randomIndex],transform.position);
        }

        public void _PlayOneShot(AudioClip myClip)
        {
            if (audioClips.Count == 0) { Debug.Log("No Clips to Play"); return; }

            int randomIndex = Random.Range(0, audioClips.Count);
            _PlayOneShot(myClip, transform.position);
        }

        public void _PlayOneShotHere(Transform where)
        {
            if (audioClips.Count == 0) { Debug.Log("No Clips to Play"); return; }

            int randomIndex = Random.Range(0, audioClips.Count);
            _PlayOneShot(audioClips[randomIndex],where.position);
        }

        // Play the specified audio clip by creating a new AudioSource
        public void _PlayOneShot(AudioClip clip,Vector3 position)
        {
            if (clip == null) return;
            if(cachedObject == null)
            {
                cachedObject = new GameObject("QuickSFX"); // Create a new GameObject
                cachedObject.transform.parent = transform;
                cachedSource = cachedObject.AddComponent<AudioSource>();
                if (audioSourceTemplate)
                {
                    cachedSource.CopyFrom(audioSourceTemplate);
                }
                if (is2D)
                {
                    cachedSource.spatialBlend = 0;
                }
                else
                {
                    cachedSource.spatialBlend = 1;
                }
                if (mixerOverride)
                {
                    cachedSource.outputAudioMixerGroup = mixerOverride;
                }
                cachedSource.loop = false;
            }
            cachedObject.transform.position = position;
            // Set random pitch and volume
            cachedSource.pitch = pitchRange.Randomize();
            cachedSource.volume = volumeRange.Randomize();
            cachedSource.PlayOneShot(clip);
        }
    }
}
