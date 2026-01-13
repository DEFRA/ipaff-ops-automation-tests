using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Defra.UI.Tests.Pages.Interfaces;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class AddressBookSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IAddressBookPage? addressBookPage => _objectContainer.IsRegistered<IAddressBookPage>() ? _objectContainer.Resolve<IAddressBookPage>() : null;

        public AddressBookSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }
    }
}
