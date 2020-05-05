namespace askLNU
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using askLNU.BLL.Configs;
    using askLNU.BLL.Infrastructure;
    using askLNU.BLL.Interfaces;
    using askLNU.BLL.Services;
    using askLNU.DAL.EF;
    using askLNU.DAL.Interfaces;
    using askLNU.DAL.Repositories;
    using AutoMapper;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.HttpsPolicy;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Owin;
    using Owin;
    using Microsoft.AspNetCore.SignalR;
    using askLNU.Hubs;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AdminConfig>(this.Configuration.GetSection("AdminConfig"));
            services.Configure<AuthMessageSenderOptions>(this.Configuration);
            services.Configure<CloudinaryConfig>(this.Configuration.GetSection("CloudinaryConfig"));

            services.AddDALDependencies(this.Configuration.GetConnectionString("DefaultConnection"));

            services.AddAuthentication().AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = this.Configuration["Authentication:Facebook:AppId"];
                facebookOptions.AppSecret = this.Configuration["Authentication:Facebook:AppSecret"];
            });

            services.AddTransient<IUnitOfWork, EFUnitOfWork>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ISignInService, SignInService>();
            services.AddTransient<IQuestionService, QuestionService>();
            services.AddTransient<IAnswerService, AnswerService>();

            services.AddTransient<ITagService, TagService>();
            services.AddTransient<IFacultyService, FacultyService>();
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IImageService, ImageService>();

            services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true;
            });
            services.AddSignalR();
            services.AddAutoMapper();
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.ConfigureApplicationCookie(options => options.LoginPath = "/Login");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHub<ChatHub>("/chatHub");
            });

            serviceProvider.CreateUserRoles().Wait();
        }
    }
}
