using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IViewOperatorPage
    {
        bool IsPageLoaded(string operatorName);
        void ClickDelete();
    }
}
