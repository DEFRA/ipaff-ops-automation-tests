using Newtonsoft.Json;
using System.Collections.Generic;

namespace PdfExtraction.Models
{
    // A generic class to hold the fields of any section
    public class ChedSection
    {
        // Common fields found across many sections
        public string Value { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string IsoCode { get; set; }
        public string ApprovalNumber { get; set; }
        
        // Transport & Goods
        public string Mode { get; set; }
        public string InternationalTransportDocument { get; set; }
        public string Identification { get; set; }
        public string ProductType { get; set; }
        public string Species { get; set; }
        public string Commodity { get; set; }
        public string NetWeight { get; set; }
        public string PackageCount { get; set; }
        public string CountryOfOrigin { get; set; }
        
        // Dates & Signatures
        public string Date { get; set; }
        public string Time { get; set; }
        public string DateOfSignature { get; set; }
        public string NameOfSignatory { get; set; }
        public string Signature { get; set; }

        // Boolean/Checkbox flags (serialized as strings "true"/"false" in your JSON)
        public string Ambient { get; set; }
        public string Chilled { get; set; }
        public string Frozen { get; set; }
        public string HumanConsumption { get; set; }
        public string Conforming { get; set; }
        public string NonConforming { get; set; }
        public string Satisfactory { get; set; }
        public string NotSatisfactory { get; set; }
        public string Yes { get; set; }
        public string No { get; set; }
        public string EuStandard { get; set; }
        public string SatisfactoryFollowingOfficialIntervention { get; set; }
        public string NotDone { get; set; }
        public string SealCheckOnly { get; set; }
        public string FullIdentityCheck { get; set; }
        public string Random { get; set; }
        public string Suspicion { get; set; }
        public string IntensifiedControls { get; set; }
        public string Pending { get; set; }
        public string LocalCompetentAuthority { get; set; }
        public string SecondEntryPoint { get; set; }
        public string ArrivalOfConsignment { get; set; }
        public string ComplianceOfTheConsignment { get; set; }

        // Catch-all for any other dynamic fields
        [JsonExtensionData]
        public Dictionary<string, object> AdditionalData { get; set; }
    }
}