using System;
using System.Collections.Generic;

namespace studakAPI.Models
{
    public partial class PublicEvent
    {
        public PublicEvent()
        {
            PublicInvolvements = new HashSet<PublicInvolvement>();
        }

        public int Id { get; set; }
        public int Organization { get; set; }
        public int Responsible { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int Rate { get; set; }

        public virtual PublicOrganization OrganizationNavigation { get; set; } = null!;
        public virtual PublicUser ResponsibleNavigation { get; set; } = null!;
        public virtual ICollection<PublicInvolvement> PublicInvolvements { get; set; }
    }
}
