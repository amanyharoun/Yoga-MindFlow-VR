using System.Collections.Generic;
using System;
using UnityEngine.InputSystem;
using UnityEngine;

namespace Core3lb
{
    //This is a copy of InputActionManager from XRIT a fast way to enable all the input actions
    public class InputActionEnabler : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Input action assets to affect when inputs are enabled or disabled.")]
        List<InputActionAsset> m_ActionAssets;
        /// <summary>
        /// Input action assets to affect when inputs are enabled or disabled.
        /// </summary>
        public List<InputActionAsset> actionAssets
        {
            get => m_ActionAssets;
            set => m_ActionAssets = value ?? throw new ArgumentNullException(nameof(value));
        }

        private static bool isKeyboardAndMouse;

        public static bool isUsingKeyboard
        {
            get
            {
                return isKeyboardAndMouse;
            }
        }

        private static string lastInput;
        [CoreReadOnly]
        public string lastInputDevice;
        public static string lastInputType
        {
            get
            {
                return lastInput;
            }
        }


        private void InputActionChangeCallback(object obj, InputActionChange change)
        {
            if (change == InputActionChange.ActionPerformed)
            {
                InputAction receivedInputAction = (InputAction)obj;
                InputDevice lastDevice = receivedInputAction.activeControl.device;
                lastInput = lastDevice.name;
                lastInputDevice = lastInput;
                isKeyboardAndMouse = lastDevice.name.Equals("Keyboard") || lastDevice.name.Equals("Mouse");
            }
        }

        /// <summary>
        /// See <see cref="MonoBehaviour"/>.
        /// </summary>
        protected void OnEnable()
        {
            InputSystem.onActionChange += InputActionChangeCallback;
            EnableInput();
        }

        /// <summary>
        /// See <see cref="MonoBehaviour"/>.
        /// </summary>
        protected void OnDisable()
        {
            DisableInput();
        }
        public void EnableInput()
        {
            if (m_ActionAssets == null)
                return;

            foreach (var actionAsset in m_ActionAssets)
            {
                if (actionAsset != null)
                {
                    actionAsset.Enable();
                }
            }
        }

        /// <summary>
        /// Disable all actions referenced by this component.
        /// </summary>
        /// <remarks>
        /// This function will automatically be called when this <see cref="InputActionManager"/> component is disabled.
        /// However, this method can be called to disable input manually, such as after enabling it with <see cref="EnableInput"/>.
        /// <br />
        /// Disabling inputs only disables the action maps contained within the referenced
        /// action map assets (see <see cref="actionAssets"/>).
        /// </remarks>
        /// <seealso cref="EnableInput"/>
        public void DisableInput()
        {
            if (m_ActionAssets == null)
                return;

            foreach (var actionAsset in m_ActionAssets)
            {
                if (actionAsset != null)
                {
                    actionAsset.Disable();
                }
            }
        }
    }
}