using Authentication.Client.Pages;
using Authentication.Components;
using Authentication.Components.Account;
using Authentication.Data;
using Authentication.Models.UsersModels;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Authentication
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents()
                .AddInteractiveWebAssemblyComponents();

            
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
               //optionsAction.UseLazyLoadingProxies();


            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            
            
            
            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddScoped<IdentityUserAccessor>();
            builder.Services.AddScoped<IdentityRedirectManager>();
            builder.Services.AddScoped<AuthenticationStateProvider,PersistingRevalidatingAuthenticationStateProvider>();



            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            });
            //AddIdentityCookies() => !! Not funkcional with AddIdentityCore() !! Separate AddIdentity() and AddIdentityCookies() methods setsup..


            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.Cookie.Name = "IdentityServer.Cookie";
                options.Cookie.HttpOnly = true;
            });



            builder.Services.AddIdentity<ApplicationUser,IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

            builder.Services.AddAuthorization(options =>
            {
                //Example polycy :
                // Policy with specific claim type and value
                options.AddPolicy("RequireAdministratorRole",policy => policy.RequireClaim(ClaimTypes.Role,"Administrator"));

                // Example policy :
                // Policy with specific claims
                options.AddPolicy("AdminOnly",policy => policy.RequireClaim("Role","Admin"));
                options.AddPolicy("UserOnly",policy => policy.RequireClaim("Role","User"));

                // Example policy :
                // Policy with multiple claims
                options.AddPolicy("AdminWithEmail",policy =>
                {
                    policy.RequireClaim("Role","Admin");
                    policy.RequireClaim("Email","admin@example.com");
                });

                // Example policy :
                // Policy with roles
                options.AddPolicy("AdminOnlyRole",policy => policy.RequireRole("Admin"));
                options.AddPolicy("UserOnlyRole",policy => policy.RequireRole("User"));
            });
                
                




            builder.Services.AddSingleton<IEmailSender<ApplicationUser>,IdentityNoOpEmailSender>();


            // Register Seed_Role_User as a service
            builder.Services.AddScoped<Seed_Role_User>();





            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if(app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode()
                .AddInteractiveWebAssemblyRenderMode()
                .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);

            // Add additional endpoints required by the Identity /Account Razor components.
            app.MapAdditionalIdentityEndpoints();


            using(var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var seedRoleUser = services.GetRequiredService<Seed_Role_User>();
                await seedRoleUser.InitAsync();
            }
            


            app.Run();
        }
    }
}


// --------------- Old Methods ----------------





//// Seed the database.
//using(var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
//{
//    // Update the database.
//    var services = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//    services.Database.Migrate();


//    //rozsah.poskytovatelsluûeb.ZÌsk·PoûadovanouSluûbu<Seed>
//    var seed = scope.ServiceProvider.GetRequiredService<Seed_Role_User>();
//    await seed.InitAsync();
//}