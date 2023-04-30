using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoolEvents.Data
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [ForeignKey("RoleId")]
        public int RoleId { get; set; }
        public Role? Role { get; set; }
        public virtual ICollection<UserTickets> UserTickets { get; set; }
    }
}
