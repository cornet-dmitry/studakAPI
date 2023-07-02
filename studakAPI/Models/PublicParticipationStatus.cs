using System;
using System.Collections.Generic;

namespace studakAPI.Models
{
    public partial class PublicParticipationStatus
    {
        public PublicParticipationStatus()
        {
            PublicInvolvements = new HashSet<PublicInvolvement>();
        }

        public int Id { get; set; }
        public string ParticipationName { get; set; } = null!;

        public virtual ICollection<PublicInvolvement> PublicInvolvements { get; set; }
    }
}
