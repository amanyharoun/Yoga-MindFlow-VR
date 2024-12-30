
namespace Core3lb
{
    public class MetaGestureEvent : BaseInputEvent
    {
        [CoreHeader("Settings")]
        public MetaGestureInput.eHandType whichHand = MetaGestureInput.eHandType.leftHand;
        public InputXR.GestureType gestureType = InputXR.GestureType.GrabCombo;

        public override bool GetInput()
        {
            return MetaGestureInput.instance.GetInput(whichHand, gestureType);

        }
    }
}
