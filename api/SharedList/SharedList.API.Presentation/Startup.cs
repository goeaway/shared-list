using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Serilog;
using SharedList.API.Application.Exceptions;
using SharedList.API.Application.Pipeline;
using SharedList.Persistence;
using SharedList.Persistence.Models.Entities;

namespace SharedList.API.Presentation
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddDbContext<SharedListContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                    x => x.MigrationsAssembly("SharedList.Migrations")));

            //services.AddIdentity<IdentityUser, IdentityRole>();
            //services.AddAuthentication(options =>
            //    {
            //        options.DefaultAuthenticateScheme = GoogleDefaults.AuthenticationScheme;
            //        options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            //    })
            //    .AddGoogle(options =>
            //    {
            //        options.CallbackPath = "/auth-redirect";
            //        options.ClientId = "787759781218-74b6fgtbbddggjp9hu68ciqijp61m3h7.apps.googleusercontent.com";
            //        options.ClientSecret = "2nLpqiAXZyMJQsi9moIQSJ9R";
            //    });
            //// https://docs.microsoft.com/en-us/aspnet/core/security/authentication/social/google-logins?view=aspnetcore-3.1

            services.AddNowProvider();
            services.AddRandomWordsProvider();
            services.AddLogger();
            services.AddAutoMapper(typeof(List));
            services.AddMediatR(Assembly.GetAssembly(typeof(ValidationBehaviour<,>)));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseExceptionHandler(ExceptionHandler);
            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseAuthentication();
        }

        private void ExceptionHandler(IApplicationBuilder app)
        {
            app.Run(async ctx =>
            {
                ctx.Response.StatusCode = 500;
                ctx.Response.ContentType = "application/json";
                var exHandlerPathFeature = ctx.Features.Get<IExceptionHandlerFeature>();
                var exception = exHandlerPathFeature.Error;
                var uri = ctx.Request.Path;

                var logger = app.ApplicationServices.GetService<ILogger>();
                logger.Error(exception, "Error occurred when processing request {uri}", uri);

                var errorList = new List<string> { exception.Message };

                if (exception.InnerException != null)
                {
                    errorList.Add(exception.InnerException.Message);
                }

                if (exception is RequestValidationFailedException)
                {
                    ctx.Response.StatusCode = 400;
                    errorList.AddRange((exception as RequestValidationFailedException).Failures.Select(f => f.ErrorMessage));
                }

                await ctx.Response.WriteAsync(JsonConvert.SerializeObject(errorList));
            });
        }
    }
}
