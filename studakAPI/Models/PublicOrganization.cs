using System;
using System.Collections.Generic;

namespace studakAPI.Models
{
    public partial class PublicOrganization
    {
        public PublicOrganization()
        {
            PublicActives = new HashSet<PublicActive>();
            PublicEvents = new HashSet<PublicEvent>();
        }

        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string ShortName { get; set; } = null!;
        public int Level { get; set; }
        public string? Address { get; set; }

        public virtual PublicOrganizationLevel LevelNavigation { get; set; } = null!;
        public virtual ICollection<PublicActive> PublicActives { get; set; }
        public virtual ICollection<PublicEvent> PublicEvents { get; set; }
    }
}
