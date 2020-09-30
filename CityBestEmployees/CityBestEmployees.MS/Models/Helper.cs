using CityBestEmployees.MS.Context;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace CityBestEmployees.MS.Models
{
    public static class Helper
    {
        public static async Task Seed(EmployeeDbContext ctx, RoleManager<IdentityRole> roleManager, UserManager<Employee> userManager)
        {
            ctx.Database.EnsureCreated();

            if (!roleManager.Roles.Any())
            {
                var listOfRoles = new List<IdentityRole>
                {
                    new IdentityRole("Admin"),
                    new IdentityRole("Customer")
                };

                var listOfDepartment = new List<Department>
                {
                    new Department{Name = "Admin"},
                    new Department{Name = "Account"},
                    new Department{Name = "Logistics"}
                };



                foreach (var role in listOfRoles)
                {
                    await roleManager.CreateAsync(role);
                }
            }

            if (!ctx.Departments.Any())
            {

                var listOfDepartment = new List<Department>
                {
                    new Department{Name = "Admin"},
                    new Department{Name = "Account"},
                    new Department{Name = "Logistics"}
                };



                foreach (var department in listOfDepartment)
                {
                    ctx.Departments.Add(department);
                    ctx.SaveChanges();
                }
            }


            if (!userManager.Users.Any())
            {
                    var listOfUsers = new List<Employee>
                {
                    new Employee{ UserName="kaosi@me.com", Email = "kaosi@me.com", LastName="Nwizu", FirstName="Kaosilisochukwu", Photo = "~/images/avarta.jpg" },
                    new Employee{ UserName="chidi@me.com", Email = "chidi@me.com", LastName="Okobia", FirstName="Chidi", Photo = "~/images/chidi.jpg" }
                };



                int counter = 0;
                foreach (var user in listOfUsers)
                {
                    if(counter == 0)
                        user.Department = ctx.Departments.FirstOrDefault(x => x.Name == "Admin");
                    if(counter == 1)
                        user.Department = ctx.Departments.FirstOrDefault(x => x.Name == "Logistics");
                    var result = await userManager.CreateAsync(user, "P@$$word1");

                    if (result.Succeeded)
                    {
                        if (counter == 0)
                        {
                            await userManager.AddToRoleAsync(user, "Admin");
                        }
                        else
                        {
                            await userManager.AddToRoleAsync(user, "Customer");
                        }
                    }
                    counter++;
                }
            }
        }
    }
}
