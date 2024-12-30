using UnityEngine;

namespace Core3lb
{
    public class Facing : MonoBehaviour
    {
        [Tooltip("If Follow main camera this will be ignored")]
        [CoreHeader("Settings")]
        public Transform faceTarget;
        public bool followMainCamera = true;
        public bool flipZ;
        public float faceSpeed = 10.0f;
        public bool isFacing = true;
        // Lock axis Booleans
        public bool onlyXRotation;
        public bool onlyYRotation;
        [Tooltip("For example text mesh pro needs to be flipped")]
        [CoreHeader("Scale")]
        public bool scaleToDistance;
        public float ScaleMultiplier = 0.5f;
        private Vector3 initialScale;
        Vector3 scaleTo;

        void Awake()
        {
            initialScale = transform.localScale;
        }

        private void FixedUpdate()
        {
            if (isFacing)
            {
                FaceObject();
            }
            if (scaleToDistance)
            {
                ScaleObject();
            }
        }

        void FaceObject()
        {
            Vector3 targetPosition;
            if (followMainCamera && Camera.main)
            {
                targetPosition = Camera.main.transform.position;
            }
            else
            {
                targetPosition = faceTarget.transform.position;
            }
            Vector3 scaleSystem = Vector3.one;
            // Lock axes if needed
            if (onlyXRotation) scaleSystem.x = 0;
            if (onlyYRotation) scaleSystem.y = 0;
            if (flipZ)
            {
                scaleSystem *= -1;
            }
            Quaternion targetRotation = transform.LookAtWithConstraint(targetPosition, scaleSystem);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * faceSpeed);
        }

        void ScaleObject()
        {
            if (followMainCamera && Camera.main)
            {
                scaleTo = Camera.main.transform.position;
            }
            else
            {
                scaleTo = faceTarget.transform.position;
            }
            float distance = Vector3.Distance(scaleTo, transform.position);
            transform.localScale = Mathf.Sqrt(distance) * ScaleMultiplier * initialScale;
        }

        public void _FaceTo(GameObject loc)
        {
            faceTarget = loc.transform;
            isFacing = true;
        }

        public void _FacingToggle()
        {
            isFacing = !isFacing;
        }

        public void _FacingOff()
        {
            isFacing = false;
        }

        public void _FacingOn()
        {
            isFacing = true;
        }
    }
}

