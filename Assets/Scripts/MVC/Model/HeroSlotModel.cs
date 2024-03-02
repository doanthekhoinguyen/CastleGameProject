namespace MVC.Model
{
    public enum HeroSlotState
    {
        None,
        Occupied
    }

    public class HeroSlotModel
    {
        public int SlotIndex;
        public HeroSlotState HeroSlotState;
        public HeroModel HeroModel;
    }
}