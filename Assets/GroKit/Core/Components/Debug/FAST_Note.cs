using UnityEngine;

namespace Core3lb
{
    //Simple Note section
    public class FAST_Note : MonoBehaviour
    {
        public enum eGizmoType
        {
            WireSphere,
            Sphere,
            Circle
        }

        [TextArea]
        public string Notes;
        public string URL;
        [CoreToggleHeader("Show Editor Gizmo")]
        public bool showGizmo;
        [CoreShowIf("showGizmo")]
        public Color gizmoColor = Color.red;
        [CoreShowIf("showGizmo")]
        public eGizmoType gizmoType = eGizmoType.WireSphere;
        [CoreShowIf("showGizmo")]
        public Color textColor = Color.black;
        [CoreShowIf("showGizmo")]
        public bool showText;


        [CoreButton("Open URL",true)]
        public void OpenAsURL()
        {
            if (!string.IsNullOrEmpty(URL))
            {
               Application.OpenURL(URL);
            }
        }

        public void OnDrawGizmos()
        {
            if(!showGizmo)
            {
                return;
            }
            // Save the original Gizmo color
            Color originalColor = Gizmos.color;

            // Set the Gizmo color to the specified color
            Gizmos.color = gizmoColor;

            switch (gizmoType)
            {
                case eGizmoType.WireSphere:
                    Gizmos.DrawWireSphere(transform.position, transform.localScale.x);
                    break;
                case eGizmoType.Sphere:
                    Gizmos.DrawSphere(transform.position, transform.localScale.x);
                    break;
                case eGizmoType.Circle:
                    DrawCircle(transform.position, transform.localScale.x);
                    break;
                default:
                    break;
            }
            // Restore the original Gizmo color
            Gizmos.color = originalColor;
#if UNITY_EDITOR
            // Display text if showText is enabled
            if (showText && !string.IsNullOrEmpty(Notes))
            {
                GUIStyle style = new GUIStyle
                {
                    fontSize = 15,
                    normal = new GUIStyleState { textColor = textColor },
                    alignment = TextAnchor.MiddleCenter,
                    fontStyle = FontStyle.Bold // Add this line to make the text bold
                };
                UnityEditor.Handles.Label(transform.position + Vector3.up * (transform.localScale.x + 0.5f), Notes, style);
            }
#endif
        }

        // Helper method to draw a circle in the scene view
        private void DrawCircle(Vector3 center, float radius)
        {
            int segments = 64; // Number of segments for the circle
            float angleStep = 360f / segments;

            Vector3 previousPoint = center + Vector3.right * radius;
            for (int i = 1; i <= segments; i++)
            {
                float angle = i * angleStep * Mathf.Deg2Rad;
                Vector3 nextPoint = center + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;

                Gizmos.DrawLine(previousPoint, nextPoint);
                previousPoint = nextPoint;
            }
        }

    }
}
