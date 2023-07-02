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
    public class EventController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly studakContext _context;

        public EventController(IConfiguration configuration, studakContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        
        [Authorize]
        [HttpGet("AllEvents")]
        public async Task<ActionResult<PublicEvent>> AllEvents()
        {
            var events = await _context.PublicEvents
                .ToListAsync();

            return Ok(events);
        }
        
        [Authorize]
        [HttpPost("GetEvent")]
        public async Task<ActionResult<PublicEvent>> GetEvent([FromForm] string name)
        {
            var records = await _context.PublicEvents
                .Where(x => x.Name == name)
                .Select(x =>
                    new EventDto()
                    {
                        Id = x.Id,
                        Organization = x.OrganizationNavigation.FullName,
                        Responsible = x.ResponsibleNavigation.Surname + " " 
                                                                      + x.ResponsibleNavigation.Name + " " 
                                                                      + x.ResponsibleNavigation.Patronymic,
                        Name = x.Name,
                        Description = x.Description,
                        StartDate = x.StartDate,
                        EndDate = x.EndDate,
                        StartTime = x.StartTime,
                        EndTime = x.EndTime,
                        Rate = x.Rate
                    }).ToListAsync();

            return Ok(records);
        }
        
        [Authorize]
        [HttpPost("CheckEventName")]
        public async Task<ActionResult<PublicEvent>> CheckEventName([FromForm] string name)
        {
            var events = await _context.PublicEvents
                .Where(x => x.Name == name)
                .ToListAsync();

            if (events.Count > 0)
                return BadRequest("Используйте другое название");
            return Ok();
        }
        
        [HttpPost("AddEvent")]
        [Authorize(Roles = "1")]
        public async Task<ActionResult<PublicEvent>> AddEvent(
            [FromForm] string organization,
            [FromForm] string responsible,
            [FromForm] string name,
            [FromForm] string description,
            [FromForm] string startDate,
            [FromForm] string endDate,
            [FromForm] string startTime,
            [FromForm] string endTime,
            [FromForm] string rate)
        {
            PublicEvent events = new PublicEvent
            {
                Id = GetLastId(),
                Organization = int.Parse(organization),
                Responsible = int.Parse(responsible),
                Name = name,
                Description = description,
                StartDate = DateTime.Parse(startDate).ToUniversalTime(),
                EndDate = DateTime.Parse(endDate).ToUniversalTime(),
                StartTime = DateTime.Parse(startTime).ToUniversalTime(),
                EndTime = DateTime.Parse(endTime).ToUniversalTime(),
                Rate = int.Parse(rate)
            };
            _context.PublicEvents.Add(events);
            await _context.SaveChangesAsync();

            return Ok();
        }
        
        [HttpPost("DelEvent")]
        [Authorize(Roles = "1")]
        public async Task<ActionResult<PublicEvent>> DelEvent([FromForm] int eventId)
        {
            var delEvent = await _context.PublicEvents
                .Where(x => x.Id == eventId)
                .ToListAsync();

            if (delEvent.Count == 0)
                return BadRequest("Такого мероприятия нет!!");

            _context.PublicEvents.Remove(delEvent[0]);
            await _context.SaveChangesAsync();

            return Ok(delEvent);
        }
        
        private int GetLastId()
        {
            int lastId = _context.PublicEvents
                .OrderByDescending(u => u.Id)
                .Select(u => u.Id)
                .FirstOrDefault();

            return ++lastId;
        }
    }
}