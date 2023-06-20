using Microsoft.AspNetCore.Identity;

namespace Lab4.Models
{
    public class Role : IdentityRole<int>
    {
        public virtual ICollection<User>? Users { get; set; }
    }
}
