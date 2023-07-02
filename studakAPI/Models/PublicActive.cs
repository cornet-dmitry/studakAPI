using System;
using System.Collections.Generic;

namespace studakAPI.Models
{
    public partial class PublicActive
    {
        public int Id { get; set; }
        public int Organization { get; set; }
        public int User { get; set; }
        public int Direction { get; set; }
        public string Position { get; set; } = null!;
        public DateOnly StartDate { get; set; }

        public virtual PublicWorkDirection DirectionNavigation { get; set; } = null!;
        public virtual PublicOrganization OrganizationNavigation { get; set; } = null!;
        public virtual PublicUser UserNavigation { get; set; } = null!;
    }
}
