using Microsoft.AspNetCore.Mvc;

namespace WebHost.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [HttpGet("GetAll")]
        public IEnumerable<User> Get()
        {
            User user1 = new();
            user1.FirstName = "Mauro Fabio";
            user1.LastName = "Hefti Bortolomeazzi";
            user1.Email = "mauro.hefti@diplomarbeit.ch";
            user1.IsAdmin = true;
            yield return user1;

            User user2 = new();
            user2.FirstName = "Aljoscha";
            user2.LastName = "Vogler";
            user2.Email = "aljoscha.vogler@diplomarbeit.ch";
            user2.IsAdmin = false;
            yield return user2;

            User user3 = new();
            user3.FirstName = "Lorenzo";
            user3.LastName = "Bittmann";
            user3.Email = "lorenzo.bittmann@diplomarbeit.ch";
            user3.IsAdmin = false;
            yield return user3;
        }
    }
}