using UnityEngine;

namespace Core3lb
{
    public class FAST_ColorChanger : MonoBehaviour
    {
        [Tooltip("Optional")]
        [CoreEmphasize]
        public Renderer myRender;

        void GetRenderer()
        {
            myRender = gameObject.GetComponentIfNull<Renderer>(myRender);
        }

        public void _Red()
        {
            ChangeColor(Color.red);
        }
        public void _Green()
        {
            ChangeColor(Color.green);
        }
        public void _Blue()
        {
            ChangeColor(Color.blue);
        }

        public void _Cyan()
        {
            ChangeColor(Color.cyan);
        }
        [CoreButton]
        public void _White()
        {
            ChangeColor(Color.white);
        }

        [CoreButton]
        public void _Random()
        {
            ChangeColor(Random.ColorHSV());
        }

        void ChangeColor(Color whatColor)
        {
            GetRenderer();
            myRender.material.color = whatColor;
        }
    }
}
