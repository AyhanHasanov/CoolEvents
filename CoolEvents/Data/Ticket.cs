namespace CoolEvents.Data
{
    public class Ticket
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }
        public virtual ICollection<UserTickets> UserTickets { get; set; }
    }
}
