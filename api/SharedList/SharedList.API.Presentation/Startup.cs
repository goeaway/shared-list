using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Serilog;
using SharedList.API.Application.Exceptions;
using SharedList.API.Application.Pipeline;
using SharedList.API.Application.Queries.GetList;
using SharedList.API.Presentation.Hubs;
using SharedList.Persistence;
using SharedList.Persistence.Models.Entities;

namespace SharedList.API.Presentation
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            //Configuration = AWSConfigurationHelper.CreateConfiguration(configuration);
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private const string AllowSpecificOriginsCORSPolicy = "allowSpecificOrigins";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(AllowSpecificOriginsCORSPolicy, builder =>
                    {
                        builder.WithOrigins(Configuration["URLs:FrontEnd"]);
                        builder.AllowAnyMethod();
                        builder.AllowAnyHeader();
                        builder.AllowCredentials();
                    });
            });

            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddFluentValidation(fv =>
                {
                    fv.RegisterValidatorsFromAssemblyContaining<GetListValidator>();
                });

            services.AddDbContext<SharedListContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                    x => x.MigrationsAssembly("SharedList.Migrations")));

            services.AddNowProvider();
            services.AddRandomWordsProvider();
            services.AddAutoMapper(typeof(List));
            services.AddMediatR(Assembly.GetAssembly(typeof(ValidationBehaviour<,>)));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddSignalR();
            services.AddConfiguration(Configuration);

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["Jwt:Key"])),
                        ValidateIssuerSigningKey = true,
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/listHub"))
                            {
                                context.Token = accessToken;
                            }

                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddFileLogger();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if(env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            } 
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseExceptionHandler(ExceptionHandler);
            app.UseCors(AllowSpecificOriginsCORSPolicy);
            
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ListHub>("/listhub");
            });
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
                else if (exception is RequestFailedException)
                {
                    var requestFailedException = exception as RequestFailedException;
                    ctx.Response.StatusCode = (int)requestFailedException.Code;
                }

                await ctx.Response.WriteAsync(JsonConvert.SerializeObject(errorList));
            });
        }
    }
}
