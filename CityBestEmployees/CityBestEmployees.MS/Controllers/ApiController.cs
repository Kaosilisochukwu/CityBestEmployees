using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CityBestEmployees.MS.Context;
using CityBestEmployees.MS.DTOs;
using CityBestEmployees.MS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CityBestEmployees.MS.Controllers
{
    [Route("Api")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly EmployeeDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<Employee> _manager;
        private readonly SignInManager<Employee> _signinManager;

        public ApiController(IConfiguration configuration, EmployeeDbContext context, IWebHostEnvironment webHostEnvironment, UserManager<Employee> manager, SignInManager<Employee> signinManager)
        {
            _configuration = configuration;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _manager = manager;
            _signinManager = signinManager;
        }
        public IActionResult Index()
        {
            return Ok();
        }

        public IActionResult Register()
        {
            return Ok();
        }

        public IActionResult Login()
        {
            return Ok();
        }

        [AllowAnonymous]
        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login(SignInDto model)
        {

            var employee = _context.Users.FirstOrDefault(x => x.Email == model.Email);

            if (ModelState.IsValid && employee != null)
            {
                var token = GetToken(employee);
                await _signinManager.SignInAsync(employee, model.RememberMe);

                var roles = await _manager.GetRolesAsync(employee);
                return Ok(new DashBoardDto
                {
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Email = employee.Email,
                    PhotoUrl = employee.Photo,
                    Token = token,
                    Department = _context.Departments.FirstOrDefault(x => x.Id == employee.DepartmentId).Name,
                    Role = roles.ToList()

                });
            }

            return Ok("Incorrect Credentials");
        }

        [AllowAnonymous]
        [Route("Api/Register")]
        [HttpPost]
        public async Task<IActionResult> Register(ApiRegistrationDto model)
        {

            if (ModelState.IsValid)
            {
                var employee = new Employee
                {
                    UserName = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Photo = model.PhotoUrl,
                    Department = _context.Departments.FirstOrDefault(x => x.Name == model.Department)
                };
                var result = await _manager.CreateAsync(employee, model.Password);
                var errors = result.Errors;
                if (result.Succeeded)
                {
                    return Ok();
                }
            }
            return Ok(model);
        }

        public string GetToken(Employee loginDetails)
        {
            var employee = _manager.Users.FirstOrDefault(x => x.Email == loginDetails.Email);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, employee.Id),
                new Claim(ClaimTypes.Name, employee.LastName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = cred
            };
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(securityTokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("GetUsers")]
        public IActionResult GetUsers()
        {
            var res = _manager.Users.ToList();
            return Ok(res);
        }

    }
}
