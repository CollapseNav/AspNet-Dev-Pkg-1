using System;
using System.IO;
using AspNet.Dev.Pkg.Infrastructure.Ext;
using AspNet.Dev.Pkg.Infrastructure.Util;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace AspNet.Dev.Pkg.Demo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType(typeof(DemoDbContext)).As(typeof(DbContext));
            builder.RegisterModule(new BaseAutofacModule());
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DemoDbContext>(options =>
            {
                options.UseSqlite(Configuration.GetConnectionString("Default"));
            });
            services.AddControllers(
                options =>
            {
                options.Filters.Add<ApiFilter>();
            }
            );
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AspNet.Dev.Pkg.Demo", Version = "v1" });
                c.DocInclusionPredicate((docName, description) => true);
                DirectoryInfo d = new(AppContext.BaseDirectory);
                FileInfo[] files = d.GetFiles("*.xml");
                foreach (var item in files)
                {
                    c.IncludeXmlComments(item.FullName, true);
                }
            });


            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                });
            });
            services.AddAutoMapper(typeof(MappingProfile));
            services.ConfigureCoreHttpContext();

            var authUrl = Configuration.GetSection("AuthUrl").Get<string>();
            if (!string.IsNullOrEmpty(authUrl))
            {
                services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = authUrl;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
                });
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCoreServiceGet();
            app.UseCors();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseExceptionHandler(action => action.Use(ErrorHandler.ExceptionHandler));
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AspNet.Dev.Pkg.Demo v1"));

            // app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();



            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
