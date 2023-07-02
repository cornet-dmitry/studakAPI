using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using studakAPI.Context;
using studakAPI.Models;
using Microsoft.EntityFrameworkCore;
using studakAPI.DTO;

namespace studakAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static PublicUser user = new PublicUser();
        private readonly IConfiguration _configuration;
        private readonly studakContext _context;

        public AuthController(IConfiguration configuration, studakContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        /*  В методе Register происходит регистрация нового пользователя
            Пользователь передаёт связку ключ:значение (key:value)
            Сначала проверяется, если ли уже пользователь с таким именем. В случае, если нет -
            пароль передаётся в метод CreatePasswordHash(), который хэширует его, а также создаёт
            соль-ключ (Silt) для безопасности и возможности дальнейшей верификации
            После этого данные заносятся новой записью в БД
         */
        [HttpPost("register")]
        public async Task<ActionResult<PublicUser>> Register([FromForm] string username, [FromForm] string password)
        {
            var verify = await _context.PublicUsers
                .Where(x => x.UserLogin == username)
                .ToListAsync();
            
            if (verify.Count > 0)
                return BadRequest("Имя пользователя занято!");
            
            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            user.Id = GetLastId();
            user.UserLogin = username;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            user.Role = "0"; // роль по умолчанию 0 - обычный пользователь
            user.Kpi = 0; // KPI по умолчанию 0
            
            _context.PublicUsers.Add(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }
        
        /*  Ниже представлен метод Login (авторизация пользователя)
            Пользователь передаёт связку ключ:значение (key:value)
            По имени пользователя ищется запись в БД, из которой берётся Hash пароля и Silt пароля,
            которые вместе с введённым паролем пользователя при авторизации передаются в метод VerifyPasswordHash,
            где генерируется Hash на введёный пароль по ключу соли (Silt) и сравнивается на Истину
            
            Если Hash's совпали, тогда создаётся токен сессии, рефреш токен,
            после чего обновляется запись о пользователе в БД в соответствии с новыми данными,
            а именно рефреш токен, время его создания и время его жизни.
            
            Возвращается актуальный токен сессии (время жизни 5 минут)
         */
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromForm] string username, [FromForm] string password)
        {
            var verify = await _context.PublicUsers
                .Where(x => x.UserLogin == username)
                .ToListAsync();
            
            if (verify.Count == 0)
                return BadRequest("Неверное имя пользователя или пароль!");
            
            foreach (var v in verify)
            {
                if (VerifyPasswordHash(password, v.PasswordHash, v.PasswordSalt) == false)
                    return BadRequest("Неверное имя пользователя или пароль!");
            }
            
            string token = CreateToken(verify[0]);

            await _context.SaveChangesAsync();
            return Ok(token);
        }

        private string CreateToken(PublicUser user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, Convert.ToString(user.Id)),
                new Claim(ClaimTypes.Name, user.UserLogin),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
        
        

        /*  Метод GetLastId служит для получения последнего значения ID в таблице,
            т.к. автоинкремент не работает :(
         */
        private int GetLastId()
        {
            int lastId = _context.PublicUsers
                .OrderByDescending(u => u.Id)
                .Select(u => u.Id)
                .FirstOrDefault();

            return ++lastId;
        }
    }
}
