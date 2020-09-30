using CityBestEmployees.MS.Models;
using System.Collections.Generic;

namespace CityBestEmployees.MS.Repositories
{
    public interface IDepartmentRepository
    {
        IEnumerable<Department> GetDepartments();
        void AddDepartment(string department);
        void DeleteDepartment(int deptId);
        void EditDepartment(Department department);
        Department GetDepartmentById(int deptId);
    }
}
