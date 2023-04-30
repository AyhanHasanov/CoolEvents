using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoolEvents.ViewModels.Events
{
    public class EventCreateVM
    {
        [Required]
        [DisplayName("Event name: ")]
        [MaxLength(64, ErrorMessage = "Event name is too long!")]
        [MinLength(2, ErrorMessage = "Event name is too short!")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Description of the event: ")]
        [MaxLength(255, ErrorMessage = "Event description is too long!")]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DisplayName("Date of the event: ")]
        public DateTime Date { get; set; }

        [NotMapped]
        [DisplayName("Upload image: ")]
        public IFormFile? ImageFile { get; set; }

        public string? ImageUrl { get; set; }
    }
}
