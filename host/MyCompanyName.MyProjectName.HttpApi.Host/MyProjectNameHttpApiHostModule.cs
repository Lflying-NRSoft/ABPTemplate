using MyCompanyName.MyProjectName.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Reflection;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Caching;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.Settings;

namespace MyCompanyName.MyProjectName
{
    [DependsOn(typeof(MyProjectNameHttpApiModule))]
    [DependsOn(typeof(MyProjectNameApplicationModule))]
    [DependsOn(typeof(MyProjectNameEntityFrameworkCoreModule))]
    [DependsOn(typeof(MyProjectNameDapperModule))]
    [DependsOn(typeof(AbpAspNetCoreSerilogModule))]
    [DependsOn(typeof(AbpAutofacModule))]
    [DependsOn(typeof(AbpSettingsModule))]
    [DependsOn(typeof(AbpCachingModule))]
    [DependsOn(typeof(AbpAspNetCoreMvcModule))]
    public class MyProjectNameHttpApiHostModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            var configuration = context.Services.GetConfiguration();

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseSqlServer();
            });

            // 配置Redis分布式缓存
            //Configure<AbpDistributedCacheOptions>(options =>
            //{
            //    options.KeyPrefix = "MyProjectName:";
            //});

            //context.Services.AddStackExchangeRedisCache(options =>
            //{
            //    options.Configuration = configuration["Redis:Configuration"];
            //});

            ConfigureConventionalControllers();
            ConfigureSwaggerServices(context, configuration);
        }

        /// <summary>
        /// 配置Swagger
        /// </summary>
        /// <param name="context"></param>
        private void ConfigureSwaggerServices(ServiceConfigurationContext context, IConfiguration configuration)
        {
            context.Services.AddSwaggerGen(
                options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "MyProjectName API", Version = "v1" });
                    options.DocInclusionPredicate((docName, description) => true);
                    options.CustomSchemaIds(type => type.FullName);

                    // 显示注释说明
                    var xmlHostFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlHostPath = Path.Combine(AppContext.BaseDirectory, xmlHostFile);
                    options.IncludeXmlComments(xmlHostPath);

                    // 显示注释说明
                    var xmlFile = $"{typeof(MyProjectNameApplicationModule).Assembly.GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    options.IncludeXmlComments(xmlPath);
                });
        }

        /// <summary>
        /// 配置自动生成应用服务层的控制器方法
        /// </summary>
        private void ConfigureConventionalControllers()
        {
            Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options.ConventionalControllers.Create(typeof(MyProjectNameApplicationModule).Assembly);
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();

            if (!context.GetEnvironment().IsDevelopment())
            {
                app.UseHsts();
            }

            app.UseCorrelationId();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            //app.UseAbpRequestLocalization();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Support APP API");
                options.RoutePrefix = string.Empty; // 在应用的根 (http://localhost:<port>/) 处提供 Swagger UI
            });
            //app.UseAuditing();
            app.UseConfiguredEndpoints();
        }
    }
}
