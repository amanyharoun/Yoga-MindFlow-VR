using UnityEngine;
using UnityEngine.InputSystem;

namespace Core3lb
{
    public class PCInputEvent : BaseInputEvent
    {
        [CoreHeader("PC Settings")]
        public bool useReference = true;
        [CoreHideIf("useReference")]
        public Key keyboardKey = Key.V;
        [CoreHideIf("useReference")]
        public bool useMouseButtonInstead;
        [CoreHideIf("useReference")]
        public int mouseButton = 0;
        [CoreShowIf("useReference")]
        public InputActionReference actionRef;




        //Touch events can be added later
        public override bool GetInput()
        {
            if(useReference)
            {
                if (actionRef)
                {
                    return actionRef.action.IsPressed();
                }
                
            }
            else
            {
                if (useMouseButtonInstead)
                {
                    switch (mouseButton)
                    {
                        case 0:
                            return Mouse.current.leftButton.isPressed;   // Left mouse button
                        case 1:
                            return Mouse.current.rightButton.isPressed;  // Right mouse button
                        case 2:
                            return Mouse.current.middleButton.isPressed; // Middle mouse button
                        case 3:
                            return Mouse.current.forwardButton.isPressed; // Mouse button 4 (usually "Forward" on some mice)
                        case 4:
                            return Mouse.current.backButton.isPressed;   // Mouse button 5 (usually "Back" on some mice)
                        default:
                            Debug.LogWarning("Invalid mouse button index");
                            return false;
                    }
                }
                else
                {
                    return Keyboard.current[keyboardKey].isPressed;
                }
            }
            return false;
        }
    }
}
