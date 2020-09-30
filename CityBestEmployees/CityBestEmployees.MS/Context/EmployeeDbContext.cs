using CityBestEmployees.MS.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CityBestEmployees.MS.Context
{
    public class EmployeeDbContext : IdentityDbContext<Employee>
    {
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options)   {  }

        public DbSet<Department> Departments { get; set; }
    }
}
