using System;
using System.Collections.Generic;

namespace studakAPI.Models
{
    public partial class PublicWorkDirection
    {
        public PublicWorkDirection()
        {
            PublicActives = new HashSet<PublicActive>();
        }

        public int Id { get; set; }
        public string DirectionName { get; set; } = null!;

        public virtual ICollection<PublicActive> PublicActives { get; set; }
    }
}
