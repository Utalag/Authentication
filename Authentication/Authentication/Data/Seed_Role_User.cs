using Authentication.Models.UsersModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;



namespace Authentication.Data
{
    public class Seed_Role_User
    {
        readonly UserManager<ApplicationUser> userManager;
        readonly RoleManager<IdentityRole> roleManager;
        readonly ILogger<Seed_Role_User> logger;

        public Seed_Role_User(UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager,ILogger<Seed_Role_User> logger)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.logger = logger;
        }

        public string Role_Admin { get; set; } = "Admin";
        public string Role_User { get; set; } = "User";

        private string admin_Email = "admin@admin.com";
        private string admin_Password = "Qwert123!";


        public async Task InitAsync()
        {
            if(await roleManager.Roles.AnyAsync() && await userManager.Users.AnyAsync())
            {
                logger.LogInformation("Roles and users already exist. Skipping initialization.");
                return;
            }

            await SeedRolesAsync();
            await SeedUsersAsync();
        }


        private async Task SeedUsersAsync()
        {
            await CheckUserOrCreateAsync(admin_Email);

        }
        private async Task CheckUserOrCreateAsync(string email)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(email);
                if(user == null)
                {
                    var newUser = new ApplicationUser
                    {
                        UserName = admin_Email,
                        Email = admin_Email,
                        EmailConfirmed = true
                    };
                    var result = await userManager.CreateAsync(newUser,admin_Password);
                    if(result.Succeeded)
                    {
                        logger.LogInformation($"User {admin_Email} created successfully.");
                    }
                    else
                    {
                        logger.LogError($"Failed to create user {admin_Email}: {string.Join(", ",result.Errors.Select(e => e.Description))}");
                    }
                }
            }
            catch(Exception ex)
            {
                logger.LogError(ex,$"An error occurred while checking or creating user {email}.");
            }
        }



        private async Task SeedRolesAsync()
        {
            await CheckRoleOrCreateAsync(Role_Admin,admin_Email);
            await CheckRoleOrCreateAsync(Role_User,string.Empty);
        }

        private async Task CheckRoleOrCreateAsync(string roleName,string email)
        {
            try
            {
                var roleExists = await roleManager.RoleExistsAsync(roleName);
                if(!roleExists)
                {
                    var result = await roleManager.CreateAsync(new IdentityRole(roleName));
                    if(result.Succeeded)
                    {
                        logger.LogInformation($"Role {roleName} created successfully.");
                        if(!string.IsNullOrEmpty(email))
                        {
                            var user = await userManager.FindByEmailAsync(email);
                            if(user != null)
                            {
                                var addToRoleResult = await userManager.AddToRoleAsync(user,roleName);
                                if(addToRoleResult.Succeeded)
                                {
                                    logger.LogInformation($"User {email} added to role {roleName} successfully.");
                                }
                                else
                                {
                                    logger.LogError($"Failed to add user {email} to role {roleName}: {string.Join(", ",addToRoleResult.Errors.Select(e => e.Description))}");
                                }
                            }
                        }
                    }
                    else
                    {
                        logger.LogError($"Failed to create role {roleName}: {string.Join(", ",result.Errors.Select(e => e.Description))}");
                    }
                }
            }
            catch(Exception ex)
            {
                logger.LogError(ex,$"An error occurred while checking or creating role {roleName}.");
            }
        }
    }
}
