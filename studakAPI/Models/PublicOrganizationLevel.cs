using System;
using System.Collections.Generic;

namespace studakAPI.Models
{
    public partial class PublicOrganizationLevel
    {
        public PublicOrganizationLevel()
        {
            PublicOrganizations = new HashSet<PublicOrganization>();
        }

        public int Id { get; set; }
        public string LevelName { get; set; } = null!;

        public virtual ICollection<PublicOrganization> PublicOrganizations { get; set; }
    }
}
