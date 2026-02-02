using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Data.Users;
using Defra.UI.Tests.HelperMethods;
using Defra.UI.Tests.Pages.Classes;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Hooks
{
    [Binding]
    public class PageHooks
    {

        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        public PageHooks(IObjectContainer objectContainer, ScenarioContext senarioContext)
        {
            _objectContainer = objectContainer;
            _scenarioContext = senarioContext;
        }

        [BeforeScenario(Order = (int)HookRunOrder.Pages)]
        public void BeforeScenario()
        {
            BindAllPages();
        }

        private void BindAllPages()
        {
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<UserObject, IUserObject>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<UrlBuilder, IUrlBuilder>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<SignInPage, ISignInPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<HomePage, IHomePage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<EmailSignUpPage, IEmailSignUpPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<GovernmentGatewayTypePage, IGovernmentGatewayTypePage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<YourImportNotificationsPage, IYourImportNotificationsPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<AboutConsignmentPage, IAboutConsignmentPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<OriginOfProductPage, IOriginOfProductPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<OriginOfImportPage, IOriginOfImportPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<CommodityPage, ICommodityPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<ReasonForImportPage, IReasonForImportPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<RiskCategoryPage, IRiskCategoryPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<NotificationHubPage, INotificationHubPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<AdditionalDetailsPage, IAdditionalDetailsPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<AccompanyingDocumentsPage, IAccompanyingDocumentsPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<ApprovedEstablishmentPage, IApprovedEstablishmentPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<AddressesPage, IAddressesPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<SearchExistingConsignorPage, ISearchExistingConsignorPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<SearchExistingConsigneePage, ISearchExistingConsigneePage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<SearchExistingDestinationPage, ISearchExistingDestinationPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<PortOfEntryPage, IPortOfEntryPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<GoodsMovementServicesPage, IGoodsMovementServicesPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<ContactDetailsPage, IContactDetailsPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<NominatedContactsPage, INominatedContactsPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<ContactAddressPage, IContactAddressPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<BillingDetailsPage, IBillingDetailsPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<ReviewYourNotificationPage, IReviewYourNotificationPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<DeclarationPage, IDeclarationPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<ConfirmationPage, IConfirmationPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<SignOutPage, ISignOutPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<BTMSSearchPage, IBTMSSearchPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<BTMSSearchResultPage, IBTMSSearchResultPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<SummaryPage, ISummaryPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<InspectorImportNotificationsPage, IInspectorImportNotificationsPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<DecisionHubPage, IDecisionHubPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<LocalReferenceNumberPage, ILocalReferenceNumberPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<DocumentaryCheckPage, IDocumentaryCheckPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<IdentityAndPhysicalChecksPage, IIdentityAndPhysicalChecksPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<SealNumbersPage, ISealNumbersPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<LaboratoryTestsPage, ILaboratoryTestsPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<DecisionPage, IDecisionPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<ReviewOutcomeDecisionPage, IReviewOutcomeDecisionPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<ChecksSubmittedPage, IChecksSubmittedPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<AnimalIdentificationDetailsPage, IAnimalIdentificationDetailsPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<LatestHealthCertificatePage, ILatestHealthCertificatePage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<TransporterPage, ITransporterPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<TransportContactsPage, ITransportContactsPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<SearchExistingTranspoterPage, ISearchExistingTranspoterPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<CountyParishHoldingPage, ICountyParishHoldingPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<CountriesConsignmentTravelPage, ICountriesConsignmentTravelPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<ChecksPage, IChecksPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<SearchExistingControlledDestinationPage, ISearchExistingControlledDestinationPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<ControlledDestinationPage, IControlledDestinationPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<HealthCertificatePage, IHealthCertificatePage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<CatchCertificatesPage, ICatchCertificatesPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<SearchExistingImporterPage, ISearchExistingImporterPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<IUUPage, IIUUPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<ReasonForRefusalPage, IReasonForRefusalPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<ConsignmentsRequiringControlPage, IConsignmentsRequiringControlPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<RecordLaboratoryTestInformationPage, IRecordLaboratoryTestInformationPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<ReviewBorderNotificationPage, IReviewBorderNotificationPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<OverrideRiskDecisionPage, IOverrideRiskDecisionPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<CHEDOverviewPage, ICHEDOverviewPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<EnterBorderNotificationDetailsPage, IEnterBorderNotificationDetailsPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<BorderNotificationSubmittedPage, IBorderNotificationSubmittedPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<BorderNotificationsPage, IBorderNotificationsPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<BorderNotificationOverviewPage, IBorderNotificationOverviewPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<ReplaceCHEDPage, IReplaceCHEDPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<NotificationOverviewPage, INotificationOverviewPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<TransportAfterPortOfEntryPage, ITransportAfterPortOfEntryPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<AddressBookPage, IAddressBookPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<DocumentsPage, IDocumentsPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<ChooseAddressTypePage, IChooseAddressTypePage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<ChooseOperatorTypePage, IChooseOperatorTypePage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<AddOperatorDetailsPage, IAddOperatorDetailsPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<TheAddressHasBeenAddedPage, ITheAddressHasBeenAddedPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<SelectTheTransporterTypePage, ISelectTheTransporterTypePage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<ViewOperatorPage, IViewOperatorPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<DeleteAddressPage, IDeleteAddressPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<TheAddressHasBeenDeletedPage, ITheAddressHasBeenDeletedPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<UploadCatchCertificatesPage, IUploadCatchCertificatesPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<ManageCatchCertificatesPage, IManageCatchCertificatesPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<AddCatchCertificateDetailsPage, IAddCatchCertificateDetailsPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<RequestAmendmentPage, IRequestAmendmentPage>());

            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<CookiesPage, ICookiesPage>());
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<ContactPage, IContactPage>());

            // PDF Certificate Page
            _objectContainer.RegisterInstanceAs(GetBaseWithContainer<NotificationCertificatePage, INotificationCertificatePage>());

            //Read Email
            _objectContainer.RegisterInstanceAs(GetBaseWithScenarioContext<FetchCodeFromEmail, IFetchCodeFromEmail>());
        }


        private TU GetBaseWithContainer<T, TU>() where T : TU => (TU)Activator.CreateInstance(typeof(T), _objectContainer);
        private TU GetBaseWithContainerScenarioContext<T, TU>() where T : TU => (TU)Activator.CreateInstance(typeof(T), _objectContainer, _scenarioContext);
        private TU GetBaseWithScenarioContext<T, TU>() where T : TU => (TU)Activator.CreateInstance(typeof(T), _scenarioContext);
    }
}