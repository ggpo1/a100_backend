using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A100_AspNetCore.API.Authentication.Options;
using A100_AspNetCore.Models.A100_Models.DataBase;
using A100_AspNetCore.Models.API;
using A100_AspNetCore.Models.Authentication;
using A100_AspNetCore.Services.API;
using A100_AspNetCore.Services.API.BackupService;
using A100_AspNetCore.Services.API.CityService;
using A100_AspNetCore.Services.API.EmployeesService;
using A100_AspNetCore.Services.API.PartialService;
using A100_AspNetCore.Services.API.Projects;
using A100_AspNetCore.Services.API.RefreshTokenService;
using A100_AspNetCore.Services.API.SchemeService;
using A100_AspNetCore.Services.API.SpecificationsService;
using A100_AspNetCore.Services.API.VikService;
using A100_AspNetCore.Services.Globalsat.GlobalsatService;
using A100_AspNetCore.Services.MapEngineAPI.MapService;
using A100_AspNetCore.Services.MapEngineAPI.ProjectService;
using A100_AspNetCore.Services.MapEngineAPI.WarehouseService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace A100_AspNetCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // Подключаем контекст базы данных пользователей
            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("aspUsers")));

            // Подключаем базу данных моделей контекста А100
            // Подключаем контекст базы данных пользователей
            services.AddDbContext<ASTIContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ASTI_db")));


            // Конфигурация куки файлов
            //services.ConfigureApplicationCookie(opts => {
            //    opts.Cookie.Name = "My.Cookie.User";
            //});

            // Валидация защиты (Сброс куки тут)
            services.Configure<SecurityStampValidatorOptions>(options =>
            {
                options.ValidationInterval = TimeSpan.FromSeconds(1);
            });

            // JWT токен для авторизации


            services
                .AddAuthentication(x =>
            {                
                x.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;                
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // укзывает, будет ли валидироваться издатель при валидации токена
                    ValidateIssuer = true,
                    // строка, представляющая издателя
                    ValidIssuer = AuthOptions.ISSUER,

                    // будет ли валидироваться потребитель токена
                    ValidateAudience = true,
                    // установка потребителя токена
                    ValidAudience = AuthOptions.AUDIENCE,
                    // будет ли валидироваться время существования
                    ValidateLifetime = true,
                    LifetimeValidator = AuthOptions.CustomLifetimeValidator, // Валидация времени

                    // установка ключа безопасности
                    IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                    // валидация ключа безопасности
                    ValidateIssuerSigningKey = true,
                };
            });
            // Конец JWT

            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    builder.WithOrigins(
                        "http://localhost:8080",
                        "http://192.168.50.8:10101",
                        "http://192.168.50.8",
                        "http://localhost:8081",
                        "http://192.168.50.8:11111",
                        "http://localhost:3000",
                        "https://localhost:3000",
                        "https://a100mapengine.herokuapp.com"
                        );
                    builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
                });
            });

            // Подключаем идентификацию пользователей по базе данных
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationContext>();

            services.AddSingleton<CounterMiddleWare>();
            services.AddSingleton<TokensService>();
            // Регистрируем сервисы (AddScoped - выделяет память, в случае обращения к сервису, на всю транзакцию)                      
            services.AddScoped<IUserService, UserService>(); // Сервис авторизации
            services.AddScoped<ICityService, CityService>(); // Сервис городов
            services.AddScoped<IProjectsService, ProjectsService>(); // Сервис проектов
            services.AddScoped<IEmployeeService, EmployeeService>(); // Сервис клиентов
            services.AddScoped<ISchemeService, SchemeService>(); // Сервис схемы (карта)
            services.AddScoped<IVikService, VikService>(); // VIK сервис (повреждения
            services.AddScoped<ISpecificationsService, SpecificationsService>(); // Сервис спецификаций
            services.AddScoped<IBackupService, BackupService>(); // Сервис бекапов для android приложения
            services.AddScoped<IPartialService, PartialService>(); // Сервис проведения ЧТО

            // сервисы для a100 MapEngine
            services.AddScoped<IMapService, MapService>(); // сервис для работы с картой
            services.AddScoped<IProjectService, ProjectService>(); // сервис для работы с проектами
            services.AddScoped<IWarehouseService, WarehouseService>(); // сервис для работы с объектами
            
            // globalsat integration
            services.AddScoped<IGlobalsatService, GlobalsatService>();


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors(MyAllowSpecificOrigins); 
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            

            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "default", template: "{controller=Home}/{action=Get}");
            });
        }
    }

}
