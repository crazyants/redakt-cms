﻿using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Redakt.Core.Cache;
using Redakt.Core.Services;
using Redakt.Data.Repository;
using System;
using Redakt.Data;

namespace Redakt.Core.Extensions
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public static class RedaktServiceCollectionExtensions
    {
        public static IServiceCollection AddRedakt(this IServiceCollection services)
        {
            return services.AddRedakt(null);
        }

        public static IServiceCollection AddRedakt(this IServiceCollection services, Action<RedaktOptions> setupAction)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            var options = new RedaktOptions();
            setupAction?.Invoke(options);

            services.AddSingleton<ICache, InMemoryCache>();

            services.AddSingleton<ISiteService, SiteService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<IPageContentService, PageContentService>();
            services.AddSingleton<IPageTypeService, PageTypeService>();
            services.AddSingleton<IFieldTypeService, FieldTypeService>();

            if (options.UseTestData)
            {
                services.AddTransient<IDbInit, Data.Test.DbInit>();
                services.AddScoped<ISiteRepository, Data.Test.Repository.SiteRepository>();
                services.AddScoped<IPageRepository, Data.Test.Repository.PageRepository>();
                services.AddScoped<IPageContentRepository, Data.Test.Repository.PageContentRepository>();
                services.AddScoped<IPageTypeRepository, Data.Test.Repository.PageTypeRepository>();
                services.AddScoped<IFieldTypeRepository, Data.Test.Repository.FieldTypeRepository>();
            }
            else
            {
                services.AddTransient<IDbInit, Data.Mongo.DbInit>();
                services.AddScoped<Data.Mongo.IConnection, Data.Mongo.MongoConnection>();
                services.AddScoped<ISiteRepository, Data.Mongo.Repository.SiteRepository>();
                services.AddScoped<IPageRepository, Data.Mongo.Repository.PageRepository>();
                services.AddScoped<IPageContentRepository, Data.Mongo.Repository.PageContentRepository>();
                services.AddScoped<IPageTypeRepository, Data.Mongo.Repository.PageTypeRepository>();
                services.AddScoped<IFieldTypeRepository, Data.Mongo.Repository.FieldTypeRepository>();
            }

            return services;
        }
    }
}
