namespace Defra.Trade.Plants.SpecFlowBindings.Tables;

public class ChargeTable
{
    public ChargeTable()
    {
        WorkOrderTask = string.Empty;
        ChargeExempt = string.Empty;
    }

    public string ProductOrService { get; set; }
    public string PriceList { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitChargeAmount { get; set; }
    public string Unit { get; set; }
    public string WorkOrderTask { get; set; }
    public string ChargeExempt { get; set; }
}