using dotnetCoreWebSample.Impl;
using dotnetCoreWebSample.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace dotnetCoreWebSample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        //跨域策略名称,随意设置
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //配置跨域
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    //配置允许的域名和ip,例如 http://example.com ,配置*为全部允许
                    builder.WithOrigins(Configuration["AllowedHosts"].Split(",", StringSplitOptions.RemoveEmptyEntries).ToArray());
                });
            });

            //注入MVC
            services.AddMvcCore(options =>
            {
                options.EnableEndpointRouting = false;
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
            .AddApiExplorer();  //swagger用到,开启接口浏览
            
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Sample API", Version = "v1" });
                // Set the comments path for the Swagger JSON and UI. 注释
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            //配置生成的url为小写
            services.AddRouting(option => option
                .LowercaseUrls = true
            );

            //依赖注入配置
            services.AddScoped<IMyDependency, MyDependency>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //开发模式下开启错误页面
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //路由
            app.UseRouting();

            
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sample API V1");
                //配置该行直接在应用的根目录显示wagger
                c.RoutePrefix = string.Empty;
            });

            //允许跨域访问,需要在UseMvc之前配置
            app.UseCors(MyAllowSpecificOrigins);


            app.UseMvc();
        }
    }
}
