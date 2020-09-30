using CityBestEmployees.MS.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CityBestEmployees.MS.DTOs
{
    public class RegistrationDto
    {
        [Required]
       // [Display(Name = "First Name")]
        [MaxLength(50, ErrorMessage = "Firstname must not have more than 50 characters")]
        [MinLength(2, ErrorMessage = "Firstname must not have less than 2 characters")]
        public string FirstName { get; set; }
        [Required]
      //  [Display(Name = "Last Name")]
        [MaxLength(50, ErrorMessage = "Lastname must not have more than 50 characters")]
        [MinLength(2, ErrorMessage = "Lastname must not have less than 2 characters")]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        //public decimal Salary { get; set; }
        [Required]
        public IFormFile Photo { get; set; }
        //public Gender Gender { get; set; }
        [Required]
        [MaxLength(30)]
        public string Password { get; set; }
        [Required]
        public string Department { get; set; }
        [Required]
        //[Display(Name = "Confirm Password")]
        [MaxLength(30)]
        [Compare("Password", ErrorMessage = "Password does not match")]
        public string ConfirmPassword { get; set; }
    }
}
