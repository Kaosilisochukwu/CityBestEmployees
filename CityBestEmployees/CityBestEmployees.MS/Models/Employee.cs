using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityBestEmployees.MS.Models
{
    public class Employee : IdentityUser
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Firstname must not have more than 50 characters")]
        [MinLength(2, ErrorMessage = "Firstname must not have less than 2 characters")]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "Lastname must not have more than 50 characters")]
        [MinLength(2, ErrorMessage = "Lastname must not have less than 2 characters")]
        public string LastName { get; set; }
        [Required]
        public decimal Salary { get; set; }
        public string Photo { get; set; }
        public Gender Gender { get; set; }
        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public Department Department { get; set; }

    }
}
