using UnityEngine;
using UnityEngine.Events;

namespace Core3lb
{
    public class DoRotation : MonoBehaviour
    {
        public bool isRotating;
        public Vector3 rotateDirection;
        public float rotateSpeed;
        public float accumulatedAngle;
        public UnityEvent onCompleteRound;
        public virtual void FixedUpdate()
        {
            if(isRotating)
            {
                float rotationStep = rotateSpeed * Time.fixedDeltaTime;

                // Rotate the object
                transform.Rotate(rotateDirection * rotationStep);

                // Track the accumulated rotation angle
                accumulatedAngle += (rotateDirection.z * rotationStep);

                // Check if a full rotation (360 degrees) is completed
                if (accumulatedAngle >= 360f)
                {
                    accumulatedAngle -= 360f; // Reset the accumulated angle for the next round
                    onCompleteRound?.Invoke(); // Trigger the UnityEvent
                }
            }
        }

        public virtual void _SetSpeed(float speed)
        {
            rotateSpeed = speed;
        }

        public virtual void _AddSpeed(float speed)
        {
            rotateSpeed += speed;
        }

        public virtual void _ToggleRotate()
        {
            isRotating = !isRotating; 
        }

        public virtual void _SetRotationDirection(Vector3 rotation)
        {
            rotateDirection = rotation;
        }
        public virtual void _SetRotate(bool chg)
        {
            isRotating = chg;
        }
    }
}
