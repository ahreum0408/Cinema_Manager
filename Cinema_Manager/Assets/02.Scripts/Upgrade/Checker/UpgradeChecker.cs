using UIToolkit;
using UnityEngine;


public class UpgradeChecker : CheckerArea {
    public UpgradeViewType viewType;

    public override void EnterInteraction(Collider collider) {
        FindViewRegister(viewType);
    }
    private void FindViewRegister(UpgradeViewType type) {
        switch (type) {
            case UpgradeViewType.PlayerUpgradeView:
                MainEvents.PlayerUpgradeViewShow?.Invoke();
                break;
            case UpgradeViewType.EmployeeUpgradeView:
                MainEvents.EmployeeUpgradeViewShow?.Invoke();
                break;
            case UpgradeViewType.TruckMachineUpgradeView:
                MainEvents.TruckMachineUpgradeViewShow?.Invoke();
                break;
            case UpgradeViewType.PackageMachineUpgradeView:
                MainEvents.PackageMachineUpgradeViewShow?.Invoke();
                break;
            default:
                Debug.LogWarning("지금 들어온 type은 띄우지 못하는 view이거나 case를 추가하지 않음");
                break;
        }
    }

    public override void ExitInteraction(Collider collider) {
        MainEvents.CloseCurrentEvent?.Invoke();
    }
}
    