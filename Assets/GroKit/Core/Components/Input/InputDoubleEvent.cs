namespace Core3lb
{
    //For combining two inputs
    public class InputDoubleEvent : BaseInputEvent
    {
        public BaseInputEvent button1;
        public BaseInputEvent button2;
        public bool OrNotAnd = false;
        // Start is called before the first frame update

        public override bool GetInput()
        {
            if(OrNotAnd)
            {
                return button1.GetInput() || button2.GetInput();
            }
            return button1.GetInput() && button2.GetInput();
        }
    }
}
