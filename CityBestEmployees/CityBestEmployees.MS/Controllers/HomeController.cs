using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
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
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CityBestEmployees.MS.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly EmployeeDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<Employee> _manager;
        private readonly SignInManager<Employee> _signinManager;

        public HomeController(IConfiguration configuration, EmployeeDbContext context, IWebHostEnvironment webHostEnvironment, UserManager<Employee> manager, SignInManager<Employee> signinManager)
        {
            _configuration = configuration;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _manager = manager;
            _signinManager = signinManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {

            ViewBag.Errors = null;
            ViewBag.departments = _context.Departments.Where(x => x.Name != "Admin").Select(x => x.Name);
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public async Task<IActionResult> GetUsers(DashBoardDto model)
        {
            if (ModelState.IsValid)
            {
                var client = new HttpClient();
                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", model.Token);
                message.Method = HttpMethod.Get;
                var json = JsonSerializer.Serialize(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                string url = "https://localhost:44388/Api/GetUsers";
                message.Content = content;
                message.RequestUri = new Uri(url);

                var response = await client.SendAsync(message);
                var responsebody = await response.Content.ReadAsStringAsync();
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(SignInDto model)
        {

            if (ModelState.IsValid)
            {
                var client = new HttpClient();
                HttpRequestMessage message = new HttpRequestMessage();
                message.Method = HttpMethod.Post;
                var json = JsonSerializer.Serialize(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                string url = "https://localhost:44388/Api/Login";
                message.Content = content;
                message.RequestUri = new Uri(url);

                var response = await client.SendAsync(message);
                var responsebody = await response.Content.ReadAsStringAsync();

                if(responsebody == "Incorrect Credentials")
                {
                    ViewBag.Error = responsebody;
                    return View();
                }
                var res = JsonSerializer.Deserialize<DashBoardDto>(responsebody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true});
                if (res.Role.Contains("Admin"))
                {

                    var client2 = new HttpClient();
                    HttpRequestMessage message2 = new HttpRequestMessage();
                    message2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", res.Token);
                    message2.Method = HttpMethod.Get;
                    //var json2 = JsonSerializer.Serialize(model);
                    //var content2 = new StringContent(json, Encoding.UTF8, "application/json");
                    string url2 = "https://localhost:44388/Api/GetUsers";
                    message2.RequestUri = new Uri(url2);

                    var response2 = await client2.SendAsync(message2);
                    var responsebody2 = await response2.Content.ReadAsStringAsync();
                }
                return RedirectToAction("Index", "Dashboard", res);
            }
            var details = new DashBoardDto();
            return View("Dashboard", details);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationDto model)
        {

            ViewBag.Departments = _context.Departments.Select(x => x.Name);
            if (ModelState.IsValid)
            {
                string photoLink = "";

                if (model.Photo != null)
                {
                    var folderLink = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    var photoName = Guid.NewGuid() + "_" + model.Photo.FileName;

                    photoLink = Path.Combine(folderLink, photoName);

                    using (var file = new FileStream(photoLink, FileMode.Create))
                    {
                        model.Photo.CopyTo(file);
                    }
                }
                var data = new ApiRegistrationDto
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Password = model.Password,
                    Email = model.Email,
                    ConfirmPassword = model.ConfirmPassword,
                    PhotoUrl = photoLink,
                    Department = model.Department
                };

                var client = new HttpClient();
                HttpRequestMessage message = new HttpRequestMessage();
                message.Method = HttpMethod.Post;
                var json = JsonSerializer.Serialize(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                string url = "https://localhost:44388/Api/Register";
                message.Content = content;
                message.RequestUri = new Uri(url);

                var response = await client.SendAsync(message); 
                var responsebody = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    return View("Login");
                }
            
            }
            return View("Register", model);
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

    }
}
