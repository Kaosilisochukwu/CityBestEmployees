using CityBestEmployees.MS.Models;
using System.Collections.Generic;

namespace CityBestEmployees.MS.Repositories
{
    public interface IDepartment
    {
        IEnumerable<IDepartment> GetDepartments();
        bool AddDepartment(Department department);
        bool DeleteDepartment(int deptId);
        bool EditDepartment(int deptId);
        Department GetDepartmentById(int deptId);
    }
}
