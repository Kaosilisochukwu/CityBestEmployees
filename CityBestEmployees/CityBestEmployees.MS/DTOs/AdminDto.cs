using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityBestEmployees.MS.DTOs
{
    public class AdminDto : DashBoardDto
    {
        public List<DashBoardDto> Employees { get; set; }
    }
}
