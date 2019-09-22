namespace CVD_Calc.Models
{
    public enum MenuItemType
    {
        Home,
        Configs,
        Interval_Timer,
        Profile,
        About
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }
        public string Title { get; set; }
    }
}
