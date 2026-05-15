using Defra.UI.Tests.Pages.Classes;
using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;


namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class AdditionalDetailsSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IAdditionalDetailsPage? additionalDetailsPage => _objectContainer.IsRegistered<IAdditionalDetailsPage>() ? _objectContainer.Resolve<IAdditionalDetailsPage>() : null;


        public AdditionalDetailsSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }


        [Then("the Additional details page should be displayed")]
        public void ThenTheAdditionalDetailsPageShouldBeDisplayed()
        {
            Assert.True(additionalDetailsPage?.IsPageLoaded(), "Description of the goods Additional details page not loaded");
        }

        [Then("the Additional animal details page should be displayed")]
        public void ThenTheAdditionalAnimalDetailsPageShouldBeDisplayed()
        {
            Assert.True(additionalDetailsPage?.IsAdditionalAnimalDetailsPageLoaded(), "Description of the goods Additional animal details page not loaded");
        }

        [When("the user selects {string} radio button on the Additional details page")]
        public void WhenTheUserSelectsAnyRadioButtonOnTheAdditionalDetailsPage(string option)
        {
            additionalDetailsPage?.ClickImportingProduct(option);
            _scenarioContext["Temperature"]=option;
        }

        [When("the user selects {string} radio button under Commodity intended for on the Additional details page")]
        public void WhenTheUserSelectsRadioButtonUnderCommodityIntendedForOnTheAdditionalDetailsPage(string commIntendedForOption)
        {
            Assert.True(additionalDetailsPage?.SelectCommodityIntendedForRadio(commIntendedForOption), "Commodity intended for radio is not selected on the Additional details page");
            _scenarioContext["CommodityIntendedFor"] = commIntendedForOption;
        }

        [When("the user changes What are the animals certified for? to {string}")]
        [When("the user selects {string} for What are the animals certified for?")]
        public void WhenTheUserSelectsForWhatAreTheAnimalsCertifiedFor(string certificationOption)
        {
            additionalDetailsPage?.SelectAnimalCertification(certificationOption);
            _scenarioContext["CertificationOption"] = certificationOption;
        }

        [When("the user selects {string} for Does the consignment contain any unweaned animals?")]
        public void WhenTheUserSelectsForDoesTheConsignmentContainAnyUnweanedAnimals(string unweanedAnimalsOption)
        {
            additionalDetailsPage?.SelectUnweanedAnimalsOption(unweanedAnimalsOption);
            _scenarioContext["UnweanedAnimalsOption"] = unweanedAnimalsOption;
        }

        [When("the user keeps the {string} option selected for Does the consignment contain any unweaned animals?")]
        public void WhenTheUserKeepsTheOptionSelectedForDoesTheConsignmentContainAnyUnweanedAnimals(string unweanedAnimalsOption)
        {
            Assert.True(additionalDetailsPage?.IsUnweanedAnimalsRadioSelected(unweanedAnimalsOption), $"Unweaned animals radio is not pre-selected with {unweanedAnimalsOption} option");
            _scenarioContext["UnweanedAnimalsOption"] = unweanedAnimalsOption;
        }

        [When(@"the user clicks on Save and review")]
        public void WhenIClickSaveAndReview()
        {
            additionalDetailsPage?.ClickSaveAndReview();
        }

        [Then("the Commodity intended for field displays the radio options {string} {string} {string} and {string}")]
        public void ThenTheCommodityIntendedForFieldDisplaysTheRadioOptionsAnd(string radioOne, string radioTwo, string radioThree, string radioFour)
        {
            List<string> commOptionsListExpected = new List<string>() { radioOne, radioTwo, radioThree, radioFour };
            Assert.True(additionalDetailsPage?.AreAllCommIntendedForRadioOptionsDisplayed(commOptionsListExpected), "Not all Commodity intended for radio options are displayed on the Additional details page");
        }

        [Then("the user verifies and enters any missing data on the Additional details page")]
        public void ThenTheUserVerifiesAndEntersAnyMissingDataOnTheAdditionalDetailsPage()
         {
            var commIntendedForRadio = additionalDetailsPage?.GetCommIntendedForRadioLabelText;
            if (!string.IsNullOrEmpty(commIntendedForRadio))
                _scenarioContext["CommodityIntendedFor"] = commIntendedForRadio;
            else
                WhenTheUserSelectsRadioButtonUnderCommodityIntendedForOnTheAdditionalDetailsPage("Human consumption");

            var temperatureRadio = additionalDetailsPage?.GetTemperatureRadioLabelText;
            if (!string.IsNullOrEmpty(temperatureRadio))
                _scenarioContext["Temperature"] = temperatureRadio;
            else
                WhenTheUserSelectsAnyRadioButtonOnTheAdditionalDetailsPage("Chilled");
        }

        [Then("the additional details should be validated with the values given in the input")]
        public void ThenTheAdditionalDetailsShouldBeValidatedWithTheValuesGivenInTheInput()
        {
            const string expectedNetWeight = "1500";
            const string expectedNumberOfPackages = "500";

            var allDataMatches = true;
            var mismatches = new List<string>();

            // Validate Total gross weight against scenario context
            if (_scenarioContext.ContainsKey("TotalGrossWeight"))
            {
                var expectedGrossWeight = _scenarioContext.Get<string>("TotalGrossWeight");
                var actualGrossWeight = additionalDetailsPage?.GetGrossWeightValue();
                if (!string.Equals(expectedGrossWeight, actualGrossWeight, StringComparison.OrdinalIgnoreCase))
                {
                    allDataMatches = false;
                    mismatches.Add($"TotalGrossWeight: Expected '{expectedGrossWeight}', Found '{actualGrossWeight}'");
                }
                else
                {
                    Console.WriteLine($"[ADDITIONAL DETAILS VALIDATION] ✓ TotalGrossWeight: '{expectedGrossWeight}' matches");
                }
            }

            // Validate Net weight is fixed at 1500
            var actualNetWeight = additionalDetailsPage?.GetNetWeight();
            if (!string.Equals(expectedNetWeight, actualNetWeight, StringComparison.OrdinalIgnoreCase))
            {
                allDataMatches = false;
                mismatches.Add($"NetWeight: Expected '{expectedNetWeight}', Found '{actualNetWeight}'");
            }
            else
            {
                Console.WriteLine($"[ADDITIONAL DETAILS VALIDATION] ✓ NetWeight: '{actualNetWeight}' matches");
            }

            // Validate Number of packages is fixed at 500
            var actualNumberOfPackages = additionalDetailsPage?.GetNumberOfPackages();
            if (!string.Equals(expectedNumberOfPackages, actualNumberOfPackages, StringComparison.OrdinalIgnoreCase))
            {
                allDataMatches = false;
                mismatches.Add($"NumberOfPackages: Expected '{expectedNumberOfPackages}', Found '{actualNumberOfPackages}'");
            }
            else
            {
                Console.WriteLine($"[ADDITIONAL DETAILS VALIDATION] ✓ NumberOfPackages: '{actualNumberOfPackages}' matches");
            }

            // Validate Total gross volume is empty
            var actualGrossVolume = additionalDetailsPage?.GetGrossVolumeValue();
            if (!string.IsNullOrEmpty(actualGrossVolume))
            {
                allDataMatches = false;
                mismatches.Add($"TotalGrossVolume: Expected empty but found '{actualGrossVolume}'");
            }
            else
            {
                Console.WriteLine($"[ADDITIONAL DETAILS VALIDATION] ✓ TotalGrossVolume is empty as expected");
            }

            Assert.True(allDataMatches, $"Additional details validation failed. Mismatches: {string.Join(", ", mismatches)}");
        }

        /// <summary>
        /// Randomly selects a certification option from a specification string.
        /// 
        /// Format examples:
        ///   "Any"                                        — any option visible on the page
        ///   "Approved bodies, Slaughter, Other"           — pick from whichever of these exist on the page
        ///   "Breeding and/or production, Circus/exhibition" — pick from whichever of these exist on the page
        /// </summary>
        [When("the user randomly selects a certification from {string}")]
        public void WhenTheUserRandomlySelectsACertificationFrom(string certificationSpec)
        {
            List<string>? constrainedOptions = null;

            if (!certificationSpec.Equals("Any", StringComparison.OrdinalIgnoreCase))
            {
                constrainedOptions = certificationSpec.Split(',').Select(s => s.Trim()).ToList();
            }

            additionalDetailsPage!.SelectRandomCertification(constrainedOptions);
        }
    }
}