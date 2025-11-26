namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IEmailSignUpPage
    {
        public bool IsPageLoaded();
        void EnterEmailAddress(string emailAddress);
        public void ClickContinueButton();
        public void EnterConfirmationCode(string confirmationCode);
        public void EnterFullName(string Name);
        public void EnterThePassword(string password);
        public string GetGGID();
        public void SelectIndividualUser();
        public void EnterFirstAndLastName(string firstName, string lastName);
        public void EnterTelephoneNumber(string telephoneNumber);
        public void EnterPostCode(string postCode);
        public void SelectAddress();
        public void EnterMemorableWordAndHint(string memorableWord, string hint);
    }
}
