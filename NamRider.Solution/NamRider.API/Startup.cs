using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

[assembly: OwinStartup(typeof(NamRider.API.Startup))]

namespace NamRider.API
{
    public partial class Startup
    {
        //public void ConfigureServices(IServiceCollection services)
        //{

        //}
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }

        //public void Configure(IApplicationBuilder app)
        //{
        //    app.UseSwaggerGen();
        //    app.UseSwaggerUi();
        //}
    }
}
