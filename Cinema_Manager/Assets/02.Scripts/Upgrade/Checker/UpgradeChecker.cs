using UIToolkit;
using UnityEngine;


public class UpgradeChecker : CheckerArea {
    public UpgradeViewType type;

    public override void EnterInteraction() {
        FindViewRegister(type);
    }
    private void FindViewRegister(UpgradeViewType type) {
        switch (type) {
            case UpgradeViewType.PlayerUpgradeView:
                MainEvents.PlayerUpgradeViewShow?.Invoke();
                break;
            case UpgradeViewType.EmployeeUpgradeView:
                MainEvents.EmployeeUpgradeViewShow?.Invoke();
                break;
            case UpgradeViewType.MachineUpgradeView:
                MainEvents.MachineUpgradeViewShow?.Invoke();
                break;
            default:
                Debug.LogWarning("지금 들어온 type은 띄우지 못하는 view이거나 case를 추가하지 않음");
                break;
        }
    }

    public override void ExitInteraction() {
        MainEvents.CloseCurrentEvent?.Invoke();
    }
}
    