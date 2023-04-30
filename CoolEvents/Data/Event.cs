using System.ComponentModel.DataAnnotations;

namespace CoolEvents.Data
{
    public class Event
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string imageUrl { get; set; } //?????
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
