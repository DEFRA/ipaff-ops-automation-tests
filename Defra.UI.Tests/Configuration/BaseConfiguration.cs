using Defra.UI.Tests.Configuration;

public class BaseConfiguration
{
    public TestConfiguration TestConfiguration { get; set; }
    public UiFrameworkConfiguration UiFrameworkConfiguration { get; set; }
    public BrowserStackConfiguration BrowserStackConfiguration { get; set; }
    public AzureConnectionConfig AzureConnectionConfig { get; set; }
    public SearchProtectedNotifications SearchProtectedNotifications { get; set; }
    public SearchCloneNotifications SearchCloneNotifications { get; set; }
    public IntensifiedOfficialControls IntensifiedOfficialControls { get; set; }
    public Dictionary<string, UserCredential> UserCredentials { get; set; }
}

public class UserCredential
{
    public string UserName { get; set; }
    public string Credential { get; set; }
    public string? BusinessName { get; set; }
    public string? AgentCode { get; set; }
}