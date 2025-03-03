namespace Game.Scripts.Interface
{
    public interface IToggle
    {
        public bool IsFocused { get; set; }
        public bool IsToggled { get; set; }
        public void Toggle();
    }
}