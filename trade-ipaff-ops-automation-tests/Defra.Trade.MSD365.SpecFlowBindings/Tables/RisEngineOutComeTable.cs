using Defra.Trade.Plants.Model;

namespace Defra.Trade.Plants.SpecFlowBindings.Tables;

public class RisEngineOutComeTable
{
    public string Commodity { get; set; }
    public trd_plantsimportcommodityline_trd_inspectionclassification InspectionClassification { get; set; }
    public string InspectionRequired { get; set; }
}