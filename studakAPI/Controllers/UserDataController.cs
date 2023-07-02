using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using studakAPI.Context;
using studakAPI.Models;
using studakAPI.DTO;

namespace studakAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDataController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly studakContext _context;

        public UserDataController(IConfiguration configuration, studakContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        
        [Authorize]
        [HttpGet("AllUsers")]
        public async Task<ActionResult<PublicUser>> AllAUsers()
        {
            var active = await _context.PublicUsers
                .ToListAsync();

            return Ok(active);
        }
        
        [Authorize]
        [HttpPost("GetUser")]
        public async Task<ActionResult<PublicUser>> GetUser([FromForm] string userId)
        {
            var users = await _context.PublicUsers
                .Where(x => x.Id == int.Parse(userId))
                .ToListAsync();

            return Ok(users);
        }
        
        /*  Метод AddUserData позволяет заполнить все данные о пользователе после авторизации
         */

        [HttpPost("addUserData")]
        [Authorize]
        public async Task<ActionResult<PublicUser>> AddUserData(
            [FromForm] string userId,
            [FromForm] string surname,
            [FromForm] string name,
            [FromForm] string patronymic,
            [FromForm] string phone,
            [FromForm] string email,
            [FromForm] string messenger,
            [FromForm] string dateBirth) 
        {
            var data = await _context.PublicUsers
                .Where(x => Convert.ToString(x.Id) == userId)
                .ToListAsync();

            foreach (var d in data)
            {
                d.Surname = surname;
                d.Name = name;
                d.Patronymic = patronymic;
                d.Phone = phone;
                d.Email = email;
                d.Messenger = messenger;
                d.DateBirth = DateTime.Parse(dateBirth).ToUniversalTime();  //DateOnly.FromDateTime(DateTime.Parse(dateBirth));
            }
            
            await _context.SaveChangesAsync();
            return Ok(data);
        }
        
        [Authorize]
        [HttpGet("JWTinfo")]
        public IActionResult Test()
        {
            // Получение идентификатора пользователя из токена
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            // Получение имени пользователя из токена
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            
            // Получение роли пользователя из токена
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            var answer = userId + "\n" + username + "\n" + role;

            return Ok(answer);
        }
    }
}