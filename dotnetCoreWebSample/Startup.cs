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

        //�����������,��������
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //���ÿ���
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    //���������������ip,���� http://example.com ,����*Ϊȫ������
                    builder.WithOrigins(Configuration["AllowedHosts"].Split(",", StringSplitOptions.RemoveEmptyEntries).ToArray());
                });
            });

            //ע��MVC
            services.AddMvcCore(options =>
            {
                options.EnableEndpointRouting = false;
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
            .AddApiExplorer();  //swagger�õ�,�����ӿ����
            
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Sample API", Version = "v1" });
                // Set the comments path for the Swagger JSON and UI. ע��
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            //�������ɵ�urlΪСд
            services.AddRouting(option => option
                .LowercaseUrls = true
            );

            //����ע������
            services.AddScoped<IMyDependency, MyDependency>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //����ģʽ�¿�������ҳ��
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //·��
            app.UseRouting();

            
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sample API V1");
                //���ø���ֱ����Ӧ�õĸ�Ŀ¼��ʾwagger
                c.RoutePrefix = string.Empty;
            });

            //����������,��Ҫ��UseMvc֮ǰ����
            app.UseCors(MyAllowSpecificOrigins);


            app.UseMvc();
        }
    }
}
