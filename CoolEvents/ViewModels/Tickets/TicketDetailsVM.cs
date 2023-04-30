using System.ComponentModel;

namespace CoolEvents.ViewModels.Tickets
{
    public class TicketDetailsVM
    {
        public int TicketId { get; set; }
        public int EventId { get; set; }
        [DisplayName("Name of the event: ")]
        public string EventName { get; set; }

        [DisplayName("Description of the event: ")]
        public string EventDescription { get; set; }

        [DisplayName("Date of the event: ")]
        public DateTime EventDate { get; set; }

        [DisplayName("Count of tickets: ")]
        public int TicketCount { get; set; }
    }
}
