using System;
using System.Collections.Generic;

namespace studakAPI.Models
{
    public partial class PublicUser
    {
        public PublicUser()
        {
            PublicActives = new HashSet<PublicActive>();
            PublicEvents = new HashSet<PublicEvent>();
            PublicInvolvements = new HashSet<PublicInvolvement>();
        }

        public int Id { get; set; }
        public string UserLogin { get; set; } = null!;
        public byte[] PasswordHash { get; set; } = null!;
        public byte[] PasswordSalt { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string? Surname { get; set; }
        public string? Name { get; set; }
        public string? Patronymic { get; set; }
        public DateTime? DateBirth { get; set; } 
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Messenger { get; set; }
        public int? Kpi { get; set; }

        public virtual ICollection<PublicActive>? PublicActives { get; set; }
        public virtual ICollection<PublicEvent>? PublicEvents { get; set; }
        public virtual ICollection<PublicInvolvement>? PublicInvolvements { get; set; }
    }
}
