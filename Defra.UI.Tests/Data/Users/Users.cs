using Reqnroll.BoDi;
using Defra.UI.Tests.Configuration;

namespace Defra.UI.Tests.Data.Users
{
    public class User
    {
        public string UserName { get; set; }
        public string Credential { get; set; }
        public string LoginInfo { get; set; }
        public string Environment { get; set; }
        public bool HomePage { get; set; }
        public string Role { get; set; }
        public string? BusinessName { get; set; }
        public string? AgentCode { get; set; }
    }

    public interface IUserObject
    {
        User GetUser(string application, string role);
    }

    internal class UserObject : IUserObject
    {
        private readonly IObjectContainer _objectContainer;

        public UserObject(IObjectContainer objectContainer) => _objectContainer = objectContainer;
        private readonly object _lock = new object();

        public User GetUser(string application, string role)
        {
            lock (_lock)
            {
                var credentials = ConfigSetup.BaseConfiguration.UserCredentials;
                if (credentials != null && credentials.TryGetValue(role, out var userCredential)
                    && !string.IsNullOrWhiteSpace(userCredential.UserName))
                {
                    return new User
                    {
                        UserName = userCredential.UserName,
                        Credential = userCredential.Credential,
                        BusinessName = userCredential.BusinessName,
                        AgentCode = userCredential.AgentCode,
                        Role = role
                    };
                }

                throw new InvalidOperationException(
                    $"No credentials found for role '{role}'. Ensure UserCredentials are configured in appsettings.json.");
            }
        }
    }
}