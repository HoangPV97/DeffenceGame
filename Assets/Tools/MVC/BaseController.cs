using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour, INotification
{

    public const string Room_OnOpenRoom = "Room_OnOpenRoom";
    public const string Room_OnOpenRoomByGold = "Room_OnOpenRoomByGold";
    public const string Room_OnUpDateData = "Room_OnUpDateData";
    public const string Room_OnRefreshUI = "Room_OnRefreshUI";
    public const string Room_OnReCheckLockState = "Room_OnReCheckLockState";
    public const string Room_OnSellingProduct = "Room_OnSellingProduct";
    public const string Room_OnReLoadStock = "Room_OnReLoadStock";
    public const string Room_OnReLoadStockByClick = "Room_OnReLoadStockByClick";
    public const string Room_OnUpgradeLevel = "Room_OnUpgradeLevel";
    public const string Room_OnOpenWorkerAssignment = "Room_OnOpenWorkerAssignment";
    public const string Room_OnOpenGuardAssignment = "Room_OnOpenGuardAssignment";
    public const string Room_OnWatchAdsToReduce30Min = "Room_OnWatchAdsToReduce30Min";
    public const string Room_OnOpenRoomAfterIncident = "Room_OnOpenRoomAfterIncident";

    public const string Room_OnAssignWorker = "Room_OnAssignWorker";
    public const string Room_OnUnAssignWorker = "Room_OnUnAssignWorker";
    public const string Room_OnUnAssignWorkerIndirect = "Room_OnUnAssignWorkerIndirect";
    public const string Room_OnSpawnTip = "Room_OnSpawnTip";
    public const string Room_OnTipClick = "Room_OnTipClick";
    public const string Room_RecieveFlyingHeart = "Room_RecieveFlyingHeart";
    public const string Room_OnBtnEmotionClick = "Room_OnBtnEmotionClick";
    public const string Room_OnFinishFever = "Room_OnFinishFever";
    public const string Room_SetSadWorker = "Room_SetSadWorker";
    public const string UnLockAssignmentWorkerTutorial = "UnLockAssignmentWorkerTutorial";

    public const string WorkerAssignment_OnOpenWorkerList = "WorkerAssignment_OnOpenWorkerList";
    public const string WorkerAssignment_OpenWorkerDetail = "WorkerAssignment_OpenWorkerDetail";
    public const string WorkerAssignment_OnAssigWorker = "WorkerAssignment_OnAssigWorker";
    public const string WorkerAssignment_OnRemoveWorkerClick = "WorkerAssignment_OnRemoveWorkerClick";
    public const string WorkerAssignment_OnEvictWorkerClick = "WorkerAssignment_OnEvictWorkerClick";
    public const string WorkerAssignment_OnEvictWorkerClick2 = "WorkerAssignment_OnEvictWorkerClick2";

    public const string GuardAssignment_OpenGuardDetail = "GuardAssignment_OpenGuardDetail";
    public const string GuardAssignment_OnAssigGuard = "GuardAssignment_OnAssigGuard";
    public const string GuardAssignment_OnRemoveGuardClick = "GuardAssignment_OnRemoveGuardClick";
    public const string GuardAssignment_OnEvictGuardClick = "GuardAssignment_OnEvictGuardClick";
    public const string GuardAssignment_OnLevelUpGuard = "GuardAssignment_OnLevelUpGuard";
    public const string GuardAssignment_OnEvictGuardClick2 = "GuardAssignment_OnEvictGuardClick2";

    public const string Room_OnAssignGuard = "Room_OnAssignGuard";
    public const string Room_OnUnAssignGuard = "Room_OnUnAssignGuard";
    public const string Room_OnUnAssignGuardIndirect = "Room_OnUnAssignGuardIndirect";

    public const string Room_OnSpawnCustomer = "Room_OnSpawnCustomer";
    public const string Room_OnCustomerOutOfTime = "Room_OnCustomerOutOfTime";

    public const string Incident_Start = "Incident_Start";
    public const string Incident_Tap = "Incident_Tap";
    public const string ReSet_Room_Profit_Text = "ReSetRoomProfitText";

    public const string LockBtnUpgrade = "LockBtnUpgrade";
    public const string Check_Room_Lock_State = "Check_Room_Lock_State";

    public const string Merge_Select_Worker = "Merge_Select_Worker";
    public const string Merge_Select_Guard = "Merge_Select_Guard";

    public const string Add_Guard_Exp = "AddGuardExp";
    public virtual void Notify(string p_event_path, object p_target, params object[] p_data)
    {
        throw new System.NotImplementedException();
    }
}
