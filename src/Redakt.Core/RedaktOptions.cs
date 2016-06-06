using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Redakt.Core.Cache;
using Redakt.Core.Services;

namespace Redakt.Core.Extensions
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class RedaktOptions
    {
        public bool UseTestData { get; set; }
    }
}
