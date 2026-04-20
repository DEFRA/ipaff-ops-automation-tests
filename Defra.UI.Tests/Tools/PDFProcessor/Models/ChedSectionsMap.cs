using Newtonsoft.Json;

namespace Defra.UI.Tests.Tools.PDFProcessor.Models
{
    // Example of a strongly typed wrapper if you wanted to map the specific sections
    // You would deserialize the "Sections" dictionary into this object instead of Dictionary<string, ChedSection>
    public class ChedSectionsMap
    {
        [JsonProperty("PartIDescriptionOfConsignment")]
        public ChedSection PartIDescriptionOfConsignment { get; set; }

        [JsonProperty("I1ConsignorExporter")]
        public ChedSection ConsignorExporter { get; set; }

        [JsonProperty("I2ChedReference")]
        public ChedSection ChedReference { get; set; }

        [JsonProperty("I3LocalReference")]
        public ChedSection LocalReference { get; set; }

        [JsonProperty("I4BorderControlPostControlPointControlUnit")]
        public ChedSection BorderControlPost { get; set; }

        [JsonProperty("I5BorderControlPostControlPointControlUnitCode")]
        public ChedSection BorderControlPostCode { get; set; }

        [JsonProperty("I6ConsigneeImporter")]
        public ChedSection ConsigneeImporter { get; set; }
                       
        [JsonProperty("I7PlaceOfDestination")]
        public ChedSection PlaceOfDestination { get; set; }

        [JsonProperty("I8OperatorResponsibleForTheConsignment")]
        public ChedSection OperatorResponsible { get; set; }

        [JsonProperty("I9AccompanyingDocuments")]
        public ChedSection AccompanyingDocuments { get; set; }

        [JsonProperty("I10PriorNotification")]
        public ChedSection PriorNotification { get; set; }

        [JsonProperty("I11CountryOfOrigin")]
        public ChedSection CountryOfOrigin { get; set; }

        [JsonProperty("I13MeansOfTransport")]
        public ChedSection MeansOfTransport { get; set; }

        [JsonProperty("I12RegionOfOrigin")]
        public ChedSection RegionOfOrigin { get; set; }

        [JsonProperty("I14CountryOfDispatch")]
        public ChedSection CountryOfDispatch { get; set; }

        [JsonProperty("I15EstablishmentsOfOrigin")]
        public ChedSection EstablishmentsOfOrigin { get; set; }

        [JsonProperty("I16TransportConditions")]
        public ChedSection TransportConditions { get; set; }

        [JsonProperty("I17ContainerNoSealNo")]
        public ChedSection ContainerNoSealNo { get; set; }

        [JsonProperty("I18GoodsCertifiedAs")]
        public ChedSection GoodsCertifiedAs { get; set; }

        [JsonProperty("I19ConformityOfTheGoods")]
        public ChedSection ConformityOfTheGoods { get; set; }
        
        [JsonProperty("I22Market")]
        public ChedSection NonInternalMarket { get; set; }

        [JsonProperty("I23ForInternalMarket")]
        public ChedSection ForInternalMarket { get; set; }

        [JsonProperty("I27MeansOfTransportAfterBcpStorage")]
        public ChedSection MeansOfTransportAfterBcpStorage { get; set; }

        [JsonProperty("I28Transporter")]
        public ChedSection Transporter { get; set; }

        [JsonProperty("I29DateOfDeparture")]
        public ChedSection DateOfDeparture { get; set; }

        [JsonProperty("I31DescriptionOfTheGoods")]
        public List<ChedSection> DescriptionOfTheGoods { get; set; }

        [JsonProperty("I32TotalNumberOfPackages")]
        public ChedSection TotalNumberOfPackages { get; set; }

        [JsonProperty("I34TotalNetWeight")]
        public ChedSection TotalNetWeight { get; set; }

        [JsonProperty("I34TotalGrossWeight")]
        public ChedSection TotalGrossWeight { get; set; }

        [JsonProperty("I35Declaration")]
        public ChedSection Declaration { get; set; }
        
        [JsonProperty("I20ForTranshipmentOnwardTravel")]
        public ChedSection TranshipmentOnwardTravel { get; set; }

        // Part II

        [JsonProperty("Ii300324CartonFranceFrPlantsIn")]
        public ChedSection CHEDPPPageII { get; set; }

        [JsonProperty("PartIIControls")]
        public ChedSection PartIIControls { get; set; }

        [JsonProperty("II1PreviousChed")]
        public ChedSection II1PreviousChed { get; set; }

        [JsonProperty("II2ChedReference")]
        public ChedSection II2ChedReference { get; set; }

        [JsonProperty("II24SubsequentCheds")]
        public ChedSection II24SubsequentCheds { get; set; }

        [JsonProperty("II25BcpReferenceNumber")]
        public ChedSection II25BcpReferenceNumber { get; set; }

        [JsonProperty("II3DocumentaryCheck")]
        public ChedSection DocumentaryCheck { get; set; }

        [JsonProperty("II4IdentityCheck")]
        public ChedSection IdentityCheck { get; set; }

        [JsonProperty("II5PhysicalCheck")]
        public ChedSection PhysicalCheck { get; set; }

        [JsonProperty("II6LaboratoryTests")]
        public ChedSection LaboratoryTests { get; set; }

        [JsonProperty("II12AcceptableForInternalMarket")]
        public ChedSection AcceptableForInternalMarket { get; set; }

        [JsonProperty("II20IdentificationOfBcp")]
        public ChedSection IdentificationOfBcp { get; set; }

        [JsonProperty("II21CertifyingOfficer")]
        public ChedSection CertifyingOfficer { get; set; }

        [JsonProperty("II22InspectionFees")]
        public ChedSection InspectionFees { get; set; }

        [JsonProperty("II23CustomsDocumentReference")]
        public ChedSection CustomsDocumentReference { get; set; }

        // Part III
        [JsonProperty("PartIIIFollowUp")]
        public ChedSection PartIIIFollowUp { get; set; }

        [JsonProperty("III1PreviousChed")]
        public ChedSection III1PreviousChed { get; set; }

        [JsonProperty("III2ChedReference")]
        public ChedSection III2ChedReference { get; set; }

        [JsonProperty("III24SubsequentChed")]
        public ChedSection III24SubsequentChed { get; set; }

        [JsonProperty("III4DetailsOnReDispatching")]
        public ChedSection DetailsOnReDispatching { get; set; }

        [JsonProperty("III5FollowUp")]
        public ChedSection FollowUp { get; set; }

        [JsonProperty("III6OfficialInspector")]
        public ChedSection OfficialInspector { get; set; }

        [JsonProperty("II11AcceptableForTransit")]
        public ChedSection AcceptableForTransit { get; set; }

        [JsonProperty("II9AcceptableForTranshipment")]
        public ChedSection AcceptableForTranshipment { get; set; }

        [JsonProperty("II16NotAcceptable")]
        public ChedSection NotAcceptable { get; set; }
        
        [JsonProperty("II17ReasonForRefusal")]
        public ChedSection ReasonForRefusal { get; set; }
        
        [JsonProperty("IVResults")]
        public ChedSection LabResults { get; set; } 
        
        [JsonProperty("IVRequested analysis")]
        public ChedSection RequestedAnalysis { get; set; }
        
        [JsonProperty("IVIdentificationOfTheSample")]
        public ChedSection IdentificationOfTheSample { get; set; }
        
        [JsonProperty("IVReferences")]
        public ChedSection References { get; set; }
        
        [JsonProperty("IVFicheForSampling")]
        public ChedSection FicheForSampling { get; set; }
    }
}