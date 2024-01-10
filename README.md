Installed packets:
- "Microsoft.AspNetCore.Identity.EntityFrameworkCore" (6.0.23)
- "Microsoft.AspNetCore.Identity.UI" (6.0.23)
- "Microsoft.EntityFrameworkCore" (6.0.25)
- "Microsoft.EntityFrameworkCore.Sqlite" (6.0.25)
- "Microsoft.EntityFrameworkCore.SqlServer" (6.0.25)
- "Microsoft.EntityFrameworkCore.Tools" (6.0.25)
- "Microsoft.VisualStudio.Web.CodeGeneration.Design" (6.0.16)

Before Running project check appsettings.json and create database with the same name as in this file (probably "CarsWSEI")
After creating this database, write 2 commands in Console Packet Manager:
1) Add-Migration "Initial Create"
2) Update-Database
Roles and Admin User will add itself automatically (IdentityDataInitializer.cs)

Admin user data:
Email: "admin@admin.pl", Password: "AdminPassword123!"

Test user data:
Email: "test@test.pl", Passowrd: "Password123!"

User:
This Application is website on which Users can reserve cars. On first page User can choose car from the list and check it details or click Reserve and be moved to next page.
There are visible all upcoming reservations for next week. User can create reservation for himself after registering or logging in.
Validation of reservations check if they are not in past, if pickup date is not later than return date and there is also validation for overlapping reservations.
After User create reservation he is moved to page with his all reservations. He can also click button and see past reservations.

Admin:
Admin can add new cars and edit already existing ones. Also He can see all reservations with details and delete them. 
