public enum TargetType {
    DisplayStand,
    FoodContainer,
    BoxContainer,
    Table,
    ParcelService,
    Room
}
public interface IOpenTarget{
    public TargetType Type { get; set; }
    public bool IsOpen { get; set; }
    public void ActiveObj(bool active, bool on = false);
}
