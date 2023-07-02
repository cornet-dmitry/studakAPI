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
    public class OrganizationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly studakContext _context;

        public OrganizationController(IConfiguration configuration, studakContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        
        [Authorize]
        [HttpGet("AllOrgInfo")]
        public async Task<ActionResult<PublicOrganization>> AllOrganizationInfo()
        {
            var organizations = await _context.PublicOrganizations
                .ToListAsync();

            return Ok(organizations);
        }
        
        [Authorize]
        [HttpPost("GetOrgIdByShortName")]
        public async Task<ActionResult<PublicOrganization>> GetOrgIdByShortName([FromForm] string shortName)
        {
            var organizationId = await _context.PublicOrganizations
                .Where(x => x.ShortName == shortName)
                .Select(x => x.Id)
                .ToListAsync();

            return Ok(organizationId);
        }
        
        [Authorize]
        [HttpGet("AllOrgLevelInfo")]
        public async Task<ActionResult<PublicOrganizationLevel>> AllOrganizationLevelInfo()
        {
            var organizationLevels = await _context.PublicOrganizationLevels
                .ToListAsync();

            return Ok(organizationLevels);
        }

        [HttpPost("AddOrg")]
        [Authorize(Roles = "1")]
        public async Task<ActionResult<PublicOrganization>> AddOrganization(
            [FromForm] string fullName,
            [FromForm] string shortName,
            [FromForm] int levelOrg,
            [FromForm] string address)
        {
            var verify = await _context.PublicOrganizations
                .Where(x => x.FullName == fullName)
                .ToListAsync();
            if (verify.Count > 0)
                return BadRequest("Данное название организации уже имеется!");
        
            PublicOrganization orgData = new PublicOrganization
                { Id = GetLastId("PublicOrganizations"), 
                    FullName = fullName,
                    ShortName = shortName,
                    Level = levelOrg,
                    Address = address
                };
            _context.PublicOrganizations.Add(orgData);
            await _context.SaveChangesAsync();

            return Ok(orgData);
        }
        
        [HttpPost("DelOrg")]
        [Authorize(Roles = "1")]
        public async Task<ActionResult<PublicOrganization>> DelOrganization([FromForm] int orgId)
        {
            var delOrd = await _context.PublicOrganizations
                .Where(x => x.Id == orgId)
                .ToListAsync();
            
            if (delOrd.Count == 0)
                return BadRequest("Организации под таким номером нет!");

            _context.PublicOrganizations.Remove(delOrd[0]);
            await _context.SaveChangesAsync();
            
            return Ok(delOrd);
        }
        
        [HttpPost("AddOrgLevel")]
        [Authorize(Roles = "1")]
        public async Task<ActionResult<PublicOrganizationLevel>> AddOrganizationLevel(
            [FromForm] string levelName)
        {
            var verify = await _context.PublicOrganizationLevels
                .Where(x => x.LevelName == levelName)
                .ToListAsync();
            
            if (verify.Count > 0)
                return BadRequest("Данный уровень уже имеется!");
        
            PublicOrganizationLevel orgLevelData = new PublicOrganizationLevel
            { Id = GetLastId("PublicOrganizationLevels"), 
                LevelName = levelName
            };
            
            _context.PublicOrganizationLevels.Add(orgLevelData);
            await _context.SaveChangesAsync();

            return Ok(orgLevelData);
        }
        
        [HttpPost("DelOrgLevel")]
        [Authorize(Roles = "1")]
        public async Task<ActionResult<PublicOrganizationLevel>> DelOrganizationLevel([FromForm] int orgId)
        {
            var delOrd = await _context.PublicOrganizationLevels
                .Where(x => x.Id == orgId)
                .ToListAsync();
            
            if (delOrd.Count == 0)
                return BadRequest("Такого уровня не существует!");

            _context.PublicOrganizationLevels.Remove(delOrd[0]);
            await _context.SaveChangesAsync();
            
            return Ok(delOrd);
        }
        
        private int GetLastId(string tableName)
        {
            if (tableName == "PublicOrganizations")
            {
                int lastId = _context.PublicOrganizations
                    .OrderByDescending(u => u.Id)
                    .Select(u => u.Id)
                    .FirstOrDefault();
                return ++lastId;
            }
            if (tableName == "PublicOrganizationLevels")
            {
                int lastId = _context.PublicOrganizationLevels
                    .OrderByDescending(u => u.Id)
                    .Select(u => u.Id)
                    .FirstOrDefault();
                return ++lastId;
            }

            return 0;
        }
    }
}