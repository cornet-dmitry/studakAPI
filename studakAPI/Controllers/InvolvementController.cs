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
    public class InvolvementController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly studakContext _context;

        public InvolvementController(IConfiguration configuration, studakContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        
        [Authorize]
        [HttpGet("AllStatus")]
        public async Task<ActionResult<PublicParticipationStatus>> AllStatus()
        {
            var statuses = await _context.PublicParticipationStatuses
                .ToListAsync();

            return Ok(statuses);
        }
        
        [Authorize]
        [HttpGet("AllInvolvement")]
        public async Task<ActionResult<PublicInvolvement>> AllInvolvement()
        {
            var involvement = await _context.PublicInvolvements
                .ToListAsync();

            return Ok(involvement);
        }
        
        [HttpPost("AddPInvolvement")]
        [Authorize(Roles = "1")]
        public async Task<ActionResult<PublicInvolvement>> AddInvolvements(
            [FromForm] int eventActive,
            [FromForm] int user,
            [FromForm] int status)
        {
            var verify = await _context.PublicInvolvements
                .Where(x => x.Event == eventActive)
                .Where(x => x.User == user)
                .ToListAsync();

            if (verify.Count > 0)
                return BadRequest("Данный пользователь уже подал заявку на это мероприятие!");

            PublicInvolvement part = new PublicInvolvement()
            {
                Id = GetLastId("PublicInvolvements"),
                Event = eventActive,
                User = user,
                Status = status
            };

            _context.PublicInvolvements.Add(part);
            await _context.SaveChangesAsync();

            return Ok(part);
        }

        [HttpPost("DelPInvolvement")]
        [Authorize(Roles = "1")]
        public async Task<ActionResult<PublicInvolvement>> AddInvolvements([FromForm] int statusId)
        {
            var delStatus = await _context.PublicInvolvements
                .Where(x => x.Id == statusId)
                .ToListAsync();

            if (delStatus.Count == 0)
                return BadRequest("Такого частника не существует!");

            _context.PublicInvolvements.Remove(delStatus[0]);
            await _context.SaveChangesAsync();

            return Ok(delStatus);
        }
        
        [HttpPost("AddPartStatus")]
        [Authorize(Roles = "1")]
        public async Task<ActionResult<PublicParticipationStatus>> AddParticipationStatus([FromForm] string statusName)
        {
            var verify = await _context.PublicParticipationStatuses
                .Where(x => x.ParticipationName == statusName)
                .ToListAsync();

            if (verify.Count > 0)
                return BadRequest("Данный статус уже существует!");

            PublicParticipationStatus statName = new PublicParticipationStatus
            {
                Id = GetLastId("PublicParticipationStatuses"),
                ParticipationName = statusName
            };

            _context.PublicParticipationStatuses.Add(statName);
            await _context.SaveChangesAsync();

            return Ok(statName);
        }

        [HttpPost("DelPartStatus")]
        [Authorize(Roles = "1")]
        public async Task<ActionResult<PublicParticipationStatus>> DelParticipationStatus([FromForm] int statusId)
        {
            var delStatus = await _context.PublicParticipationStatuses
                .Where(x => x.Id == statusId)
                .ToListAsync();

            if (delStatus.Count == 0)
                return BadRequest("Такого статуса не существует!");

            _context.PublicParticipationStatuses.Remove(delStatus[0]);
            await _context.SaveChangesAsync();

            return Ok(delStatus);
        }
        
        private int GetLastId(string tableName)
        {
            if (tableName == "PublicParticipationStatuses")
            {
                int lastId = _context.PublicParticipationStatuses
                    .OrderByDescending(u => u.Id)
                    .Select(u => u.Id)
                    .FirstOrDefault();
                return ++lastId;
            }

            if (tableName == "PublicInvolvements")
            {
                int lastId = _context.PublicInvolvements
                    .OrderByDescending(u => u.Id)
                    .Select(u => u.Id)
                    .FirstOrDefault();
                return ++lastId;
            }

            return 0;
        }
    }
}