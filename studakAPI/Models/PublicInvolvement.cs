using System;
using System.Collections.Generic;

namespace studakAPI.Models
{
    public partial class PublicInvolvement
    {
        public int Id { get; set; }
        public int Event { get; set; }
        public int User { get; set; }
        public int Status { get; set; }

        public virtual PublicEvent EventNavigation { get; set; } = null!;
        public virtual PublicParticipationStatus StatusNavigation { get; set; } = null!;
        public virtual PublicUser UserNavigation { get; set; } = null!;
    }
}
