using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SHKang.Core.Helpers;
using SHKang.Model.Context;
using SHKang.Model.ViewModels.Admin;
using SHKang.Repository.Interface;
using SHKang.Repository.Repository;
using System.Text;

namespace SHKang.API
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            #region Cross origin
            services.AddCors(); 
            #endregion

            #region JWT Token

            var appSettingsSection = Configuration.GetSection("app_settings");
            services.Configure<AppSettings>(appSettingsSection);
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["app_settings:Issuer"],
                    ValidAudience = Configuration["app_settings:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

            #endregion

            #region Connection String

            services.AddDbContext<AppDbContext>(opts => opts.UseSqlServer(Configuration["ConnectionString:AppDbContext"]));

            services.Configure<ConnectionString>(Configuration.GetSection("ConnectionString"));
            services.AddOptions();
            #endregion
                       
            services.AddScoped<ILogin, LoginRepository>();
            services.AddScoped<IForgotPassword, ForgotPasswordRepository>();
            services.AddScoped<IOrder, OrderRepository>();
            services.AddScoped<IDashboard, DashboardRepository>();
            services.AddScoped<IUser, UserRepository>();
            services.AddScoped<IProductCategory, ProductCategoryRepository>();
            services.AddScoped<IProductCategoryType, ProductCategoryTypeRepository>();
            services.AddScoped<IOrderInvoice, OrderInvoiceRepository>();
            services.AddScoped<IProduct, ProductRepository>();
            services.AddScoped<IMaster, MasterRepository>();
            services.AddScoped<IContent, ContentRepository>();
            services.AddScoped<ICart, CartRepository>();
            services.AddScoped<IStyleCampaign, StyleCampaignRepository>();
            services.AddScoped<ISizeRatio, SizeRatioRepository>();
            services.AddScoped<ISize, SizeRepository>();
            services.AddScoped<IPromoCode, PromoCodeRepository>();
            services.AddScoped<IColor, ColorRepository>();
            services.AddScoped<ITestimonial, TestimonialRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            #region Cross Origin

            app.UseCors(x => x
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed(origin => true) // allow any origin
                    .AllowCredentials()); // allow credentials 

            #endregion

            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                  name: "areas",
                  template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
            });
        }
    }
}
