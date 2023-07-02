using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace studakAPI.DTO
{
    public class User
    {
        public int Id { get; set; }
        public string UserLogin { get; set; } = null!;
        public byte[] PasswordHash { get; set; } = null!;
        public byte[] PasswordSalt { get; set; } = null!;
        public int? Role { get; set; }
        public string? Surname { get; set; }
        public string? Name { get; set; }
        public string? Patronymic { get; set; }
        public DateOnly? DateBirth { get; set; } 
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Messenger { get; set; }
        public int? Kpi { get; set; }
    }
}

