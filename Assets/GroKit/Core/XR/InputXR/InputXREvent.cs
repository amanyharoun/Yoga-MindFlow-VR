using UnityEngine;

namespace Core3lb
{
    [CoreClassName("Input XR Event")]
    public class InputXREvent : BaseInputEvent
    {
        [CoreHeader("InputXR")]
        public InputXR.Controller controller;
        [CoreHideIf("useOverride")]
        public InputXR.Button buttons = InputXR.Button.Grab;
        [Tooltip("Use these overrides instead of the typical options")]
        public bool useReference;
        [CoreShowIf("useReference")]
        [Tooltip("Use these overrides instead of the typical options")]
        public InputReferencesXR overrideReference;

        public override bool GetInput()
        {
            if(useReference)
            {
                return InputXR.HandleReference(overrideReference, controller, InputXR.InputRequest.Get);
            }
            else
            {
                return InputXR.instance.GetButton(buttons, controller,forceBase);
            }
        }
    }
}
