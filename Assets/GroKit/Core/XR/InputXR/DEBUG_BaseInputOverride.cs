using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core3lb
{
    public class DEBUG_BaseInputOverride : MonoBehaviour
    {
        public Key key = Key.V;
        public bool controlON;
        public bool getFromChildrenOnly;
        public bool autoGet;

        // Update is called once per frame
        public BaseInputEvent toControl;
        public List<BaseInputEvent> events;

        public void Awake()
        {
            if (controlON)
            {
                Debug.LogError("DEBUG_BaseInputOverride is on and ACTIVE", gameObject);
            }
        }
        void Update()
        {
            if (!controlON)
            {
                return;
            }
            if (Keyboard.current[key].wasPressedThisFrame)
            {
                Debug.Log($"Key {key} was pressed.");
                ControlInputs();
            }
        }

        public void ControlInputs()
        {
            if (autoGet)
            {
                if (getFromChildrenOnly)
                {
                    events = GetComponentsInChildren<BaseInputEvent>().ToList();
                }
                else
                {
                    events = FindObjectsOfType<BaseInputEvent>().ToList();
                }
            }
            if (toControl)
            {
                toControl.inputProcessor.DEBUG_OverrideInputs();
            }
            foreach (BaseInputEvent e in events)
            {
                e.inputProcessor.DEBUG_OverrideInputs();
            }
        }

    }
}
