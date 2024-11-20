using System.Collections;

public enum TargetType {
    DisplayStand,
    FoodContainer,
    BoxContainer,
    Table,
    ParcelService,
    Room,
    Counter,
    TrashBin
}
public interface IOpenTarget{
    public TargetType Type { get; set; }
    public bool IsOpen { get; set; }
    public void ActiveObj(bool active, bool on = false);

    // 켜졌을 때 사이즈 팝 효과
    public void ScaleSetting();
}
