using Defra.Trade.Plants.Model;

namespace Defra.Trade.Plants.SpecFlowBindings.Tables;

public class ImportNotificationTable
{
    public string Version { get; set; }
    public trd_plantsimportnotification_trd_ipaffsstatus? NotificationStatus { get; set; }
    public trd_plantsimportnotificationState Status { get; set; }
    public trd_plantsimportnotification_statuscode StatusReason { get; set; }
}