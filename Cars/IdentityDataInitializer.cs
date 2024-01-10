using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

public static class IdentityDataInitializer
{
    public static async Task SeedData(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        await SeedRoles(roleManager);
        await SeedAdminUser(userManager);
        await SeedTestUser(userManager);
    }

    private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }
        if (!await roleManager.RoleExistsAsync("User"))
        {
            await roleManager.CreateAsync(new IdentityRole("User"));
        }
    }

    private static async Task SeedAdminUser(UserManager<ApplicationUser> userManager)
    {
        var adminEmail = "admin@admin.pl";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new ApplicationUser { UserName = adminEmail, Email = adminEmail, FirstName = "Admin", LastName = "Admin", EmailConfirmed = true};
            await userManager.CreateAsync(adminUser, "AdminPassword123!");
        }

        if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }

    private static async Task SeedTestUser(UserManager<ApplicationUser> userManager)
    {
        var userEmail = "test@test.pl";
        var testUser = await userManager.FindByEmailAsync(userEmail);
        if (testUser == null)
        {
            testUser = new ApplicationUser { UserName = userEmail, Email = userEmail, FirstName = "Jhon", LastName = "Test", EmailConfirmed = true };
            await userManager.CreateAsync(testUser, "Password123!");
        }

        if (!await userManager.IsInRoleAsync(testUser, "User"))
        {
            await userManager.AddToRoleAsync(testUser, "User");
        }
    }
}
