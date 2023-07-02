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
    public class ActiveController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly studakContext _context;

        public ActiveController(IConfiguration configuration, studakContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [Authorize]
        [HttpGet("AllActiveInfo")]
        public async Task<ActionResult<PublicActive>> AllActiveInfo()
        {
            var active = await _context.PublicActives
                .ToListAsync();

            return Ok(active);
        }
        
        [Authorize]
        [HttpGet("AllWorkDirectInfo")]
        public async Task<ActionResult<PublicWorkDirection>> AllWorkDirectionInfo()
        {
            var organizationLevels = await _context.PublicWorkDirections
                .ToListAsync();

            return Ok(organizationLevels);
        }

        [HttpPost("AddActiveUser")]
        [Authorize(Roles = "1")]
        public async Task<ActionResult<PublicActive>> AddActiveUser(
            [FromForm] int organization,
            [FromForm] int user,
            [FromForm] int direction,
            [FromForm] string position,
            [FromForm] string startDate)
        {
            var verify = await _context.PublicActives
                .Where(x => x.Organization == organization)
                .Where(x => x.User == user)
                .ToListAsync();
            if (verify.Count > 0)
                return BadRequest("Пользователь уже состоит в данной организации!");

            PublicActive activeUser = new PublicActive
            {
                Id = GetLastId("PublicActives"),
                Organization = organization,
                User = user,
                Direction = direction,
                Position = position,
                StartDate = DateOnly.FromDateTime(DateTime.Parse(startDate))
            };
            _context.PublicActives.Add(activeUser);
            await _context.SaveChangesAsync();

            return Ok(activeUser);
        }

        [HttpPost("DelActiveUser")]
        [Authorize(Roles = "1")]
        public async Task<ActionResult<PublicActive>> DelActiveUser([FromForm] int organization, [FromForm] int user)
        {
            var delActive = await _context.PublicActives
                .Where(x => x.Organization == organization)
                .Where(x => x.User == user)
                .ToListAsync();

            if (delActive.Count == 0)
                return BadRequest("Такой пользователь не состоит в данной организации!");

            _context.PublicActives.Remove(delActive[0]);
            await _context.SaveChangesAsync();

            return Ok(delActive);
        }

        [HttpPost("AddWorkDirect")]
        [Authorize(Roles = "1")]
        public async Task<ActionResult<PublicWorkDirection>> AddWorkDirection([FromForm] string directionName)
        {
            var verify = await _context.PublicWorkDirections
                .Where(x => x.DirectionName == directionName)
                .ToListAsync();

            if (verify.Count > 0)
                return BadRequest("Данное направдение деятельности уже имеется!");

            PublicWorkDirection directData = new PublicWorkDirection
            {
                Id = GetLastId("PublicWorkDirections"),
                DirectionName = directionName
            };

            _context.PublicWorkDirections.Add(directData);
            await _context.SaveChangesAsync();

            return Ok(directData);
        }

        [HttpPost("DelWorkDirect")]
        [Authorize(Roles = "1")]
        public async Task<ActionResult<PublicWorkDirection>> DelWorkDirection([FromForm] int orgId)
        {
            var delDirect = await _context.PublicWorkDirections
                .Where(x => x.Id == orgId)
                .ToListAsync();

            if (delDirect.Count == 0)
                return BadRequest("Такого уровня не существует!");

            _context.PublicWorkDirections.Remove(delDirect[0]);
            await _context.SaveChangesAsync();

            return Ok(delDirect);
        }

        private int GetLastId(string tableName)
        {
            if (tableName == "PublicOrganizations")
            {
                int lastId = _context.PublicActives
                    .OrderByDescending(u => u.Id)
                    .Select(u => u.Id)
                    .FirstOrDefault();
                return ++lastId;
            }

            if (tableName == "PublicWorkDirections")
            {
                int lastId = _context.PublicWorkDirections
                    .OrderByDescending(u => u.Id)
                    .Select(u => u.Id)
                    .FirstOrDefault();
                return ++lastId;
            }

            return 0;
        }
    }
}