using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityBestEmployees.MS.DTOs
{
    public class DashBoardDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Department { get; set; }
        public string PhotoUrl { get; set; }
        public string Email { get; set; }
        public List<string> Role { get; set; }
        public string Token { get; set; }

    }
}
