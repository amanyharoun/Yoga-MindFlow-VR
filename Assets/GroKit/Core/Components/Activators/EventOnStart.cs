using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Core3lb
{
    public class EventOnStart : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent startEvent;
        [CoreToggleHeader("More Events")]
        [SerializeField]
        public bool moreEvents;
        [CoreShowIf("moreEvents")]
        [SerializeField]
        private UnityEvent awakeEvent;
        [CoreShowIf("moreEvents")]
        [SerializeField]
        private UnityEvent delayStartEvent;
        [CoreShowIf("moreEvents")]
        [SerializeField]
        private float delay = 0;

        private void Awake()
        {
            awakeEvent.Invoke();
        }

        IEnumerator Start()
        {
            startEvent.Invoke();
            yield return new WaitForSeconds(delay);
            delayStartEvent.Invoke();
        }
    }
}
