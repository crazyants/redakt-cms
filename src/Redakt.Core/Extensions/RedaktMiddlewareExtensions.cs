using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Redakt.Core.Cache;
using Redakt.Core.Services;
using Redakt.Data.Repository;
using System;
using Microsoft.AspNetCore.Builder;
using Redakt.Data;

namespace Redakt.Core.Extensions
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public static class RedaktMiddlewareExtensions
    {
        public static IApplicationBuilder UseRedakt(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            app.ApplicationServices.GetService<IDbInit>().Run();

            return app;
        }
    }
}
