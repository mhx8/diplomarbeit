using Microsoft.AspNetCore.Mvc;

namespace WebHost.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ActiveDirectoryController : ControllerBase
    {
        private readonly ILogger<ActiveDirectoryController> _logger;

        public ActiveDirectoryController(ILogger<ActiveDirectoryController> logger)
        {
            _logger = logger;
        }

        [HttpGet("GetAll")]
        public IEnumerable<ActiveDirectory> Get()
        {
            ActiveDirectory entry1 = new();
            entry1.FirstName = "Mauro Roman";
            entry1.LastName = "Hefti";
            entry1.DateOfBirth = new DateTime(1997, 11, 25);
            entry1.Age = GetAge(entry1.DateOfBirth);
            entry1.Role = "Developer";
            yield return entry1;

            ActiveDirectory entry2 = new();
            entry2.FirstName = "Luana";
            entry2.LastName = "Hefti";
            entry2.DateOfBirth = new DateTime(2003, 1, 23);
            entry2.Age = GetAge(entry2.DateOfBirth);
            entry2.Role = "Manager";
            yield return entry2;

            ActiveDirectory entry3 = new();
            entry3.FirstName = "Corinne";
            entry3.LastName = "Hefti";
            entry3.DateOfBirth = new DateTime(1965, 12, 28);
            entry3.Age = GetAge(entry3.DateOfBirth);
            entry3.Role = "Boss";
            yield return entry3;

            ActiveDirectory entry4 = new();
            entry4.FirstName = "Lorenzo";
            entry4.LastName = "Bittmann";
            entry4.DateOfBirth = new DateTime(1980, 12, 28);
            entry4.Age = GetAge(entry4.DateOfBirth);
            entry4.Role = "Boss";
            yield return entry4;

            ActiveDirectory entry5 = new();
            entry5.FirstName = "Aljoscha";
            entry5.LastName = "Vogler";
            entry5.DateOfBirth = new DateTime(1980, 12, 28);
            entry5.Age = GetAge(entry5.DateOfBirth);
            entry5.Role = "Manager";
            yield return entry5;

            ActiveDirectory entry6 = new();
            entry6.FirstName = "Diplom";
            entry6.LastName = "Arbeit";
            entry6.DateOfBirth = new DateTime(1980, 12, 28);
            entry6.Age = GetAge(entry6.DateOfBirth);
            entry6.Role = "Lehrling";
            yield return entry6;
        }

        private static int GetAge(DateTime dateOfBirth)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - dateOfBirth.Year;

            if (dateOfBirth.Date > today.AddYears(-age))
            {
                age--;
            }

            return age;
        }
    }
}