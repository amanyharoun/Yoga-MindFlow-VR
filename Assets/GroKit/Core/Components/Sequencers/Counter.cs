namespace Core3lb
{
    using UnityEngine;
    using UnityEngine.Events;

    public class Counter : MonoBehaviour
    {
        public int starting = 0;
        public int current;
        public int max = 100;
        public DisplayTMPBase display;

        public UnityEvent Full;
        public UnityEvent Zeroed;



        void Start()
        {
            current = starting;
            UpdateText();
        }

        public virtual void UpdateText()
        {
            if(display)
            {
                display._SetTextSilent(current.ToString());
            }
        }

        public virtual void _Add(int value)
        {
            current += value;
            if (current >= max)
            {
                current = max;
                Full.Invoke();
            }
            UpdateText();
        }

        public virtual void _Set(int value)
        {
            current = value;
            _Subtract(0);
            _Add(0);
        }

        public virtual void _Subtract(int value)
        {
            current -= value;
            if (current <= 0)
            {
                current = 0;
                Zeroed.Invoke();
            }
            UpdateText();
        }

        [CoreButton]
        public void _AddOne()
        {
            _Add(1);
        }

        [CoreButton]
        public void _SubtractOne()
        {
            _Subtract(1);
        }


        [CoreButton]
        public virtual void _Reset()
        {
            _Set(starting);
            UpdateText();
        }
    }
}
