using Defra.UI.Tests.Tools.PDFProcessor.Models;
using Newtonsoft.Json;

namespace PdfExtraction.Models
{
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

        // Part II
        [JsonProperty("PartIiControls")]
        public ChedSection PartIiControls { get; set; }

        [JsonProperty("Ii1PreviousChed")]
        public ChedSection Ii1PreviousChed { get; set; }

        [JsonProperty("Ii2ChedReference")]
        public ChedSection Ii2ChedReference { get; set; }

        [JsonProperty("Ii24SubsequentCheds")]
        public ChedSection Ii24SubsequentCheds { get; set; }

        [JsonProperty("Ii25BcpReferenceNumber")]
        public ChedSection Ii25BcpReferenceNumber { get; set; }

        [JsonProperty("Ii3DocumentaryCheck")]
        public ChedSection DocumentaryCheck { get; set; }

        [JsonProperty("Ii4IdentityCheck")]
        public ChedSection IdentityCheck { get; set; }

        [JsonProperty("Ii5PhysicalCheck")]
        public ChedSection PhysicalCheck { get; set; }

        [JsonProperty("Ii6LaboratoryTests")]
        public ChedSection LaboratoryTests { get; set; }

        [JsonProperty("Ii12AcceptableForInternalMarket")]
        public ChedSection AcceptableForInternalMarket { get; set; }

        [JsonProperty("Ii20IdentificationOfBcp")]
        public ChedSection IdentificationOfBcp { get; set; }

        [JsonProperty("Ii21CertifyingOfficer")]
        public ChedSection CertifyingOfficer { get; set; }

        [JsonProperty("Ii22InspectionFees")]
        public ChedSection InspectionFees { get; set; }

        [JsonProperty("Ii23CustomsDocumentReference")]
        public ChedSection CustomsDocumentReference { get; set; }

        // Part III
        [JsonProperty("PartIiiFollowUp")]
        public ChedSection PartIiiFollowUp { get; set; }

        [JsonProperty("Iii1PreviousChed")]
        public ChedSection Iii1PreviousChed { get; set; }

        [JsonProperty("Iii2ChedReference")]
        public ChedSection Iii2ChedReference { get; set; }

        [JsonProperty("Iii24SubsequentChed")]
        public ChedSection Iii24SubsequentChed { get; set; }

        [JsonProperty("Iii4DetailsOnReDispatching")]
        public ChedSection DetailsOnReDispatching { get; set; }

        [JsonProperty("Iii5FollowUp")]
        public ChedSection FollowUp { get; set; }

        [JsonProperty("Iii6OfficialInspector")]
        public ChedSection OfficialInspector { get; set; }
    }
}