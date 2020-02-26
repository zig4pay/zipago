using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ZREL.ZiPago.Aplicacion.Web.Models.Settings;

namespace ZREL.ZiPago.Aplicacion.Web
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.Strict;
            });

            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddMvc(options => {
                options.EnableEndpointRouting = false;
            });            
            services.Configure<WebSiteSettingsModel>(Configuration.GetSection("ZRELZiPagoWebApi"));
            services.Configure<WebSiteSettingsModel>(Configuration.GetSection("ZRELZiPagoWebSite"));
            services.Configure<WebSiteSettingsModel>(Configuration.GetSection("ZRELZiPagoDatos"));
            services.Configure<WebSiteSettingsModel>(Configuration.GetSection("GoogleReCaptcha"));
            services.AddCors();

            services.AddDistributedMemoryCache();

            services.AddAuthentication(options =>
            {
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;                
            }).AddCookie(options => {
                options.LoginPath = "/Seguridad";
                });
            services.AddMvc().AddRazorPagesOptions(options =>
            {
                options.Conventions.AuthorizeFolder("/");
                options.Conventions.AllowAnonymousToPage("/Seguridad");                
            });
            
            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }

            //app.UseRouting();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            //app.UseSession();
            app.UseAuthorization();
            app.UseAuthentication();
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapRazorPages();
            //    endpoints.MapControllers();
            //    endpoints.MapControllerRoute(
            //        name: "default",
            //        pattern: "{controller=Seguridad}/{action=UsuarioAutenticar}/{id?}");
            //});

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Seguridad}/{action=UsuarioAutenticar}");
            });
        }
    }
}
