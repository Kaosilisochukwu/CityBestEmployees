using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityBestEmployees.MS.Models
{
    public class Employee
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal Salary { get; set; }
        public string Photo { get; set; }
        public int GenderId { get; set; }
        public Gender Gender { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }

    }
}
