using CityBestEmployees.MS.Context;
using CityBestEmployees.MS.Models;
using System.Collections.Generic;
using System.Linq;

namespace CityBestEmployees.MS.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly EmployeeDbContext _context;

        public DepartmentRepository(EmployeeDbContext context)
        {
            _context = context;
        }

        public void AddDepartment(string department)
        {
            _context.Add(new Department() { Name = department });
            _context.SaveChanges();
        }

        public void DeleteDepartment(int deptId)
        {
            var department = _context.Departments.FirstOrDefault(x => x.Id == deptId);
            _context.Remove(department);
        }

        public void EditDepartment(Department department)
        {
            var currentDepartment = _context.Departments.FirstOrDefault(x => x.Id == department.Id);
            currentDepartment.Name = department.Name;
            _context.SaveChanges();
        }

        public Department GetDepartmentById(int deptId)
        {
            return _context.Departments.FirstOrDefault(x => x.Id == deptId);
        }

        public IEnumerable<Department> GetDepartments()
        {
            return _context.Departments;
        }
    }
}
